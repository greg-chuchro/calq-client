using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Calq.Client {
    public class CalqDictionary<TKey, TValue> : ICalqObject, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary, IDeserializationCallback, ISerializable where TKey : notnull {
        private readonly CalqObject parent;
        private readonly string name;

        private Dictionary<TKey, TValue> dictionary = new();

        ICalqObject ICalqObject.Parent => parent;

        string ICalqObject.Name => name;

        public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)dictionary).Keys;

        public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)dictionary).Values;

        public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)dictionary).Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)dictionary).Values;

        public bool IsFixedSize => ((IDictionary)dictionary).IsFixedSize;

        ICollection IDictionary.Keys => ((IDictionary)dictionary).Keys;

        ICollection IDictionary.Values => ((IDictionary)dictionary).Values;

        public bool IsSynchronized => ((ICollection)dictionary).IsSynchronized;

        public object SyncRoot => ((ICollection)dictionary).SyncRoot;

        public object? this[object key] { get => ((IDictionary)dictionary)[key]; set => ((IDictionary)dictionary)[key] = value; }
        public TValue this[TKey key] { get => ((IDictionary<TKey, TValue>)dictionary)[key]; set => ((IDictionary<TKey, TValue>)dictionary)[key] = value; }

        protected CalqDictionary(CalqObject parent, string name) {
            this.parent = parent;
            this.name = name;
        }

        public void Add(TKey key, TValue value) {
            ((IDictionary<TKey, TValue>)dictionary).Add(key, value);
        }

        public bool ContainsKey(TKey key) {
            return ((IDictionary<TKey, TValue>)dictionary).ContainsKey(key);
        }

        public bool Remove(TKey key) {
            return ((IDictionary<TKey, TValue>)dictionary).Remove(key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) {
            return ((IDictionary<TKey, TValue>)dictionary).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Add(item);
        }

        public void Clear() {
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)dictionary).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((System.Collections.IEnumerable)dictionary).GetEnumerator();
        }

        public void Add(object key, object? value) {
            ((IDictionary)dictionary).Add(key, value);
        }

        public bool Contains(object key) {
            return ((IDictionary)dictionary).Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() {
            return ((IDictionary)dictionary).GetEnumerator();
        }

        public void Remove(object key) {
            ((IDictionary)dictionary).Remove(key);
        }

        public void CopyTo(Array array, int index) {
            ((ICollection)dictionary).CopyTo(array, index);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            ((ISerializable)dictionary).GetObjectData(info, context);
        }

        public void OnDeserialization(object? sender) {
            ((IDeserializationCallback)dictionary).OnDeserialization(sender);
        }
    }
}
