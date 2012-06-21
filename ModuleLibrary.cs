using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCB_Layout_GA
{
    class ModuleLibrary
    {
        private Dictionary<string, Module> modules = new Dictionary<string, Module>();

        public void AddModule(Module mod)
        {
            if(!modules.ContainsKey(mod.LibName))
            {
                modules.Add(mod.LibName, mod);
            }
        }

        public Module FindModule(string libName)
        {
            Module mod = null;
            modules.TryGetValue(libName, out mod);
            return mod;
        }
    }
}
