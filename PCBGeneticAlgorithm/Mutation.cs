using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public class Mutation
    {
        private static Random sRand = new Random();

        public static void Mutate(GeneticAlgorithm ga, GALayout[] selected)
        {

            for (int i = 0; i < selected.Length; i++)
            {
                double x = sRand.NextDouble();
                if (x < ga.MutationRateRotation)
                {
                    RotateMutaion(ga, selected[i]);
                }

                x = sRand.NextDouble();
                if (x < ga.MutationRateSwap)
                {
                    SwapMutaion(ga, selected[i]);
                }

                x = sRand.NextDouble();
                if (x < ga.MutationRateTranspose)
                {
                    TransposeMutaion(ga, selected[i]);
                }

            }
        }

        public static void TransposeMutaion(GeneticAlgorithm ga, GALayout layout)
        {
            int index = sRand.Next(0, layout.ModuleLocations.Length);
            GALayout.RandomlyPlaceModule(ga, layout, index, true);
        }

        public static void SwapMutaion(GeneticAlgorithm ga, GALayout layout)
        {
            int index1 = sRand.Next(0, layout.ModuleLocations.Length);
            int index2 = index1;
            while (index1 == index2)
            {
                index2 = sRand.Next(0, layout.ModuleLocations.Length);
            }

            GAModuleLocation loc1 = layout.ModuleLocations[index1];
            GAModuleLocation loc2 = layout.ModuleLocations[index2];

            GAModule mod1 = ga.Modules[index1];
            GAModule mod2 = ga.Modules[index2];

            //Clear the modules to make space
            layout.ModuleLocations[index1] = new GAModuleLocation();
            layout.ModuleLocations[index2] = new GAModuleLocation();


            GAModuleLocation newLoc1 = new GAModuleLocation(loc2.Rotation, loc2.X, loc2.Y, 
                mod1.getRotatedWidth(loc2.Rotation), mod1.getRotatedHeight(loc2.Rotation));

            GAModuleLocation newLoc2 = new GAModuleLocation(loc1.Rotation, loc1.X, loc1.Y, 
                mod2.getRotatedWidth(loc1.Rotation), mod2.getRotatedHeight(loc1.Rotation));

            TryReplace(ga, layout, index1, newLoc1);
            TryReplace(ga, layout, index2, newLoc2);

        }

        private static void TryReplace(GeneticAlgorithm ga, GALayout layout, int modIndex, GAModuleLocation newLocation)
        {
            if (GALayout.CheckIfFits(layout, newLocation))
            {
                layout.ModuleLocations[modIndex] = newLocation;
            }
            else
            {
                //Try rotating
                newLocation = new GAModuleLocation((newLocation.Rotation + 1) % 4,
                    newLocation.X, newLocation.Y, newLocation.Height, newLocation.Width);
                if (GALayout.CheckIfFits(layout, newLocation))
                {
                    layout.ModuleLocations[modIndex] = newLocation;
                }
                else
                {
                    //Randomly place
                    GALayout.RandomlyPlaceModule(ga, layout, modIndex, true);
                }
            }
        }

        public static void RotateMutaion(GeneticAlgorithm ga, GALayout layout)
        {
            int index = sRand.Next(0, layout.ModuleLocations.Length);
            GAModuleLocation loc = layout.ModuleLocations[index]; 
            layout.ModuleLocations[index] = new GAModuleLocation();

            GAModuleLocation loc1 =  new GAModuleLocation((loc.Rotation + 1) % 4, loc.X, loc.Y, loc.Height, loc.Width);
            if (GALayout.CheckIfFits(layout, loc))
            {
                layout.ModuleLocations[index] = loc1;
            }
            else
            {
                //TODO Try other orientations or rotations
                GALayout.RandomlyPlaceModule(ga, layout, index, true);
            }
        }
    }
}
