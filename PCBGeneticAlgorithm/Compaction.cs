using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PCBGeneticAlgorithm
{
    public class Compaction
    {

        public static GAModuleLocation[] Compact(GAModuleLocation[] locations)
        {
            GAModuleLocation[] compacted = CompactX(locations);
            compacted = CompactY(compacted);
            return compacted;
        }

        public class MyXSorter : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                GAModuleLocation loc1 = (GAModuleLocation)x;
                GAModuleLocation loc2 = (GAModuleLocation)y;

                return loc1.X.CompareTo(loc2.X);
            }
        }

        
        public class MyYSorter : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                GAModuleLocation loc1 = (GAModuleLocation)x;
                GAModuleLocation loc2 = (GAModuleLocation)y;

                return loc1.Y.CompareTo(loc2.Y);
            }

        }

        public static GAModuleLocation[] CompactX(GAModuleLocation[] locations)
        {
            //Sort by X
            GAModuleLocation[] locCopy = new GAModuleLocation[locations.Length];
            Array.Copy(locations, locCopy, locCopy.Length);
            Array.Sort(locCopy, new MyXSorter());

            //For each ModuleLocation in ascending X order
            for(int i = 0; i < locCopy.Length; i++)
            {
                int rightEdge = 0;
                int YMin = locCopy[i].Y;
                int YMax = locCopy[i].Y + locCopy[i].Height;
                for(int j = 0; j < i; j++)
                {
                   if(locCopy[j].Y < YMax && (locCopy[j].Y+locCopy[j].Height) > YMin)
                   {
                       //Overlaps
                       if(rightEdge < locCopy[j].X + locCopy[j].Width)
                       {
                           //furthest right
                           rightEdge = locCopy[j].X + locCopy[j].Width;
                       }
                   }
                }
                //Move this block over to rightedge
                locCopy[i] = new GAModuleLocation(locCopy[i].ModIndex, locCopy[i].Rotation, 
                    rightEdge, locCopy[i].Y, locCopy[i].Width, locCopy[i].Height);

            }

            GAModuleLocation[] compacted = new GAModuleLocation[locCopy.Length];
            //Copy compacted layout back
            for (int i = 0; i < locCopy.Length; i++)
            {
                compacted[locCopy[i].ModIndex] = locCopy[i];
            }
            return compacted;
        }

        public static GAModuleLocation[] CompactY(GAModuleLocation[] locations)
        {
            //Sort by Y
            GAModuleLocation[] locCopy = new GAModuleLocation[locations.Length];
            Array.Copy(locations, locCopy, locCopy.Length);
            Array.Sort(locCopy, new MyYSorter());

            //For each ModuleLocation in ascending X order
            for(int i = 0; i < locCopy.Length; i++)
            {
                int bottomEdge = 0;
                int XMin = locCopy[i].X;
                int XMax = locCopy[i].X + locCopy[i].Width;
                for(int j = 0; j < i; j++)
                {
                   if(locCopy[j].X < XMax && (locCopy[j].X+locCopy[j].Width) > XMin)
                   {
                       //Overlaps
                       if(bottomEdge < locCopy[j].Y + locCopy[j].Height)
                       {
                           //furthest down
                           bottomEdge = locCopy[j].Y + locCopy[j].Height;
                       }
                   }
                }
                //Move this block up to bottomEdge
                locCopy[i] = new GAModuleLocation(locCopy[i].ModIndex, locCopy[i].Rotation, 
                    locCopy[i].X, bottomEdge, locCopy[i].Width, locCopy[i].Height);

            }

            GAModuleLocation[] compacted = new GAModuleLocation[locCopy.Length];
            //Copy compacted layout
            for (int i = 0; i < locCopy.Length; i++)
            {
                compacted[locCopy[i].ModIndex] = locCopy[i];
            }
            return compacted;
        }
   

        

    }
}
