using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PCBGeneticAlgorithm
{
    public struct GAModuleLocation
    {
        int mRotation;
        public int Rotation { get { return mRotation; } }
        int mX;
        public int X { get { return mX; } }
        int mY;
        public int Y { get { return mY; } }
        int mHeight;
        /// <summary>
        /// Returns the height in current orientation
        /// </summary>
        public int Height { get { return mHeight; } }

        int mWidth;
        /// <summary>
        /// Returns the width in the current orientation
        /// </summary>
        public int Width { get { return mWidth; } }


        public GAModuleLocation(int rotation, int x, int y, int rotatedWidth, int rotatedHeight)
        {
            mRotation = rotation;
            mX = x;
            mY = y;
            mHeight = rotatedHeight;
            mWidth = rotatedWidth;
        }

        public bool Intersects(GAModuleLocation other)
        {
            return Intersects(other.X, other.Y, other.Width, other.Height);
        }

        public bool Intersects(Rectangle rectangle)
        {
            return Intersects(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public bool Intersects(int x, int y, int width, int height)
        {
            return !(((x+width) < mX) || //this right of other
                ((mX + mWidth) < x) || //this left of other
                ((y+height) < mY) ||  //this below other
                ((mY + mHeight) < y)); //this above other
        }

        public override string ToString()
        {
            return "{x=" + mX + ",y=" + mY + ",rot=" + mRotation + ",w=" + mWidth + ",h=" + mHeight + "}";
        }
    }
}
