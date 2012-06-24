using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PCB_Layout_GA
{
    public abstract class Drawing
    {
        public int Width { get; set; }
        public int Layer { get; set; }

        public abstract int Left { get; }
        public abstract int Right { get; }
        public abstract int Top { get; }
        public abstract int Bottom { get; }

        public abstract void draw(Graphics graphicsObj);

    }

    public class ArcDrawing : Drawing
    {
        int CenterX { get; set; }
        int CenterY { get; set; }
        int X1 { get; set; }
        int Y1 { get; set; }
        int X2 { get { return CenterX - CenterY + Y1; } }
        int Y2 { get { return CenterX + CenterY - X1; } }
        int Angle { get; set; }

        //TODO Wrong
        public override int Left
        {
            get
            {
                int dx = X1 - CenterX;
                int dy = Y1 - CenterY;
                if (dx < 0 && dy < 0)
                {
                    //point 1 in quad 3, calculate radius
                    int radius = (int)Math.Sqrt(dx * dx + dy * dy);
                    return CenterX - radius - Width/2;
                }
                return Math.Min(X1, X2) - Width / 2;
            }
                
        }

        public override int Right
        {
            get
            {
                int dx = X1 - CenterX;
                int dy = Y1 - CenterY;
                if (dx > 0 && dy > 0)
                {
                    //point 1 in quad 1, calculate radius
                    int radius = (int)Math.Sqrt(dx * dx + dy * dy);
                    return CenterX + radius + Width / 2;
                }
                return Math.Max(X1, X2) + Width / 2;
            }
        }

        public override int Top
        {
            get
            {
                int dx = X1 - CenterX;
                int dy = Y1 - CenterY;
                if (dx < 0 && dy > 0)
                {
                    //point 1 in quad 2, calculate radius
                    int radius = (int)Math.Sqrt(dx * dx + dy * dy);
                    return CenterY + radius + Width / 2;
                }
                return Math.Max(Y1, Y2) + Width / 2;
            }
        }

        public override int Bottom
        {
            get
            {
                int dx = X1 - CenterX;
                int dy = Y1 - CenterY;
                if (dx > 0 && dy < 0)
                {
                    //point 1 in quad 4, calculate radius
                    int radius = (int)Math.Sqrt(dx * dx + dy * dy);
                    return CenterY - radius - Width / 2;
                }
                return Math.Min(Y1, Y2) - Width / 2;
            }
        }

        public override void draw(Graphics graphicsObj)
        {

        }

        public static Drawing parse(string line)
        {
            List<string> strings = LibraryParser.SplitBySpaces(line);
            ArcDrawing drawing = new ArcDrawing();
            drawing.CenterX = Int32.Parse(strings[1]);
            drawing.CenterY = Int32.Parse(strings[2]);
            drawing.X1 = Int32.Parse(strings[3]);
            drawing.Y1 = Int32.Parse(strings[4]);
            drawing.Angle = Int32.Parse(strings[5]);
            drawing.Width = Int32.Parse(strings[6]);
            drawing.Layer = Int32.Parse(strings[7]);
            return drawing;
        }
    }

    public class SegmentDrawing : Drawing
    {
        public int XStart { get; set; }
        public int YStart { get; set; }
        public int XEnd { get; set; }
        public int YEnd { get; set; }

        public override int Left
        {
            get { return Math.Min(XEnd, XStart) - Width / 2; }
        }

        public override int Right
        {
            get { return Math.Max(XEnd, XStart) + Width / 2; }
        }

        public override int Bottom
        {
            get { return Math.Min(YEnd, YStart) - Width / 2; }
        }

        public override int Top
        {
            get { return Math.Max(YEnd, YStart) + Width / 2; }
        }

        public override void draw(Graphics graphicsObj)
        {

        }

        public static Drawing parse(string line)
        {
            List<string> strings = LibraryParser.SplitBySpaces(line);
            SegmentDrawing drawing = new SegmentDrawing();
            drawing.XStart = Int32.Parse(strings[1]);
            drawing.YStart = Int32.Parse(strings[2]);
            drawing.XEnd = Int32.Parse(strings[3]);
            drawing.YEnd = Int32.Parse(strings[4]);
            drawing.Width = Int32.Parse(strings[5]);
            drawing.Layer = Int32.Parse(strings[6]);
            return drawing;
        }
    }

    public class CircleDrawing : Drawing
    {
        int XCenter { get; set; }
        int YCenter { get; set; }
        int XPoint { get; set; }
        int YPoint { get; set; }

        public override int Left
        {
            get {
                int dx = XCenter - XPoint;
                int dy = YCenter - YPoint;
                int radius = (int)Math.Sqrt(dx * dx + dy * dy);
                return XCenter - radius - Width / 2; 
            }
        }

        public override int Right
        {
            get
            {
                int dx = XCenter - XPoint;
                int dy = YCenter - YPoint;
                int radius = (int)Math.Sqrt(dx * dx + dy * dy);
                return XCenter + radius + Width/2;
            }

        }

        public override int Top
        {
            get
            {
                int dx = XCenter - XPoint;
                int dy = YCenter - YPoint;
                int radius = (int)Math.Sqrt(dx * dx + dy * dy);
                return YCenter + radius + Width / 2;
            }
        }

        public override int Bottom
        {
            get
            {
                int dx = XCenter - XPoint;
                int dy = YCenter - YPoint;
                int radius = (int)Math.Sqrt(dx * dx + dy * dy);
                return YCenter - radius - Width / 2;
            }
        }

        public override void draw(Graphics graphicsObj)
        {

        }

        public static Drawing parse(string line)
        {
            List<string> strings = LibraryParser.SplitBySpaces(line);
            CircleDrawing drawing = new CircleDrawing();
            drawing.XCenter = Int32.Parse(strings[1]);
            drawing.YCenter = Int32.Parse(strings[2]);
            drawing.XPoint = Int32.Parse(strings[3]);
            drawing.YPoint = Int32.Parse(strings[4]);
            drawing.Width = Int32.Parse(strings[5]);
            drawing.Layer = Int32.Parse(strings[6]);
            return drawing;
        }
    }
}
