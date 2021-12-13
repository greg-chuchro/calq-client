#nullable enable

using Ghbvft6.Calq.Client;

namespace Ghbvft6.Calq.ClientTest {

    public class CalqObject : ICalqObject {
        private ICalqObject? Parent { get; set; }
        private string? Name { get; set; }

        ICalqObject? ICalqObject.Parent => Parent;
        string? ICalqObject.Name => Name;

        public CalqObject() { }

        internal void Attach(ICalqObject parent, string name) {
            Parent = parent;
            Name = name;
        }
    }
}
