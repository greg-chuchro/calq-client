#nullable enable
#pragma warning disable CS0649

using Ghbvft6.Calq.Client;
using System.Collections;
using System.Collections.Generic;

namespace Ghbvft6.Calq.ClientTest {

    public class CalqObjectList<T> : List<T>, ICalqObject, ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList where T : CalqObject {
        private ICalqObject? Parent { get; set; }
        private string? Name { get; set; }

        ICalqObject? ICalqObject.Parent => Parent;
        string? ICalqObject.Name => Name;

        public CalqObjectList() { }

        internal void Attach(ICalqObject parent, string name) {
            Parent = parent;
            Name = name;
        }

        public int Add(object? value) {
            ((T?)value)?.Attach(this, this.Count.ToString());
            Add((T?)value);
            return this.Count;
        }

        public void Insert(int index, object? value) {
            ((T?)value)?.Attach(this, this.Count.ToString());
            base.Insert(index, (T?)value);
        }

        new public void Add(T item) {
            item?.Attach(this, this.Count.ToString());
            base.Add(item);
        }

        new public void Insert(int index, T item) {
            item?.Attach(this, this.Count.ToString());
            base.Insert(index, item);
        }

        new public T this[int i] {
            get => base[i];
            set {
                value?.Attach(this, i.ToString());
                base[i] = value;
            }
        }
        new public void AddRange(IEnumerable<T> collection) {
            var count = this.Count;
            foreach (var item in collection) {
                item?.Attach(this, count.ToString());
                ++count;
            }
            base.AddRange(collection);
        }
    }
}
