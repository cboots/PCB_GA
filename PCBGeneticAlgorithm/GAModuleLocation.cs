using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class GAModuleLocation
    {
        private int moduleID;
        public int ModuleID { get { return moduleID; } }
        public int Rotation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public GAModuleLocation(int modID)
        {
            moduleID = modID;
        }
    }
}
