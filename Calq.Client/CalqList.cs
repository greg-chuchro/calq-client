using System;
using System.Collections;
using System.Collections.Generic;

namespace Calq.Client {
    public class CalqList<T> : ICalqObject, ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList {
        private readonly CalqObject parent;
        private readonly string name;

        private List<T> list = new();

        ICalqObject ICalqObject.Parent => parent;

        string ICalqObject.Name => name;

        public int Count => ((ICollection<T>)list).Count;

        public bool IsReadOnly => ((ICollection<T>)list).IsReadOnly;

        public bool IsSynchronized => ((ICollection)list).IsSynchronized;

        public object SyncRoot => ((ICollection)list).SyncRoot;

        public bool IsFixedSize => ((IList)list).IsFixedSize;

        object? IList.this[int index] { get => ((IList)list)[index]; set => ((IList)list)[index] = value; }
        public T this[int index] { get => ((IList<T>)list)[index]; set => ((IList<T>)list)[index] = value; }

        protected CalqList(CalqObject parent, string name) {
            this.parent = parent;
            this.name = name;
        }

        public int IndexOf(T item) {
            return ((IList<T>)list).IndexOf(item);
        }

        public void Insert(int index, T item) {
            ((IList<T>)list).Insert(index, item);
        }

        public void RemoveAt(int index) {
            ((IList<T>)list).RemoveAt(index);
        }

        public void Add(T item) {
            ((ICollection<T>)list).Add(item);
        }

        public void Clear() {
            ((ICollection<T>)list).Clear();
        }

        public bool Contains(T item) {
            return ((ICollection<T>)list).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            ((ICollection<T>)list).CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            return ((ICollection<T>)list).Remove(item);
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)list).GetEnumerator();
        }

        public void CopyTo(Array array, int index) {
            ((ICollection)list).CopyTo(array, index);
        }

        public int Add(object? value) {
            return ((IList)list).Add(value);
        }

        public bool Contains(object? value) {
            return ((IList)list).Contains(value);
        }

        public int IndexOf(object? value) {
            return ((IList)list).IndexOf(value);
        }

        public void Insert(int index, object? value) {
            ((IList)list).Insert(index, value);
        }

        public void Remove(object? value) {
            ((IList)list).Remove(value);
        }
    }
}
