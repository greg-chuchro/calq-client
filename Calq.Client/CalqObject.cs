namespace Calq.Client {
    public abstract class CalqObject {
        private readonly CalqObject parent;
        private readonly string name;

        internal string Path {
            get {
                var path = name;
                var parent = this.parent;
                while (parent != null) {
                    path = $"{parent.name}/{path}";
                    parent = parent.parent;
                }
                return path;
            }
        }

        protected CalqObject(CalqObject parent, string name) {
            this.parent = parent;
            this.name = name;
        }
    }
}
