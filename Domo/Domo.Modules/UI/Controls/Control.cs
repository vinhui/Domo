namespace Domo.Modules.UI.Controls
{
    public delegate void ClickEvent(UIModule owner);

    public abstract class Control
    {
        public string title;
        public string text;

        public int minWidth;
        public int minHeight;

        public UIModule owner { get; private set; }

        public Control(UIModule owner)
        {
            this.owner = owner;
        }
    }
}