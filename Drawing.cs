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

        public abstract void draw(Graphics graphicsObj);
    }

    public class ArcDrawing : Drawing
    {
        int X0 { get; set; }
        int Y0 { get; set; }
        int X1 { get; set; }
        int Y1 { get; set; }
        int Angle { get; set; }

        public override void draw(Graphics graphicsObj)
        {

        }

        public static Drawing parse(string line)
        {
            List<string> strings = LibraryParser.SplitBySpaces(line);
            ArcDrawing drawing = new ArcDrawing();
            drawing.X0 = Int32.Parse(strings[1]);
            drawing.Y0 = Int32.Parse(strings[2]);
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
