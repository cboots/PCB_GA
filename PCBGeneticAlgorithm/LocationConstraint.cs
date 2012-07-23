using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class LocationConstraint : IConstraint
    {
        public int ConstrainedModuleIndex { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int Rotation { get; set; }

        public LocationConstraint()
        {
            ConstrainedModuleIndex = -1;
            X = -1;
            Y = -1;
            Rotation = -1;
        }

        public bool ViolatesConstraint(GAModuleLocation[] locations)
        {
            GAModuleLocation loc = locations[ConstrainedModuleIndex];
            if (loc.X != X)
                return true;
            if (loc.Y != Y)
                return true;
            if (loc.Rotation != Rotation)
                return true;
            return false;
        }

    }
}
