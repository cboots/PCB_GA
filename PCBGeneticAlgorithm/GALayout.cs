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

        public ushort this[int x, int y]
        {
            get
            {
                return mLayout[x, y];
            }
            set
            {
                mLayout[x, y] = value;
            }
        }

        public int Width
        {
            get
            {
                if (mLayout != null)
                    return mLayout.GetLength(0);
                return 0;
            }
        }

        public int Height
        {
            get
            {
                if (mLayout != null)
                    return mLayout.GetLength(1);
                return 0;
            }
        }

        public double RawNetFitness { get; set; }
        public double RawAreaFitness { get; set; }
        public int RawConstraintViolations { get; set; }

        public double F1 { get; set; }
        public double F2 { get; set; }
        public double F3 { get; set; }

        public double Fitness { get; set; }

        public double VariedFitness { get; set; }

        private GAModuleLocation[] moduleLocations = null;
        public GAModuleLocation[] ModuleLocations { 
            get { return moduleLocations; } 
            set { moduleLocations = value; } 
        }


        public GALayout(int width, int height)
        {
            mLayout = new ushort[width, height];
        }


    }
}
