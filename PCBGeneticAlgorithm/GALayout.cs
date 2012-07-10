using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class GALayout
    {
        private int mWidth = 0;
        public int Width
        {
            get
            {
                return mWidth;
            }
        }

        private int mHeight = 0;
        public int Height
        {
            get
            {
                return mHeight;
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
            mWidth = ga.WorkspaceWidth;
            mHeight = ga.WorkspaceHeight;
            moduleLocations = new GAModuleLocation[ga.Modules.Length];
        }


        public static GALayout GenerateRandomLayout(GeneticAlgorithm ga)
        {
            GALayout layout = new GALayout(ga);

            for (int i = 0; i < ga.Modules.Length; i++)
            {
                bool placed = RandomlyPlaceModule(ga, layout, i, false);
                if (!placed)
                {
                    throw new Exception("Failed to place module. Could not find enough space for " + ga.Modules[i].ComponentReference);
                }
            }
            return layout;
        }

        public static bool RandomlyPlaceModule(GeneticAlgorithm ga, GALayout layout, int moduleIndex, bool checkAllCollisions)
        {
            int width = ga.WorkspaceWidth;
            int height = ga.WorkspaceHeight;

            GAModule mod = ga.Modules[moduleIndex];

            for (int tries = 0; tries < 500; tries++)
            {
                int rot = ga.rand.Next(4);//Rotation (0-3)

                //Get rotated module dims
                int mWidth = mod.getRotatedWidth(rot);
                int mHeight = mod.getRotatedHeight(rot);

                //Find upper left corner position, ignoring invalid right and lower regions
                int x = ga.rand.Next(width - mWidth);
                int y = ga.rand.Next(height - mHeight);

                bool fits = true;
                int max = checkAllCollisions ? layout.ModuleLocations.Length : moduleIndex;
                for (int m = 0; m < max; m++)
                {
                    if (m != moduleIndex)
                    {
                        if (layout.ModuleLocations[m].Intersects(x, y, mWidth, mHeight))
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
                    return true;
                }

            }
            return false;
        }

        private static void PlaceModule(GALayout layout, int moduleIndex, int rotation, int rotatedWidth, int rotatedHeight, int x, int y)
        {
            layout.ModuleLocations[moduleIndex] = new GAModuleLocation(moduleIndex, rotation, x, y, rotatedWidth, rotatedHeight);
        }

        public static bool CheckIfFits(GALayout layout, GAModuleLocation moduleLocation)
        {
            bool fits = true;
            for (int m = 0; m < layout.ModuleLocations.Length; m++)
            {
                if (layout.ModuleLocations[m].Intersects(moduleLocation))
                {
                    fits = false;
                    break;
                }
            }
            return fits;
        }

        /// <summary>
        /// Generates an array representation of the layout.  Not efficient, used for quick debugging.
        /// </summary>
        /// <returns></returns>
        public ushort[,] GenerateArray()
        {
            ushort[,] layout = new ushort[Width, Height];
            for (int i = 0; i < moduleLocations.Length; i++ )
            {

                ushort moduleID = (ushort)((i + 1) << 2);
                int x = moduleLocations[i].X;
                int y = moduleLocations[i].Y;
                int width = moduleLocations[i].Width;
                int height = moduleLocations[i].Height;
                int rotation = moduleLocations[i].Rotation;

                for (int m = x; m < x + width; m++)
                {
                    for (int n = y; n < y + height; n++)
                    {
                        if (m == x && n == y)
                        {
                            //Upper left corner (0)
                            layout[m, n] = (ushort)(moduleID | ((0 + rotation) % 4));

                        }
                        else if (m == x + width - 1 && n == y)
                        {
                            //Upper right corner (1)
                            layout[m, n] = (ushort)(moduleID | ((1 + rotation) % 4));
                        }
                        else if (m == x + width - 1 && n == y + height - 1)
                        {
                            //Lower right corner (2)
                            layout[m, n] = (ushort)(moduleID | ((2 + rotation) % 4));
                        }
                        else if (m == x && n == y + height - 1)
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
            return layout;
        }

    }
}
