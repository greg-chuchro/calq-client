#pragma warning disable CS0649

using Calq.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Calq.ClientTest {


    public class TestClient : CalqClient {
        public Calq.TestService service = new(null, "");
        public TestClient(string url) : base(new HttpClient { BaseAddress = new Uri(url) }) { }
    }


    namespace Calq {
        public class Nested : CalqObject {
            public int a;
            public int b;

            internal Nested(ICalqObject parent, string name) : base(parent, name) { }
        }

        public class TestService : CalqObject {
            internal TestService(ICalqObject parent, string name) : base(parent, name) {
                nullNested = new(this, "nullNested"); // TODO https://github.com/greg-chuchro/calq-server/issues/8
            }

            public int integer;
            public bool boolean;
            public Nested nested;
            public Nested nullNested;
            public string text;
            public string nullText;
            public CalqList<int> list;
            public CalqList<Nested> listOfObjects;
            public CalqDictionary<int, int> dictionary;
        }
    }
}
