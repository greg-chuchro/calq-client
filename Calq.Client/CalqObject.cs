namespace Ghbvft6.Calq.Client {
    public abstract class CalqObject : ICalqObject {
        private readonly CalqObject parent;
        private readonly string name;

        ICalqObject ICalqObject.Parent => parent;

        string ICalqObject.Name => name;

        protected CalqObject(CalqObject parent, string name) {
            this.parent = parent;
            this.name = name;
        }
    }
}
