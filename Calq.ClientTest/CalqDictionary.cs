#nullable enable

using Ghbvft6.Calq.Client;
using System.Collections.Generic;

namespace Ghbvft6.Calq.ClientTest {

    // TODO CalqObjectDictionary
    public class CalqDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ICalqObject where TKey : notnull {
        internal ICalqObject? Parent { get; set; }
        internal string? Name { get; set; }

        ICalqObject? ICalqObject.Parent => Parent;
        string? ICalqObject.Name => Name;

        public CalqDictionary() { }

        internal void Attach(ICalqObject parent, string name) {
            Parent = parent;
            Name = name;
        }
    }
}
