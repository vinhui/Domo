using Domo.Modules.UI.Controls;
using System;
using System.Collections.Generic;

namespace Domo.Modules.UI
{
    public class UIModule
    {
        public ModuleBase owner { get; private set; }

        public string title;
        public string description;

        private List<Control> _controls;
        public IEnumerable<Control> controls { get { return _controls; } }

        public UIModule(ModuleBase owner)
        {
            this.owner = owner;
        }

        public Control AddControl(Func<UIModule, Control> create)
        {
            Control c = create(this);
            _controls.Add(c);
            return c;
        }
    }
}