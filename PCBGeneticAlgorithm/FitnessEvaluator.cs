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
        private static RandomNorm sRandomNorm = null;
        public static void EvaluateGenerationFitness(GeneticAlgorithm ga, GALayout[] generation)
        {
            foreach (GALayout layout in generation)
            {
                GAModuleLocation[] compacted = Compaction.Compact(layout.ModuleLocations);
               if (ga.Alpha > 0.0)
               {
                   layout.RawAreaFitness = AssessRawAreaFitness(ga, compacted);
                }
                else
                {
                    layout.RawAreaFitness = 0.0;
                }

                layout.Connections = new List<Connection>();
                if (ga.Beta > 0.0 || ga.Gamma > 0.0)
                {
                    layout.RawNetFitness = AssessRawNetFitness(ga, compacted, layout.Connections);
                }
                else
                {
                    layout.RawNetFitness = 0.0;
                }

                if (ga.Gamma > 0.0)
                {
                    layout.RawCrossoverCount = AssessRawCrossoverCount(layout.Connections);
                }else
                {
                    layout.RawCrossoverCount = 0;
                }

                layout.RawConstraintViolations = AssessRawConstraintViolations(ga, compacted);
            }

            CalculateNormalizedF1(generation);
            CalculateNormalizedF2(generation);
            CalculateNormalizedF3(generation);
            CalculateF4(generation);

            foreach (GALayout layout in generation)
            {
                CalculateFitness(ga, layout);
            }
        }

        private static int AssessRawConstraintViolations(GeneticAlgorithm ga, GAModuleLocation[] moduleLocations)
        {
            //TODO Implement
            return 0;
        }

        private static void CalculateF4(GALayout[] generation)
        {
            foreach (GALayout layout in generation)
            {
                layout.F4 = layout.RawConstraintViolations;
            }
        }

        private static int AssessRawCrossoverCount(List<Connection> list)
        {
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i; j < list.Count; j++)
                {
                    if (list[i].Intersects(list[j]))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public static double AssessRawAreaFitness(GeneticAlgorithm ga, GAModuleLocation[] moduleLocations)
        {
            int minX = Int32.MaxValue;
            int minY = Int32.MaxValue;
            int maxX = Int32.MinValue;
            int maxY = Int32.MinValue;

            for (int i = 0; i < moduleLocations.Length; i++)
            {
                int left = moduleLocations[i].X;
                int top = moduleLocations[i].Y;
                int right = left + ga.Modules[i].getRotatedWidth(moduleLocations[i].Rotation);
                int bottom = top + ga.Modules[i].getRotatedHeight(moduleLocations[i].Rotation);

                if (left < minX)
                    minX = left;
                if (right > maxX)
                    maxX = right;
                if (top < minY)
                    minY = top;
                if (bottom > maxY)
                    maxY = bottom;
            }

            return (maxX - minX) * (maxY - minY);
        }

        public static double AssessRawNetFitness(GeneticAlgorithm ga, GAModuleLocation[] moduleLocations, List<Connection> connections)
        {
            double total = 0.0;
            for (int i = 0; i < ga.Nets.Length; i++)
            {
                double netLength = 0.0;
                GANet net = ga.Nets[i];
                if (net.Weight <= 0.0)
                {
                    continue;
                }
                switch (net.Connections.Length/2)
                {
                    case 1://Error
                        break;
                    case 2:
                        //Special case, only one edge to assess
                        netLength = CalculateNetLength2(ga, net, moduleLocations, connections);
                        break;
                    case 3:
                        //Special case, find shortest two nets
                        netLength = CalculateNetLength3(ga, net, moduleLocations, connections);
                        break;
                    default:
                        //use euclidian min-spanning tree approach
                        netLength = CalculateNetLengthEuclidean(ga, net, moduleLocations, connections);
                        break;

                }
                total += netLength*net.Weight;
            }

            return total;

        }

        public static List<Vertex> GetPinLocations(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations)
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

        private static void CalculateNormalizedF1(GALayout[] layouts)
        {
            double sum = 0;

            foreach (GALayout layout in layouts)
            {
                sum += layout.RawAreaFitness;
            }

            

            foreach (GALayout layout in layouts)
            {
                if (sum == 0)
                {
                    layout.F1 = 1;
                }
                else
                {
                    layout.F1 = (layout.RawAreaFitness / sum);
                }
            }
        }

        private static void CalculateNormalizedF2(GALayout[] layouts)
        {
            double sum = 0;

            foreach (GALayout layout in layouts)
            {
                sum += layout.RawNetFitness;
            }

            foreach (GALayout layout in layouts)
            {
                if (sum == 0)
                {
                    layout.F2 = 1;
                }
                else
                {
                    layout.F2 = (layout.RawNetFitness / sum);
                }
            }
        }

        private static void CalculateNormalizedF3(GALayout[] layouts)
        {
            double sum = 0;

            foreach (GALayout layout in layouts)
            {
                sum += layout.RawCrossoverCount;
            }

            foreach (GALayout layout in layouts)
            {
                if (sum == 0)
                {
                    layout.F3 = 1;
                }
                else
                {
                    layout.F3 = (layout.RawCrossoverCount / sum);
                }
            }
        }

        private static void CalculateFitness(GeneticAlgorithm ga, GALayout layout)
        {
            layout.Fitness = ga.Alpha * layout.F1 + ga.Beta * layout.F2 + ga.Gamma * layout.F3 + 100 * layout.F4;
            if(ga.XStd > 0 || ga.YStd > 0)
            {
                if(sRandomNorm == null)
                {
                    sRandomNorm = new RandomNorm();
                }

                double X;
                double Y; 
                sRandomNorm.GetNextTwo(out X, out Y, 1.0, 1.0, ga.XStd, ga.YStd);
                layout.VariedFitness = ga.Alpha * X * layout.F1 + ga.Beta * Y * layout.F2 + ga.Gamma * layout.F3 + 100 * layout.F4;

            }else{
                layout.VariedFitness = layout.Fitness;
            }
        }

        private static double CalculateNetLength2(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations, List<Connection> connections)
        {

            List<Vertex> points = GetPinLocations(ga, net, locations);
            connections.Add(new Connection(points[0], points[1]));
            return points[0].distanceTo(points[1]);
        }

        private static double CalculateNetLength3(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations, List<Connection> connections)
        {
            List<Vertex> points = GetPinLocations(ga, net, locations);

            double len0 = points[0].distanceTo(points[1]);
            double len1 = points[1].distanceTo(points[2]);
            double len2 = points[2].distanceTo(points[0]);

            double max = Math.Max(Math.Max(len0, len1), len2);
            if (max == len0)
            {
                connections.Add(new Connection(points[1], points[2]));
                return len1 + len2;
            }
            else if (max == len1)
            {
                connections.Add(new Connection(points[0], points[2]));
                return len0 + len2;
            }
            else //max == len2
            {
                connections.Add(new Connection(points[0], points[1]));
                return len0 + len1;
            }

        }

        private static double CalculateNetLengthEuclidean(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations, List<Connection> connections)
        {
            List<Vertex> points = GetPinLocations(ga, net, locations);

            //Perform edge case check
            if (Colinear(points))
            {
                return CalculateNetLengthLinear(points);
            }

            //Compute Delaunay Triangulation
            Triangulator angulator = new Triangulator();
            try
            {
                List<Triad> triads = angulator.Triangulation(points, true);

                //Convert to Graph
                SortedSet<int> nodes = new SortedSet<int>();
                for (int i = 0; i < net.Connections.Length / 2; i++)
                {
                    nodes.Add(i);//corresponds to points[index].
                }

                //Create symetric adjacency matrix
                double[,] adjMatrix = new double[nodes.Count, nodes.Count];
                foreach (Triad tri in triads)
                {
                    adjMatrix[tri.a, tri.b] = points[tri.a].distanceTo(points[tri.b]);
                    adjMatrix[tri.b, tri.a] = adjMatrix[tri.a, tri.b];
                    adjMatrix[tri.a, tri.c] = points[tri.a].distanceTo(points[tri.c]);
                    adjMatrix[tri.c, tri.a] = adjMatrix[tri.a, tri.c];
                    adjMatrix[tri.b, tri.c] = points[tri.b].distanceTo(points[tri.c]);
                    adjMatrix[tri.c, tri.b] = adjMatrix[tri.b, tri.c];
                }

                List<Edge> EMST = FindEMST(points.Count, nodes, adjMatrix);
                double total = 0.0;
                foreach (Edge edge in EMST)
                {
                    connections.Add(new Connection(points[edge.U], points[edge.V]));
                    total += edge.Length;
                }

                return total;
            }
            catch (Exception)
            {
                //Return some basic approximation
                return CalculateNetLengthLinear(points);
            }
        }

        private static double CalculateNetLengthLinear(List<Vertex> points)
        {
            Vertex min = points[0];
            Vertex max = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].x < min.x || points[i].y < min.y)
                    min = points[i];
                if (points[i].x > max.x || points[i].y > max.y)
                    max = points[i];
            }

            return Math.Max(max.x - min.x, max.y - min.y);
        }

        private static bool Colinear(List<Vertex> points)
        {
            float x = points[0].x;
            float y = points[0].y;

            bool xDiff = false;
            bool yDiff = false;
            for (int i = 1; i < points.Count; i++)
            {
                if (x != points[i].x)
                    xDiff = true;
                if (y != points[i].y)
                    yDiff = true;

                if (xDiff && yDiff)
                    return false;
            }
            return true;
        }

        public static List<Edge> FindEMST(int points, SortedSet<int> nodes, double[,] adjMatrix)
        {
            //For debugging crashes
            //SortedSet<int> nodesCopy = new SortedSet<int>(nodes);
            SortedSet<Edge> openSet = new SortedSet<Edge>();
            List<Edge> EMST = new List<Edge>();
            //nodes = new SortedSet<int>(nodesCopy);
            int currentVertex = 0;
            while (nodes.Count > 0)
            {
                nodes.Remove(currentVertex);

                //Add any edges to open set
                for (int i = 0; i < points; i++)
                {
                    if (adjMatrix[currentVertex, i] > 0)
                    {
                        Edge edge = new Edge(currentVertex, i, adjMatrix[currentVertex, i]);
                        //TODO Optimize
                        if(nodes.Contains(i))
                            openSet.Add(edge);//Other end of node is not explored already
                    }
                }

                //If more nodes to go, get next edge.
                if (nodes.Count > 0)
                {
                    //Remove edge from empty set, and add to EMST
                    Edge nextEdge = openSet.Min;
                    openSet.Remove(nextEdge);
                    if (!nodes.Contains(nextEdge.V))
                    {
                        foreach (Edge edge in openSet)
                        {
                            if (nodes.Contains(edge.V))
                            {
                                nextEdge = edge;
                                break;
                            }
                        }
                    }
                    EMST.Add(nextEdge);

                    if (nodes.Contains(nextEdge.U))
                    {
                        //V is in EMST already, add U next loop
                        currentVertex = nextEdge.U;
                    }
                    else
                    {
                        currentVertex = nextEdge.V;
                    }
                }
            }
            return EMST;
        }

        public class Edge : IComparable
        {
            int mU;
            public int U { get { return mU; } }

            int mV;
            public int V { get { return mV; } }

            double mLength;
            public double Length { get { return mLength; } }

            public Edge(int u, int v, double length)
            {
                mU = u;
                mV = v;
                mLength = length;
            }

            int IComparable.CompareTo(object obj)
            {
                if (obj == null) return 1;

                Edge other = obj as Edge;
                if (other != null)
                {
                    if (other.Length == mLength)
                    {
                        //Cannot say they are equal
                        return 1;
                    }
                    return this.mLength.CompareTo(other.Length);
                }
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

            public override string ToString()
            {
                return "Edge {" + mU + "," + mV + "," + mLength + "}";
            }
        }
    }
}
