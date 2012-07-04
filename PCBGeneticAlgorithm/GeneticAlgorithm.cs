using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private Random rand = new Random();

        public GAModule[] Modules { get; set; }
        public GANet[] Nets { get; set; }

        public int GridSize { get; set; }
        public double XStd { get; set; }
        public double YStd { get; set; }

        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double Gamma { get; set; }

        public int GenerationSize { get; set; }
        public int MaxGeneration { get; set; }

        public int WorkspaceWidth { get; set; }
        public int WorkspaceHeight { get; set; }

        public double MutationRateRotation { get; set; }
        public double MutationRateSwap { get; set; }
        public double MutationRateTranspose { get; set; }

        public double CrossoverWidth { get; set; }


        public GALayout GenerateRandomLayout()
        {
            GALayout layout = new GALayout(WorkspaceWidth, WorkspaceHeight);

            //Reverse order.  Larger chips tend to have U* designations
            //More efficient
            for (int i = Modules.Length-1; i >= 0; i-- )
            {
                bool placed = RandomlyPlaceModule(layout.Layout, i);
                if (!placed)
                {
                    throw new Exception("Failed to place module. Could not find enough space for " + Modules[i].ComponentReference);
                }
            }
            return layout;
        }

        private bool RandomlyPlaceModule(ushort[,] layout, int moduleIndex)
        {
            int width = layout.GetLength(0);
            int height = layout.GetLength(1);

            GAModule mod = Modules[moduleIndex];

            for(int tries = 0; tries < 500; tries++)
            {
                int rot = rand.Next(4);//Rotation (0-3)

                //Get rotated module dims
                int mWidth = ((rot % 2) == 0) ? mod.Width : mod.Height;
                int mHeight = ((rot % 2) == 0) ? mod.Height : mod.Width;

                //Find upper left corner position, ignoring invalid right and lower regions
                int x = rand.Next(width - mWidth);
                int y = rand.Next(height - mHeight);

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

        private static void PlaceModule(ushort[,] layout, int moduleIndex, int rotation, int mWidth, int mHeight, int x, int y)
        {
            ushort moduleID = (ushort)((moduleIndex + 1) << 2);
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
