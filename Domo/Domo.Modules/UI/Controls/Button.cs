using System;

namespace Domo.Modules.UI.Controls
{
    public class Button : Control
    {
        public event ClickEvent onButtonClick;

        public Action onClick;

        public Button(UIModule owner) : base(owner)
        {
        }
    }
}