using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DelaunayTriangulator;

namespace PCBGeneticAlgorithm
{
    public class FitnessEvaluator
    {

        public static void LocateModules(GeneticAlgorithm ga, GALayout layout)
        {
            if (layout.ModuleLocations == null || layout.ModuleLocations.Length != ga.Modules.Length)
            {
                layout.ModuleLocations = new GAModuleLocation[ga.Modules.Length];//Initialize module lengths
            }

            for (int i = 0; i < layout.ModuleLocations.Length; i++)
            {
                //Clear array
                layout.ModuleLocations[i] = null;
            }

            int width = layout.Width;
            int height = layout.Height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (layout[x, y] != 0)
                    {
                        int index = (layout[x, y] >> 2) - 1;//Zero based index
                        if (layout.ModuleLocations[index] == null)
                        {
                            layout.ModuleLocations[index] = new GAModuleLocation(index + 1);
                            layout.ModuleLocations[index].X = x;
                            layout.ModuleLocations[index].Y = y;
                            layout.ModuleLocations[index].Rotation = layout[x, y] & 0x03;

                        }
                        //Skip rotated module width
                        int mWidth = ga.Modules[index].getRotatedWidth(layout.ModuleLocations[index].Rotation);
                        x = x + mWidth - 1;
                            
                    }
                }
            }
        }

        public static void AssessRawAreaFitness(GeneticAlgorithm ga, GALayout layout)
        {
            int minX = Int32.MaxValue;
            int minY = Int32.MaxValue;
            int maxX = Int32.MinValue;
            int maxY = Int32.MinValue;

            for (int i = 0; i < layout.ModuleLocations.Length; i++)
            {
                int left = layout.ModuleLocations[i].X;
                int top = layout.ModuleLocations[i].Y;
                int right = left + ga.Modules[i].getRotatedWidth(layout.ModuleLocations[i].Rotation);
                int bottom = top + ga.Modules[i].getRotatedHeight(layout.ModuleLocations[i].Rotation);

                if (left < minX)
                    minX = left;
                if (right > maxX)
                    maxX = right;
                if (top < minY)
                    minY = top;
                if (bottom > maxY)
                    maxY = bottom;
            }

            layout.RawAreaFitness = (maxX - minX) * (maxY - minY);
        }

        public static void AssessRawNetFitness(GeneticAlgorithm ga, GALayout layout)
        {
            double total = 0.0;
            for (int i = 0; i < ga.Nets.Length; i++)
            {
                double netLength = 0.0;
                GANet net = ga.Nets[i];
                switch (net.Connections.Length)
                {
                    case 1://Error
                        break;
                    case 2:
                        //Special case, only one edge to assess
                        netLength = CalculateNetLength2(ga, net, layout.ModuleLocations);
                        break;
                    case 3:
                        //Special case, find shortest two nets
                        netLength = CalculateNetLength3(ga, net, layout.ModuleLocations);
                        break;
                    default:
                        //use euclidian min-spanning tree approach
                        netLength = CalculateNetLengthEuclidean(ga, net, layout.ModuleLocations);
                        break;

                }
                total += netLength;
            }

            layout.RawNetFitness = total;

        }

        private static List<Vertex> GetPinLocations(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations)
        {
            List<Vertex> points = new List<Vertex>(net.Connections.Length / 2);
            for (int i = 0; i < net.Connections.Length/2; i++)
            {
                int modId = net.Connections[i, 0];
                int pinId = net.Connections[i, 1];
                GAModule.GAModulePin pin = ga.Modules[modId].Pins[pinId];
                int x = pin.getX(locations[modId].Rotation);
                int y = pin.getY(locations[modId].Rotation);

                points.Add(new Vertex(x, y));
            }
            return points;
        }

        private static double CalculateNetLengthEuclidean(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations)
        {
            List<Vertex> points = GetPinLocations(ga, net, locations);

            Triangulator angulator = new Triangulator();
            List<Triad> triangles = angulator.Triangulation(points, true);
        }

        private static double CalculateNetLength2(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations)
        {

            List<Vertex> points = GetPinLocations(ga, net, locations);

            return points[0].distanceTo(points[1]);
        }

        private static double CalculateNetLength3(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations)
        {
            
            List<Vertex> points = GetPinLocations(ga, net, locations);


            double len0 = points[0].distanceTo(points[1]);
            double len1 = points[1].distanceTo(points[2]);
            double len2 = points[2].distanceTo(points[0]);


            //Return sum of lengths minus longest connection
            return len0 + len1 + len2 - Math.Max(Math.Max(len0, len1), len2);
        }


    }
}
