#pragma warning disable CS0649

using System.Collections.Generic;

namespace Calq.ClientTest {
    public class TestService {
        public class Nested {
            public int a = 1;
            public int b;
        }

        public TestService() {
        }

        public int integer;
        public bool boolean;
        public Nested nested = new();
        public Nested nullNested;
        public string text = "text";
        public string nullText;
    }
}
