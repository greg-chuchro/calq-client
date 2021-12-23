#nullable enable

using Ghbvft6.Calq.Client;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ghbvft6.Calq.ClientTest {

    public class CalqObjectDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ICalqObject, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary, IDeserializationCallback, ISerializable where TKey : notnull where TValue : CalqObject {
        internal ICalqObject? Parent { get; set; }
        internal string? Name { get; set; }

        ICalqObject? ICalqObject.Parent => Parent;
        string? ICalqObject.Name => Name;

        public CalqObjectDictionary() { }

        internal void Attach(ICalqObject parent, string name) {
            Parent = parent;
            Name = name;
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            item.Value?.Attach(this, item.Key.ToString());
            base.Add(item.Key, item.Value);
        }

        public void Add(object key, object? value) {
            ((TValue)value).Attach(this, ((TKey)key).ToString());
            base.Add((TKey)key, (TValue)value);
        }

        public object? this[object key] {
            get {
                TValue? value;
                base.TryGetValue((TKey)key, out value);
                return value;
            }
            set {
                ((TValue)value)?.Attach(this, ((TKey)key).ToString());
                base[(TKey)key] = (TValue)value;
            }
        }

        new public TValue this[TKey i] {
            get => base[i];
            set {
                value?.Attach(this, i.ToString());
                base[i] = value;
            }
        }
    }
}
