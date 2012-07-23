using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public interface IConstraint
    {
        bool ViolatesConstraint(GAModuleLocation[] locations);

    }
}
