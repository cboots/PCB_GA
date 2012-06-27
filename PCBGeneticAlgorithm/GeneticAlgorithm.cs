using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class GeneticAlgorithm
    {

        public GAModule[] Modules { get; set; }
        public GANet[] Nets { get; set; }

        public int GridSize { get; set; }
    }
}
