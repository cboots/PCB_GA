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
        public double XStd { get; set; }
        public double YStd { get; set; }

        public double Alpha { get; set; }
        public double Beta { get; set; }

        public int GenerationSize { get; set; }
        public int MaxGeneration { get; set; }

        public int WorkspaceWidth { get; set; }
        public int WorkspaceHeight { get; set; }

        public double MutationRateRotation { get; set; }
        public double MutationRateSwap { get; set; }
        public double MutationRateTranspose { get; set; }

        public double CrossoverWidth { get; set; }

    }
}
