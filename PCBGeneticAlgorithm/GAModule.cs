using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCBGeneticAlgorithm
{
    public struct GAModule
    {
        string mComponentReference;
        public string ComponentReference { get { return mComponentReference;}}

        int mWidth;
        public int Width { get { return mWidth; } }

        int mHeight;
        public int Height { get { return mHeight; } }

        GAModulePin[] mPins;
        public GAModulePin[] Pins { get { return mPins; } }


        public GAModule(string compRef, int width, int height, GAModulePin[] pins)
        {
            mComponentReference = compRef;
            mWidth = width;
            mHeight = height;
            mPins = pins;
        }

        public int getRotatedWidth(int rot)
        {
            return ((rot % 2) == 0) ? Width : Height;
        }

        public int getRotatedHeight(int rot)
        {
            return ((rot % 2) == 0) ? Height : Width;
        }

        public struct GAModulePin
        {
            private int[] X;
            private int[] Y;
            private string mPinName;
            public string PinName { get { return mPinName; } }

            /// <summary>
            /// Takes the unrotated module pin coordinates and calculates out all the rotations.
            /// parent height and width must be initialized and the 
            /// x,y coordinates of the pin are given in 0-based offsets from unrotated upper-left corner of parent
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="parentHeight"></param>
            /// <param name="parentWidth"></param>
            /// <param name="parent"></param>
            public GAModulePin(string pinName, int x, int y, int parentWidth, int parentHeight)
            {
                mPinName = pinName;
                X = new int[4];
                Y = new int[4];
                X[0] = x;
                Y[0] = y;

                X[1] = Y[0];
                Y[1] = parentWidth - x - 1;

                X[2] = Y[1];
                Y[2] = parentHeight - y - 1;

                X[3] = Y[2];
                Y[3] = X[0];
            }

            /// <summary>
            /// Gets the X offset of this pin relative to the upper left corner of the parent module.
            /// Takes the rotation value of current upper left corner
            /// </summary>
            /// <param name="rotation"></param>
            /// <returns></returns>
            public int getX(int rotation)
            {
                if (X != null)
                    return X[rotation];
                return -1;
            }

            /// <summary>
            /// Gets the Y offset of this pin relative to the upper left corner of the parent module.
            /// Takes the rotation value of current upper left corner
            /// </summary>
            /// <param name="rotation"></param>
            /// <returns></returns>
            public int getY(int rotation)
            {
                if (Y != null)
                    return Y[rotation];
                return -1;
            }
        }
    }


}
