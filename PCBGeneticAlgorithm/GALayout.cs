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


        public GALayout(GeneticAlgorithm ga)
        {
            mLayout = new ushort[ga.WorkspaceWidth, ga.WorkspaceHeight];
            moduleLocations = new GAModuleLocation[ga.Modules.Length];
        }


        public static GALayout GenerateRandomLayout(GeneticAlgorithm ga)
        {
            GALayout layout = new GALayout(ga);

            //Reverse order.  Larger chips tend to have U* designations
            //More efficient
            for (int i = ga.Modules.Length-1; i >= 0; i-- )
            {
                bool placed = RandomlyPlaceModule(ga, layout, i);
                if (!placed)
                {
                    throw new Exception("Failed to place module. Could not find enough space for " + ga.Modules[i].ComponentReference);
                }
            }
            return layout;
        }

        private static bool RandomlyPlaceModule(GeneticAlgorithm ga, GALayout layout, int moduleIndex)
        {
            int width = ga.WorkspaceWidth;
            int height = ga.WorkspaceHeight;

            GAModule mod = ga.Modules[moduleIndex];

            for (int tries = 0; tries < 500; tries++)
            {
                int rot = ga.rand.Next(4);//Rotation (0-3)

                //Get rotated module dims
                int mWidth = ((rot % 2) == 0) ? mod.Width : mod.Height;
                int mHeight = ((rot % 2) == 0) ? mod.Height : mod.Width;

                //Find upper left corner position, ignoring invalid right and lower regions
                int x = ga.rand.Next(width - mWidth);
                int y = ga.rand.Next(height - mHeight);

                bool fits = true;
                for (int m = x; m < x + mWidth; m++)
                {
                    if (!fits)
                        break;

                    for (int n = y; n < y + mHeight; n++)
                    {
                        if (layout[m, n] != 0)
                        {
                            fits = false;
                            break;
                        }
                    }
                }
                if (fits)
                {
                    //Place module
                    PlaceModule(layout, moduleIndex, rot, mWidth, mHeight, x, y);
                    //Console.WriteLine("Tries for " + Modules[moduleIndex].ComponentReference + "=" + tries);
                    return true;
                }

            }
            return false;
        }

        private static void PlaceModule(GALayout layout, int moduleIndex, int rotation, int mWidth, int mHeight, int x, int y)
        {
            ushort moduleID = (ushort)((moduleIndex + 1) << 2);
            layout.ModuleLocations[moduleIndex] = new GAModuleLocation(moduleIndex + 1, rotation, x, y);

            for (int m = x; m < x + mWidth; m++)
            {
                for (int n = y; n < y + mHeight; n++)
                {
                    if (m == x && n == y)
                    {
                        //Upper left corner (0)
                        layout[m, n] = (ushort)(moduleID | ((0 + rotation) % 4));

                    }
                    else if (m == x + mWidth - 1 && n == y)
                    {
                        //Upper right corner (1)
                        layout[m, n] = (ushort)(moduleID | ((1 + rotation) % 4));
                    }
                    else if (m == x + mWidth - 1 && n == y + mHeight - 1)
                    {
                        //Lower right corner (2)
                        layout[m, n] = (ushort)(moduleID | ((2 + rotation) % 4));
                    }
                    else if (m == x && n == y + mHeight - 1)
                    {
                        //Lower left corner (3)
                        layout[m, n] = (ushort)(moduleID | ((3 + rotation) % 4));
                    }
                    else
                    {
                        //All others
                        layout[m, n] = moduleID;
                    }
                }
            }
        }

    }
}
