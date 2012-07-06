using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public struct GAModuleLocation
    {
        int mModuleID;
        public int ModuleID { get { return mModuleID; } }
        int mRotation;
        public int Rotation { get { return mRotation; } }
        int mX;
        public int X { get { return mX; } }
        int mY;
        public int Y { get { return mY; } }

        public GAModuleLocation(int modID, int rotation, int x, int y)
        {
            mModuleID = modID;
            mRotation = rotation;
            mX = x;
            mY = y;
        }
    }
}
