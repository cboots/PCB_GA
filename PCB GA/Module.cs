using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PCBGeneticAlgorithm
{
    public class Module
    {
        public List<Pad> mPads = new List<Pad>();
        public List<Drawing> mDrawings = new List<Drawing>();

        public int XPos { get; set; }
        public int YPos { get; set; }
        public int Orientation { get; set; }

        public int Layer { get; set; }


        //Ignore timestamp
        public bool Fixed { get; set; }
        public bool Autoplaced { get; set; }

        public String LibName { get; set; }
        public String CommentDescription { get; set; }
        public String Keywords { get; set; }

        //public int RotationCost90 { get; set; }
        //public int RotationCost180 { get; set; }

        public String ComponentReference { get; set; }
        public String ComponentValue { get; set; }

        public Rectangle getBoundingRectangle()
        {
            int minX = Int32.MaxValue;
            int maxX = Int32.MinValue;
            int minY = Int32.MaxValue;
            int maxY = Int32.MinValue;

            foreach (Drawing drawing in mDrawings)
            {
                int left = drawing.Left;
                int right = drawing.Right;
                int top = drawing.Top;
                int bottom = drawing.Bottom;
                if (left < minX)
                    minX = left;
                if (right > maxX)
                    maxX = right;
                if (bottom > maxY)
                    maxY = bottom;
                if (top < minY)
                    minY = top;
            }

            foreach (Pad pad in mPads)
            {
                int left = pad.Left;
                int right = pad.Right;
                int top = pad.Top;
                int bottom = pad.Bottom;
                if (left < minX)
                    minX = left;
                if (right > maxX)
                    maxX = right;
                if (bottom > maxY)
                    maxY = bottom;
                if (top < minY)
                    minY = top;
            }

            //Return in cartesian coordinates, not screen coordinates
            return new Rectangle(minX, minY, maxX - minX, maxY - minY); 
        }


        public static Module parse(string[] lines, ref int currentLine)
        {
            Module module = new Module();
            if (!lines[currentLine].StartsWith("$MODULE"))
            {
                //Not pad
                return null;
            }

            List<string> strings = LibraryParser.SplitBySpaces(lines[currentLine]);
            module.LibName = strings[1];

            while (!lines[currentLine].StartsWith("$EndMODULE"))
            {
                parseLine(lines, ref currentLine, module);
                
                //Next Line
                currentLine++;
            }

            return module;
        }

        private static void parseLine(string[] lines, ref int currentLine, Module module)
        {
            string line = lines[currentLine];
            if (line.StartsWith("Po"))
            {
                List<string> strings = LibraryParser.SplitBySpaces(line);
                module.XPos = Int32.Parse(strings[1]);
                module.YPos = Int32.Parse(strings[2]);
                module.Orientation = Int32.Parse(strings[3]);
                module.Layer = Int32.Parse(strings[4]);
                switch (strings[7])
                {
                    case "F~":
                        module.Fixed = true;
                        module.Autoplaced = false;
                        break;
                    case "~P":
                        module.Fixed = false;
                        module.Autoplaced = true;
                        break;
                    case "FP":
                        module.Fixed = true;
                        module.Autoplaced = true;
                        break;
                    default:
                        module.Fixed = false;
                        module.Autoplaced = false;
                        break;
                }
            }
            else if(line.StartsWith("Li"))
            {
                List<string> strings = LibraryParser.SplitBySpaces(line);
                module.LibName = strings[1];
            }
            else if (line.StartsWith("Cd"))
            {
                module.CommentDescription = line.Substring(3);
            }
            else if (line.StartsWith("Kw"))
            {
                module.Keywords = line.Substring(3);
            }
            else if (line.StartsWith("T"))
            {
                List<string> strings = LibraryParser.SplitBySpaces(line);
                if (line[1] == '0')
                {
                    //Parse T0
                    module.ComponentReference = strings[strings.Count - 1];
                }
                else if (line[1] == '1')
                {
                    //Parse T0
                    module.ComponentValue = strings[strings.Count - 1];
                }
            }
            else if (line.StartsWith("DA"))
            {
                Drawing drawing = ArcDrawing.parse(line);
                module.mDrawings.Add(drawing);
            }
            else if (line.StartsWith("DS"))
            {
                Drawing drawing = SegmentDrawing.parse(line);
                module.mDrawings.Add(drawing);
            }
            else if (line.StartsWith("DC"))
            {
                Drawing drawing = CircleDrawing.parse(line);
                module.mDrawings.Add(drawing);
            }
            else if (line.StartsWith("$PAD"))
            {
                Pad pad = Pad.parse(lines, ref currentLine);
                module.mPads.Add(pad);
            }
        }


        public class Pad
        {
            public enum PadType {CIRCLE, RECTANGULAR, OBLONG, TRAPEZE};
            public enum PadAttr { STD, SMD, CONN, HOLE, MECA };

            public int X { get; set; }
            public int Y { get; set; }

            public int XSize { get; set; }
            public int YSize { get; set; }
            public int Orientation { get; set; }
            public int XDelta { get; set; }
            public int YDelta { get; set; }

            public string PadName { get; set; }

            public int NetNumber { get; set; }
            public string NetName { get; set; }

            public PadType Type { get; set; }
            public PadAttr Attr { get; set; }
            public int DrillSize { get; set; }
            public int DrillXOffset { get; set; }
            public int DrillYOffset { get; set; }

            public int Left
            {
                get { return X - XSize/2; }
            }

            public int Right
            {
                get { return X + XSize / 2; }
            }

            public  int Bottom
            {
                //+Y is down
                get { return Y + YSize/2; }
            }

            public  int Top
            {
                get { return Y - YSize / 2; }
            }

            public static Pad parse(string[] lines, ref int currentLine)
            {
                Pad pad = new Pad();
                if (!lines[currentLine].StartsWith("$PAD"))
                {
                    //Not pad
                    return null;
                }
                //Go to next line
                currentLine++;
                while(!lines[currentLine].StartsWith("$EndPAD"))
                {
                    parseLine(lines[currentLine], pad);
                    currentLine++;
                }

                return pad;
            }

            private static void parseLine(string currentLine, Pad pad)
            {
                if (currentLine.StartsWith("Sh"))
                {
                    List<string> strings = LibraryParser.SplitBySpaces(currentLine);
                    //Parse out packet
                    pad.PadName = strings[1];

                    switch (strings[2])
                    {
                        case "T":
                            pad.Type = PadType.TRAPEZE;
                            break;
                        case "O":
                            pad.Type = PadType.OBLONG;
                            break;
                        case "R":
                            pad.Type = PadType.RECTANGULAR;
                            break;
                        default:
                            pad.Type = PadType.CIRCLE;
                            break;
                    }

                    pad.XSize = Int32.Parse(strings[3]);
                    pad.YSize = Int32.Parse(strings[4]);
                    pad.XDelta = Int32.Parse(strings[5]);
                    pad.YDelta = Int32.Parse(strings[6]);
                    pad.Orientation = Int32.Parse(strings[7]);

                }
                else if (currentLine.StartsWith("Dr"))
                {
                    List<string> strings = LibraryParser.SplitBySpaces(currentLine);
                    pad.DrillSize = Int32.Parse(strings[1]);
                    pad.DrillXOffset = Int32.Parse(strings[2]);
                    pad.DrillYOffset = Int32.Parse(strings[3]);
                }
                else if (currentLine.StartsWith("At"))
                {
                    List<string> strings = LibraryParser.SplitBySpaces(currentLine);
                    switch (strings[1])
                    {
                        case "MECA":
                            pad.Attr = PadAttr.MECA;
                            break;
                        case "SMD":
                            pad.Attr = PadAttr.SMD;
                            break;
                        case "CONN":
                            pad.Attr = PadAttr.CONN;
                            break;
                        case "HOLE":
                            pad.Attr = PadAttr.HOLE;
                            break;
                        default:
                            pad.Attr = PadAttr.STD;
                            break;
                    }

                }
                else if (currentLine.StartsWith("Ne"))
                {
                    List<string> strings = LibraryParser.SplitBySpaces(currentLine);
                    pad.NetNumber = Int32.Parse(strings[1]);
                    pad.NetName = strings[2];

                }
                else if (currentLine.StartsWith("Po"))
                {
                    List<string> strings = LibraryParser.SplitBySpaces(currentLine);
                    pad.X = Int32.Parse(strings[1]);
                    pad.Y = Int32.Parse(strings[2]);
                }
            }
        }


    }
}
