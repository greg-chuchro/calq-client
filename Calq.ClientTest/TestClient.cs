#pragma warning disable CS0649

using Ghbvft6.Calq.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Ghbvft6.Calq.ClientTest {


    public class TestClient : CalqClient {
        public Calq.TestService service = new(null, "");
        public TestClient(string url) : base(new HttpClient { BaseAddress = new Uri(url) }) { }
    }


    namespace Calq {
        public class Nested : CalqObject {
            public int a;
            public int b;

            internal Nested(CalqObject parent, string name) : base(parent, name) { }
        }

        public class TestService : CalqObject {
            internal TestService(CalqObject parent, string name) : base(parent, name) {
                nullNested = new(this, "nullNested"); // TODO https://github.com/greg-chuchro/calq-server/issues/8
            }

            public int integer;
            public bool boolean;
            public Nested nested;
            public Nested nullNested;
            public string text;
            public string nullText;
            public CalqList<int> list;
            public CalqDictionary<int, int> dictionary;
        }
    }
}
