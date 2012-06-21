using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCB_Layout_GA
{
    class NetList
    {
        public Dictionary<string, Component> mComponents = new Dictionary<string, Component>();
        public Dictionary<int, Net> mNets = new Dictionary<int, Net>();

        public static NetList parseNetlistFile(string netlistFile)
        {
            string[] lines = System.IO.File.ReadAllLines(netlistFile);
            int currentLine = 0;

            NetList netList = new NetList();
            //Parse components


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
        
        public class Component
        {
            public string ID { get; set; }

            public string Value { get; set; }

            public List<string> PinNets = new List<string>();

        }

        public class Net
        {
            public int ID { get; set; }

            public string FullName { get; set; }

            public string ShortName { get; set; }

            //Format "U1 1"
            public List<string> Pins = new List<string>();

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
                        net.Pins.Add(lines[currentLine].Trim());
                        currentLine++;
                    }

                    return net;
                }
                return null;
            }

        }
    }
}
