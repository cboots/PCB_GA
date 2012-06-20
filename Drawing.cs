using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PCB_Layout_GA
{
    public abstract class Drawing
    {
        public abstract void draw(Graphics graphicsObj);
    }

    public class ArcDrawing : Drawing
    {
        public override void draw(Graphics graphicsObj)
        {

        }
    }

    public class SegmentDrawing : Drawing
    {
        public override void draw(Graphics graphicsObj)
        {

        }
    }

    public class CircleDrawing : Drawing
    {
        public override void draw(Graphics graphicsObj)
        {

        }
    }
}
