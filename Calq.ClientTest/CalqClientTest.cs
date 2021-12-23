using Ghbvft6.Calq.ClientTest.Calq;
using Ghbvft6.Calq.Server;
using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using Xunit;

namespace Ghbvft6.Calq.ClientTest {
    public class CalqClientTest {

        protected TestService root = new();
        protected TestClient client;
        private int port = 8380;

        public CalqClientTest() {
            var url = "";

            var server = new CalqServer(root);
            new Thread(() => {
                while (server.Listener.IsListening == false) {
                    try {
                        url = $"http://localhost:{port}/";
                        server.Prefixes = new[] { url };
                        server.Start();
                    } catch (HttpListenerException) {
                        ++port;
                        server = new CalqServer(root);
                    }
                }
            }).Start();

            while (server.Listener.IsListening == false) {
                Thread.Sleep(1);
            }
            client = new TestClient($"http://localhost:{port}/");
        }

        protected string Serialize(object instance) {
            var serializerOptions = new JsonSerializerOptions {
                IncludeFields = true
            };
            return JsonSerializer.Serialize(instance, serializerOptions);
        }

        protected string SerializeDeserialize(object instance, Type type) {
            var serialized = Serialize(instance);
            var deserialized = JsonSerializer.Deserialize(serialized, type);
            return Serialize(deserialized);
        }

        [Fact]
        public void Test0() {
            Assert.NotNull(root.nested);
            Assert.Null(root.nullNested);
            Assert.Equal(0, root.integer);

            Assert.NotNull(client.service.nullNested);
        }

        [Fact]
        public void Test1() {
            client.Get(client.service);
            Assert.Equal(SerializeDeserialize(root, root.GetType()), SerializeDeserialize(client.service, root.GetType()));
        }

        [Fact]
        public void Test2() {
            client.Get(client.service);
            client.service.nested.b = 2;
            client.Put(client.service.nested);
            Assert.Equal(Serialize(root.nested), Serialize(client.service.nested));
        }

        [Fact]
        public void Test3() {
            client.Post(client.service.nullNested);
            Assert.Equal(Serialize(root.nullNested), Serialize(client.service.nullNested));
        }

        [Fact]
        public void Test4() {
            client.Get(client.service);
            client.Delete(client.service.nested);
            Assert.Null(root.nested);
        }

        [Fact]
        public void Test5() {
            client.Get(client.service);
            client.service.listOfObjects[0].b = 2;
            client.Patch(client.service.listOfObjects[0]);
            Assert.Equal(Serialize(root.listOfObjects[0]), Serialize(client.service.listOfObjects[0]));
        }

        [Fact]
        public void Test6() {
            client.Get(client.service);
            client.service.nested = new Nested() { b = 2 };
            client.Put(client.service.nested);
            Assert.Equal(Serialize(root.nested), Serialize(client.service.nested));
        }

        [Fact]
        public void Test7() {
            client.Get(client.service);
            client.service.listOfObjects[0] = new Nested() { b = 2 };
            client.Patch(client.service.listOfObjects[0]);
            Assert.Equal(Serialize(root.listOfObjects[0]), Serialize(client.service.listOfObjects[0]));
        }

        [Fact]
        public void Test8() {
            client.Get(client.service);
            client.service.listOfObjects[0].b = 2;
            client.Patch(client.service.listOfObjects[0]);
            Assert.Equal(Serialize(root.listOfObjects[0]), Serialize(client.service.listOfObjects[0]));
        }

        [Fact]
        public void Test9() {
            client.Get(client.service);
            client.service.dictionaryOfObjects[0] = new Nested() { b = 2 };
            client.Patch(client.service.dictionaryOfObjects[0]);
            Assert.Equal(Serialize(root.dictionaryOfObjects[0]), Serialize(client.service.dictionaryOfObjects[0]));
        }

        [Fact]
        public void Test10() {
            client.Get(client.service);
            client.service.dictionaryOfObjects[0].b = 2;
            client.Patch(client.service.dictionaryOfObjects[0]);
            Assert.Equal(Serialize(root.dictionaryOfObjects[0]), Serialize(client.service.dictionaryOfObjects[0]));
        }
    }
}
