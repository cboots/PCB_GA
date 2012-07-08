using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace PCBGeneticAlgorithm
{
    public class Crossover
    {
        private static Random sRand = new Random();
        public static Rectangle CrossOver(GeneticAlgorithm ga, GALayout parent1, GALayout parent2, out GALayout child1, out GALayout child2)
        {
            Rectangle region = CreateCrossoverRegion(ga.CrossoverWidth, parent1.Width, parent1.Height);

            List<int> crossover1 = new List<int>();
            List<int> crossover2 = new List<int>();

            child1 = new GALayout(ga);
            child2 = new GALayout(ga);

            for (int i = 0; i < ga.Modules.Length; i++)
            {
                if (parent1.ModuleLocations[i].Intersects(region))
                {
                    //In crossover region, directly inherit
                    child1.ModuleLocations[i] = parent1.ModuleLocations[i];
                }
                else
                {
                    //Not in crossover region, will be added in order later from parent2
                    crossover1.Add(i);
                }
                
                if (parent2.ModuleLocations[i].Intersects(region))
                {
                    //In crossover region, directly inherit
                    child2.ModuleLocations[i] = parent2.ModuleLocations[i];
                }
                else
                {
                    //Not in crossover region, will be added in order later from parent1
                    crossover2.Add(i);
                }   
            }

            //Finish child1
            InheritModules(ga, parent1, parent2, child1, crossover1);
            //Finish child2
            InheritModules(ga, parent2, parent1, child2, crossover2);

            return region;

        }

        private static void InheritModules(GeneticAlgorithm ga, GALayout parent1, GALayout parent2, GALayout child, List<int> moduleIndices)
        {
            for (int i = 0; i < moduleIndices.Count; i++)
            {
                int modIndex = moduleIndices[i];

                //Try to inherit from parent2
                if (GALayout.CheckIfFits(child, parent2.ModuleLocations[modIndex]))
                {
                    //Location from parent2 works.
                    child.ModuleLocations[modIndex] = parent2.ModuleLocations[modIndex];
                }//else, try to inherit position from parent1
                else if (GALayout.CheckIfFits(child, parent1.ModuleLocations[modIndex]))
                {
                    child.ModuleLocations[modIndex] = parent1.ModuleLocations[modIndex];
                }//else, randomly place this module
                else
                {
                    GALayout.RandomlyPlaceModule(ga, child, modIndex, true);
                }
            }
        }

        public static Rectangle CreateCrossoverRegion(double crossoverWidth, int width, int height)
        {
            int rx1 = sRand.Next(0, width);
            int ry1 = sRand.Next(0, height);

            int rx2 = sRand.Next((int)(-crossoverWidth * width), (int)(crossoverWidth * width));
            int ry2 = sRand.Next((int)(-crossoverWidth * height), (int)(crossoverWidth * height));

            //Bound rx2, ry2 by workspace size
            rx2 = Math.Min(width - 1, rx2);
            rx2 = Math.Max(0, rx2);
            ry2 = Math.Min(height - 1, ry2);
            ry2 = Math.Max(0, ry2);

            int xmin = Math.Min(rx1, rx2);
            int ymin = Math.Min(ry1, ry2);
            int rWidth = Math.Abs(rx2 - rx1);
            int rHeight = Math.Abs(ry2 - ry1);
            return new Rectangle(xmin, ymin, rWidth, rHeight);
        }


        internal static void CrossOver(GeneticAlgorithm ga, GALayout[] selected)
        {
            GALayout parent1;
            GALayout parent2;

            GALayout child1;
            GALayout child2;

            for (int i = 0; i < selected.Length; i++)
            {
                double x = sRand.NextDouble();
                if (x < ga.CrossoverRate)
                {
                    parent1 = selected[i];
                    int index2 = i;
                    while(index2 == i)
                    {
                        index2 = sRand.Next(0, selected.Length);
                    }
                    parent2 = selected[index2];
                    CrossOver(ga, parent1, parent2, out child1, out child2);
                    selected[i] = child1;
                    selected[index2] = child2;
                }
            }

        }
    }
}
