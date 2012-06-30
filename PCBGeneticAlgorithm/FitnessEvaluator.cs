using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class FitnessEvaluator
    {

        public static void LocateModules(GeneticAlgorithm ga, GALayout layout)
        {
            if (layout.ModuleLocations == null || layout.ModuleLocations.Length != ga.Modules.Length)
            {
                layout.ModuleLocations = new GAModuleLocation[ga.Modules.Length];//Initialize module lengths
            }

            for (int i = 0; i < layout.ModuleLocations.Length; i++)
            {
                //Clear array
                layout.ModuleLocations[i] = null;
            }

            int width = layout.Width;
            int height = layout.Height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (layout[x, y] != 0)
                    {
                        int index = (layout[x, y] >> 2) - 1;//Zero based index
                        if (layout.ModuleLocations[index] == null)
                        {
                            layout.ModuleLocations[index] = new GAModuleLocation(index + 1);
                            layout.ModuleLocations[index].X = x;
                            layout.ModuleLocations[index].Y = y;
                            layout.ModuleLocations[index].Rotation = layout[x, y] & 0x03;

                        }
                        //Skip rotated module width
                        int mWidth = ((layout.ModuleLocations[index].Rotation % 2) == 0) ? ga.Modules[index].Width : ga.Modules[index].Height;
                        x = x + mWidth - 1;
                            
                    }
                }
            }
        }
    }
}
