namespace Ghbvft6.Calq.Client {
    public interface ICalqObject {
        protected ICalqObject? Parent { get; }
        protected string? Name { get; }

        internal string? Path {
            get {
                var path = Name;
                var parent = Parent;
                while (parent != null) {
                    path = $"{parent.Name}/{path}";
                    parent = parent.Parent;
                }
                return path;
            }
        }
    }
}
