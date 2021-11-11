using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Calq.Client {
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

            reader.Read();
            if (reader.TokenType != JsonTokenType.StartObject) {
                throw new JsonException("json must be an object");
            }

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
                        currentType = currentInstance.GetType();
                        continue;
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
                    default:
                        switch (reader.TokenType) {
                            case JsonTokenType.StartObject:
                                instanceStack.Push(currentInstance);
                                currentInstance = Reflection.GetOrInitializeFieldOrPropertyValue(currentType, currentInstance, propertyName);
                                if (currentInstance == null) {
                                    throw new JsonException();
                                }
                                currentType = currentInstance.GetType();
                                break;
                            case JsonTokenType.StartArray:
                                break;
                            default:
                                throw new JsonException();
                        }
                        continue;
                }
                Reflection.SetFieldOrPropertyValue(currentType, currentInstance, propertyName, value);
            }
        }

        public void Get(CalqObject obj) {
            var response = Send(HttpMethod.Get, obj.Path);
            Populate(response.body, obj);
        }

        public void Post(CalqObject obj) {
            Send(HttpMethod.Post, obj.Path, JsonSerializer.Serialize(obj, obj.GetType(), serializerOptions));
        }

        public void Put(CalqObject obj) {
            Send(HttpMethod.Put, obj.Path, JsonSerializer.Serialize(obj, obj.GetType(), serializerOptions));
        }

        public void Delete(CalqObject obj) {
            Send(HttpMethod.Delete, obj.Path);
        }

        public void Patch(CalqObject obj) {
            Send(HttpMethod.Patch, obj.Path, JsonSerializer.Serialize(obj, obj.GetType(), serializerOptions));
        }
    }
}
