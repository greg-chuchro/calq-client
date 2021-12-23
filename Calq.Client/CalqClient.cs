using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Ghbvft6.Calq.Client {
    public class CalqClient {

        private readonly HttpClient client;

        private readonly JsonSerializerOptions serializerOptions = new() {
            IncludeFields = true
        };
        private readonly JsonReaderOptions readerOptions = new() {
            CommentHandling = JsonCommentHandling.Skip
        };

        public CalqClient(HttpClient client) {
            this.client = client;
        }

        internal (string body, HttpStatusCode code) Send(HttpMethod method, string uri, string body = "") {
            var request = new HttpRequestMessage(method, uri);
            request.Content = new StringContent(body);
            var response = client.Send(request);
            if ((int)response.StatusCode / 100 != 2) {
                throw new Exception($"{response.StatusCode}");
            }
            return (new StreamReader(response.Content.ReadAsStream()).ReadToEnd(), response.StatusCode);
        }

        private void Populate(string text, object instance) {
            var jsonBytes = Encoding.UTF8.GetBytes(text);
            var reader = new Utf8JsonReader(jsonBytes, readerOptions);
            Populate(reader, instance);
        }

        private void Populate(Utf8JsonReader reader, object instance) {
            if (instance == null) {
                throw new ArgumentException("instance can't be null");
            }

            object? currentInstance = instance;
            var currentType = instance.GetType();
            var instanceStack = new Stack<object>();

            void ReadObject(ref Utf8JsonReader reader) {
                while (true) {
                    reader.Read();
                    string propertyName;
                    switch (reader.TokenType) {
                        case JsonTokenType.PropertyName:
                            propertyName = reader.GetString()!;
                            break;
                        case JsonTokenType.EndObject:
                            if (instanceStack.Count == 0) {
                                if (reader.Read()) {
                                    throw new JsonException();
                                }
                                return;
                            }
                            currentInstance = instanceStack.Pop();
                            if (currentInstance is not ICollection) {
                                currentType = currentInstance.GetType();
                                continue;
                            } else {
                                currentType = currentInstance.GetType().GetGenericArguments()[0];
                                return;
                            }
                        default:
                            throw new JsonException();
                    }

                    reader.Read();
                    object? value;
                    switch (reader.TokenType) {
                        case JsonTokenType.False:
                        case JsonTokenType.True:
                            value = reader.GetBoolean();
                            break;
                        case JsonTokenType.String:
                            value = reader.GetString();
                            break;
                        case JsonTokenType.Number:
                            value = reader.GetInt32();
                            break;
                        case JsonTokenType.Null:
                            value = null;
                            break;
                        case JsonTokenType.StartObject:
                            instanceStack.Push(currentInstance);
                            if (currentInstance is not ICollection) {
                                currentInstance = Reflection.GetOrInitializeFieldOrPropertyValue(currentType, currentInstance, propertyName);
                            } else {
                                currentInstance = Reflection.GetOrInitializeChildValue((ICollection)currentInstance, propertyName);
                            }
                            if (currentInstance == null) {
                                throw new JsonException();
                            }
                            currentType = currentInstance.GetType();
                            continue;
                        case JsonTokenType.StartArray:
                            instanceStack.Push(currentInstance);
                            value = Reflection.GetOrInitializeFieldOrPropertyValue(currentType, currentInstance, propertyName);
                            if (currentInstance is not ICollection) {
                                Reflection.SetFieldOrPropertyValue(currentType, currentInstance, propertyName, value);
                            } else {
                                Reflection.SetChildValue((ICollection)currentInstance, propertyName, value);
                            }
                            currentInstance = value;
                            if (currentInstance == null) {
                                throw new JsonException();
                            }
                            currentType = currentInstance.GetType().GetGenericArguments()[0];
                            ReadArray(ref reader);
                            continue;
                        default:
                            throw new JsonException();
                    }
                    if (currentInstance is not ICollection) {
                        Reflection.SetFieldOrPropertyValue(currentType, currentInstance, propertyName, value);
                    } else {
                        Reflection.SetChildValue((ICollection)currentInstance, propertyName, value);
                    }
                }
            }

            void ReadArray(ref Utf8JsonReader reader) {
                while (true) {
                    reader.Read();
                    object? value;
                    switch (reader.TokenType) {
                        case JsonTokenType.False:
                        case JsonTokenType.True:
                            value = reader.GetBoolean();
                            break;
                        case JsonTokenType.String:
                            value = reader.GetString();
                            break;
                        case JsonTokenType.Number:
                            value = reader.GetInt32();
                            break;
                        case JsonTokenType.Null:
                            value = null;
                            break;
                        case JsonTokenType.StartObject:
                            instanceStack.Push(currentInstance);
                            value = Activator.CreateInstance(currentType); // FIXME
                            Reflection.AddChildValue((ICollection)currentInstance, value);
                            currentInstance = value;
                            if (currentInstance == null) {
                                throw new JsonException();
                            }
                            currentType = currentInstance.GetType();
                            ReadObject(ref reader);
                            continue;
                        case JsonTokenType.StartArray:
                            instanceStack.Push(currentInstance);
                            value = Activator.CreateInstance(currentType); // FIXME
                            Reflection.AddChildValue((ICollection)currentInstance, value);
                            currentInstance = value;
                            if (currentInstance == null) {
                                throw new JsonException();
                            }
                            currentType = currentInstance.GetType().GetGenericArguments()[0];
                            continue;
                        case JsonTokenType.EndArray:
                            if (instanceStack.Count == 0) {
                                if (reader.Read()) {
                                    throw new JsonException();
                                }
                                return;
                            }
                            currentInstance = instanceStack.Pop();
                            if (currentInstance is not ICollection) {
                                currentType = currentInstance.GetType();
                                return;
                            } else {
                                currentType = currentInstance.GetType().GetGenericArguments()[0];
                                continue;
                            }
                            break;
                        default:
                            throw new JsonException();
                    }
                    Reflection.AddChildValue((ICollection)currentInstance, value);
                }
            }

            reader.Read();
            switch (reader.TokenType) {
                case JsonTokenType.StartObject:
                    ReadObject(ref reader);
                    break;
                case JsonTokenType.StartArray:
                    ReadArray(ref reader);
                    break;
                default:
                    throw new JsonException();
            }
        }

        public void Get(ICalqObject obj) {
            var response = Send(HttpMethod.Get, obj.Path);
            Populate(response.body, obj);
        }

        public void Post(ICalqObject obj) {
            Send(HttpMethod.Post, obj.Path, JsonSerializer.Serialize(obj, obj.GetType(), serializerOptions));
        }

        public void Put(ICalqObject obj) {
            Send(HttpMethod.Put, obj.Path, JsonSerializer.Serialize(obj, obj.GetType(), serializerOptions));
        }

        public void Delete(ICalqObject obj) {
            Send(HttpMethod.Delete, obj.Path);
        }

        public void Patch(ICalqObject obj) {
            Send(HttpMethod.Patch, obj.Path, JsonSerializer.Serialize(obj, obj.GetType(), serializerOptions));
        }
    }
}
