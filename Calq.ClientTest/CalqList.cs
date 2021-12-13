#nullable enable

using Ghbvft6.Calq.Client;
using System.Collections.Generic;

namespace Ghbvft6.Calq.ClientTest {

    public class CalqList<T> : List<T>, ICalqObject {
        private ICalqObject? Parent { get; set; }
        private string? Name { get; set; }

        ICalqObject? ICalqObject.Parent => Parent;
        string? ICalqObject.Name => Name;

        public CalqList() { }

        internal void Attach(ICalqObject parent, string name) {
            Parent = parent;
            Name = name;
        }
    }
}
