#nullable enable
#pragma warning disable CS0649

using Ghbvft6.Calq.Client;
using System;
using System.Net.Http;

namespace Ghbvft6.Calq.ClientTest {

    public class TestClient : CalqClient {
        public Calq.TestService service = new();
        public TestClient(string url) : base(new HttpClient { BaseAddress = new Uri(url) }) { }
    }

    namespace Calq {
        public class Nested : CalqObject {
            public int a;
            public int b;
        }

        public class TestService : CalqObject {

            private Nested? _nested;
            private Nested? _nullNested;
            private CalqList<int>? _list;
            private CalqObjectList<Nested>? _listOfObjects;
            private CalqObjectDictionary<int, Nested>? _dictionaryOfObjects;

            public TestService() {
                nullNested = new(); // TODO https://github.com/greg-chuchro/calq-server/issues/8
            }

            public int integer;
            public bool boolean;
            public Nested? nested { get => _nested; set { value?.Attach(this, nameof(nested)); _nested = value; } }
            public Nested? nullNested { get => _nullNested; set { value?.Attach(this, nameof(nullNested)); _nullNested = value; } }
            public string? text;
            public string? nullText;
            public CalqList<int>? list { get => _list; set { value?.Attach(this, nameof(list)); _list = value; } }
            public CalqObjectList<Nested>? listOfObjects { get => _listOfObjects; set { value?.Attach(this, nameof(listOfObjects)); _listOfObjects = value; } }
            public CalqDictionary<int, int>? dictionary;
            public CalqObjectDictionary<int, Nested>? dictionaryOfObjects { get => _dictionaryOfObjects; set { value?.Attach(this, nameof(dictionaryOfObjects)); _dictionaryOfObjects = value; } }
        }
    }
}
