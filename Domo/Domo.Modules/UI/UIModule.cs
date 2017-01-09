using Domo.Modules.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domo.Modules.UI
{
    public class UIModule
    {
        // TODO: All the modules probably shouldn't be kept in here
        private static List<UIModule> modules = new List<UIModule>();
        private static List<IUIModule> moduleInterfaces = new List<IUIModule>();

        public static void RegisterModule(UIModule module)
        {
            modules.Add(module);
        }
        public static void RegisterModule(IUIModule module)
        {
            moduleInterfaces.Add(module);
        }
        public static IEnumerable<UIModule> GetModules()
        {
            List<UIModule> m = new List<UIModule>(modules.Count + moduleInterfaces.Count);

            m.AddRange(modules);
            m.AddRange(moduleInterfaces.Select(x => x.GetUI()));

            return m;
        }

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