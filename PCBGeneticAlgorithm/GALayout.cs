using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class GALayout
    {
        private ushort[,] mLayout;

        public ushort[,] Layout { get { return mLayout; } }

        public double F1 { get; set; }
        public double F2 { get; set; }
        public double F3 { get; set; }

        public double Fitness { get; set; }
        public double VariedFitness { get; set; }

        public GALayout(int width, int height)
        {
            mLayout = new ushort[width, height];
        }
    }
}
