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
                switch (net.Connections.Length/2)
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
                int x = pin.getX(locations[modId].Rotation) + locations[modId].X;
                int y = pin.getY(locations[modId].Rotation) + locations[modId].Y;
                
                points.Add(new Vertex(x, y));
            }
            return points;
        }

        private static double CalculateNetLengthEuclidean(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations)
        {
            List<Vertex> points = GetPinLocations(ga, net, locations);

            //Compute Delaunay Triangulation
            Triangulator angulator = new Triangulator();
            List<Triad> triads = angulator.Triangulation(points, true);

            //Convert to Graph
            List<int> nodes = new List<int>(net.Connections.Length / 2);
            for (int i = 0; i < net.Connections.Length / 2; i++)
            {
                nodes.Add(i);//corresponds to points[index].
            }
            
            //Create symetric adjacency matrix
            double[,] adjMatrix = new double[nodes.Count,nodes.Count];
            foreach (Triad tri in triads)
            {
                adjMatrix[tri.a, tri.b] = points[tri.a].distanceTo(points[tri.b]);
                adjMatrix[tri.b, tri.a] = adjMatrix[tri.a, tri.b];
                adjMatrix[tri.a, tri.c] = points[tri.a].distanceTo(points[tri.c]);
                adjMatrix[tri.c, tri.a] = adjMatrix[tri.a, tri.c];
                adjMatrix[tri.b, tri.c] = points[tri.b].distanceTo(points[tri.c]);
                adjMatrix[tri.c, tri.b] = adjMatrix[tri.b, tri.c];
            }

            List<Edge> EMST = FindEMST(points, nodes, adjMatrix);

            double total = 0.0;
            foreach (Edge edge in EMST)
            {
                total += edge.Length;
            }

            return total;
        }

        private static List<Edge> FindEMST(List<Vertex> points, List<int> nodes, double[,] adjMatrix)
        {
            List<Edge> openSet = new List<Edge>();
            List<Edge> EMST = new List<Edge>();

            int currentVertex = 0;
            while (nodes.Count > 0)
            {
                nodes.Remove(currentVertex);

                //Add any edges to open set
                for (int i = 0; i < points.Count; i++)
                {
                        if (adjMatrix[currentVertex, i] > 0)
                        {

                            Edge edge = new Edge(currentVertex, i, adjMatrix[currentVertex, i]);
                            //TODO Optimize
                            if(!openSet.Contains(edge) && !EMST.Contains(edge))
                                openSet.Add(edge);
                        }
                    
                }

                openSet.Sort();
                //Remove edge from empty set, and add to EMST
                Edge nextEdge = openSet[0];
                openSet.RemoveAt(0);
                EMST.Add(nextEdge);

                if (nodes.Contains(nextEdge.U))
                {
                    //V is in EMST already, add U next loop
                    currentVertex = nextEdge.U;}
                else{
                    currentVertex = nextEdge.V;
                }
            }
            return EMST;
        }

        class Edge:IComparable
        {
            int mU;
            public int U { get { return mU; } }

            int mV;
            public int V { get { return mV; } }

            double mLength;
            public double Length { get { return mLength; } }

            public Edge(int u, int v, double length){
                mU = u;
                mV = v;
                mLength = length;
            }

            int IComparable.CompareTo(object obj)
            {
                if (obj == null) return 1;

                Edge other = obj as Edge;
                if (other != null)
                    return this.Length.CompareTo(other.Length);
                else
                    throw new ArgumentException("Object is not an Edge");
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                Edge other = obj as Edge;
                if (other != null)
                    return (((other.U == this.U && other.V == this.V)
                        || (other.V == this.U && other.U == this.V))//allow equivalent reverse order
                        && other.Length == this.Length);
                else
                    return false;
            }

            public override int GetHashCode()
            {
                return U ^ V ^ (int)Length;
            }
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
