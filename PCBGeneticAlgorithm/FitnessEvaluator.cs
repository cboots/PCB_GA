﻿using System;
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
                layout.RawAreaFitness = AssessRawAreaFitness(ga, layout.ModuleLocations);
                layout.RawNetFitness = AssessRawNetFitness(ga, layout.ModuleLocations);
                layout.RawConstraintViolations = AssessRawConstraintViolations(ga, layout.ModuleLocations);
            }

            CalculateNormalizedF1(generation);
            CalculateNormalizedF2(generation);
            CalculateF3(generation);

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

        private static void CalculateF3(GALayout[] generation)
        {
            foreach (GALayout layout in generation)
            {
                layout.F3 = layout.RawConstraintViolations;
            }
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

        public static double AssessRawNetFitness(GeneticAlgorithm ga, GAModuleLocation[] moduleLocations)
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
                        netLength = CalculateNetLength2(ga, net, moduleLocations);
                        break;
                    case 3:
                        //Special case, find shortest two nets
                        netLength = CalculateNetLength3(ga, net, moduleLocations);
                        break;
                    default:
                        //use euclidian min-spanning tree approach
                        netLength = CalculateNetLengthEuclidean(ga, net, moduleLocations);
                        break;

                }
                total += netLength;
            }

            return total;

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

        private static void CalculateNormalizedF1(GALayout[] layouts)
        {
            int n = 0;
            double sum1 = 0;
            double sum2 = 0;

            foreach (GALayout layout in layouts)
            {
                n++;
                sum1 += layout.RawAreaFitness;
            }

            double mean = sum1 / n;

            foreach (GALayout layout in layouts)
            {
                sum2 += (layout.RawAreaFitness - mean)*(layout.RawAreaFitness - mean);
            }

            double std = Math.Sqrt(sum2 / (n - 1));

            foreach (GALayout layout in layouts)
            {
                //TODO improve normalization equation based on statistical analysis of data set
                layout.F1 = (layout.RawAreaFitness - mean) / std;
            }
        }

        private static void CalculateNormalizedF2(GALayout[] layouts)
        {
            int n = 0;
            double sum1 = 0;
            double sum2 = 0;

            foreach (GALayout layout in layouts)
            {
                n++;
                sum1 += layout.RawNetFitness;
            }

            double mean = sum1 / n;

            foreach (GALayout layout in layouts)
            {
                sum2 += (layout.RawNetFitness - mean) * (layout.RawNetFitness - mean);
            }

            double std = Math.Sqrt(sum2 / (n - 1));

            foreach (GALayout layout in layouts)
            {
                //TODO improve normalization equation based on statistical analysis of data set
                layout.F2 = (layout.RawNetFitness - mean) / std;
            }
        }

        private static void CalculateFitness(GeneticAlgorithm ga, GALayout layout)
        {
            layout.Fitness = ga.Alpha * layout.F1 + ga.Beta * layout.F2 + ga.Gamma * layout.F3;
            if(ga.XStd > 0 || ga.YStd > 0)
            {
                if(sRandomNorm == null)
                {
                    sRandomNorm = new RandomNorm();
                }

                double X;
                double Y; 
                sRandomNorm.GetNextTwo(out X, out Y, 1.0, 1.0, ga.XStd, ga.YStd);
                layout.VariedFitness = ga.Alpha * X * layout.F1 + ga.Beta * Y * layout.F2 + ga.Gamma * layout.F3;

            }else{
                layout.VariedFitness = layout.Fitness;
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

        private static double CalculateNetLengthEuclidean(GeneticAlgorithm ga, GANet net, GAModuleLocation[] locations)
        {
            List<Vertex> points = GetPinLocations(ga, net, locations);

            //Compute Delaunay Triangulation
            Triangulator angulator = new Triangulator();
            List<Triad> triads = angulator.Triangulation(points, true);

            //Convert to Graph
            SortedSet<int> nodes = new SortedSet<int>();
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

            List<Edge> EMST = FindEMST(points.Count, nodes, adjMatrix);

            double total = 0.0;
            foreach (Edge edge in EMST)
            {
                total += edge.Length;
            }

            return total;
        }

        //TODO Optimize.  Currently a hot path.
        private static List<Edge> FindEMST(int points, SortedSet<int> nodes, double[,] adjMatrix)
        {
            SortedSet<Edge> openSet = new SortedSet<Edge>();
            List<Edge> EMST = new List<Edge>();

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

                //Remove edge from empty set, and add to EMST
                Edge nextEdge = openSet.Min;
                openSet.Remove(nextEdge);
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

        class Edge : IComparable
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
    }
}