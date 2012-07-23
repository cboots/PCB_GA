using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class ConstraintSet
    {
        public List<LocationConstraint> LocationConstraints { get; set; }

        public int CountViolations(GAModuleLocation[] locations)
        {
            int count = 0;
            foreach (IConstraint cons in LocationConstraints)
            {
                if (cons.ViolatesConstraint(locations))
                    count++;
            }
            return count;
        }
    }  
}
