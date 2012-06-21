using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCB_Layout_GA
{

    public class NetList
    {
        public Dictionary<string, Component> mComponents = new Dictionary<string, Component>();
        public Dictionary<int, Net> mNets = new Dictionary<int, Net>();

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

        public class Component
        {
            public string ID { get; set; }

            public string Value { get; set; }

            public Module Mod { get; set; }

            //Format e.g. "1 N-000001"
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
                        int pinNumber = -1;
                        try
                        {
                            pinNumber = Int32.Parse(pins[1]);
                        }
                        catch (Exception)
                        {
                        }
                        net.Pins.Add(new Pin(pins[0], pinNumber));
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
            public int PinNumber { get; set; }
            public int ComponentID { get; set; }

            public Pin(string componentID, int pinNumber)
            {
                ComponentName = componentID;
                PinNumber = pinNumber;
            }
        }

    }
}
