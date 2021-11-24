using Ghbvft6.Calq.Server;
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
            Assert.Equal(Serialize(root), Serialize(client.service));
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
    }
}
