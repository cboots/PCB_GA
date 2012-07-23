using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PCBGeneticAlgorithm
{

    public class NetList
    {
        public SortedDictionary<string, Component> mComponents = new SortedDictionary<string, Component>();
        public SortedDictionary<int, Net> mNets = new SortedDictionary<int, Net>();

        public static NetList parseNetlistFile(string netlistFile)
        {
            string[] lines = System.IO.File.ReadAllLines(netlistFile);
            int currentLine = 0;

            NetList netList = new NetList();
            //Parse components
            while (currentLine < lines.Length && !lines[currentLine].StartsWith("*"))
            {
                if (lines[currentLine].StartsWith(" ("))
                {
                    Component comp = Component.parse(lines, ref currentLine);
                    netList.mComponents.Add(comp.ID, comp);
                }
                else
                {
                    currentLine++;
                }
            }

            //Parse Nets
            while (currentLine < lines.Length)
            {
                if (lines[currentLine].StartsWith("Net"))
                {
                    Net net = Net.parse(lines, ref currentLine);
                    netList.mNets.Add(net.ID, net);
                }
                else
                {
                    currentLine++;
                }
            }
            return netList;
        }

        public static Dictionary<string, string> parseComponentsFile(string componentpath)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] lines = System.IO.File.ReadAllLines(componentpath);
            int currentLine = 0;

            while (currentLine < lines.Length)
            {
                if (lines[currentLine].StartsWith("BeginCmp"))
                {
                    string Reference = "";
                    string Module = "";
                    while (!lines[currentLine].StartsWith("EndCmp"))
                    {

                        if (lines[currentLine].StartsWith("Reference"))
                        {
                            Reference = lines[currentLine].Substring(12, lines[currentLine].Length - 13);//Trim Semicolon
                        }
                        else if (lines[currentLine].StartsWith("IdModule"))
                        {
                            Module = lines[currentLine].Substring(12, lines[currentLine].Length - 13);//Trim Semicolon
                        }
                        currentLine++;
                    }
                    dict.Add(Reference, Module);
                }
                else
                {
                    currentLine++;
                }
            }

            return dict;

        }

        internal int calculateModuleSideGCD()
        {
            int[] sides = new int[mComponents.Values.Count * 2];

            int i = 0;
            foreach (Component comp in mComponents.Values)
            {
                Module mod = comp.Mod;
                if (mod == null)
                    return -1;//crash thread
                Rectangle rect = mod.getBoundingRectangle();

                sides[i] = rect.Height;
                sides[i + 1] = rect.Width;

                i += 2;
            }

            return GCD(sides);
        }

        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        static int GCD(int[] integerSet)
        {
            return integerSet.Aggregate(GCD);
        }    

        

        public class Component
        {
            public string ID { get; set; }

            public string Value { get; set; }

            public Module Mod { get; set; }

            //Format e.g. "1 N-000001"
            //            "D N-000002"
            public List<string> PinNets = new List<string>();


            public static Component parse(string[] lines, ref int currentLine)
            {
                if (lines[currentLine].StartsWith(" ("))
                {
                    Component comp = new Component();
                    List<string> strings = LibraryParser.SplitBySpaces(lines[currentLine]);
                    comp.ID = strings[3];
                    comp.Value = strings[4];

                    currentLine++;
                    //Parse pin nets
                    while (lines[currentLine].StartsWith("  ("))
                    {
                        strings = LibraryParser.SplitBySpaces(lines[currentLine]);
                        comp.PinNets.Add(strings[1] + " " + strings[2]);
                        currentLine++;
                    }

                    return comp;
                }
                return null;
            }
        }

        public class Net
        {
            public int ID { get; set; }

            public string FullName { get; set; }

            public string ShortName { get; set; }

            public List<Pin> Pins = new List<Pin>();

            public static Net parse(string[] lines, ref int currentLine)
            {

                if (lines[currentLine].StartsWith("Net"))
                {
                    Net net = new Net();
                    List<string> strings = LibraryParser.SplitBySpaces(lines[currentLine]);
                    net.ID = Int32.Parse(strings[1]);
                    net.FullName = strings[2];
                    net.ShortName = strings[3];

                    currentLine++;
                    while (lines[currentLine].StartsWith(" "))
                    {
                        string[] pins = lines[currentLine].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        Pin pin = new Pin(pins[0], pins[1]);
                        net.Pins.Add(pin);
                        currentLine++;
                    }

                    return net;
                }
                return null;
            }

        }

        public class Pin
        {
            public string ComponentName { get; set; }
            public string PinName { get; set; }

            public Pin(string componentID, string pinName)
            {
                ComponentName = componentID;
                PinName = pinName;
            }
        }


    }
}
