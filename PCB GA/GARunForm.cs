using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PCBGeneticAlgorithm;
using System.IO;
using DelaunayTriangulator;

namespace PCB_Layout_GA
{
    public partial class GARunForm : Form
    {
        public GeneticAlgorithm GA {get; set;}


        public GARunForm()
        {
            InitializeComponent();
        }

        private void GARunForm_Load(object sender, EventArgs e)
        {
            gaBackgroundWorker.RunWorkerAsync();
        }

        private void gaBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RunNormalGA();
        }

        public void RunNormalGA()
        {
            //Setup first generation
            GALayout[] generation = new GALayout[GA.GenerationSize];

            logProgress("Creating Initial Population");
            for (int i = 0; i < generation.Length; i++)
            {
                generation[i] = GALayout.GenerateRandomLayout(GA);
            }

            using (System.IO.StreamWriter fitnessWriter = new System.IO.StreamWriter(@"C:\Users\Collin\Dropbox\Current Classes\Independent Study\RunFitness.text"))
            {
                using (System.IO.StreamWriter f1Writer = new System.IO.StreamWriter(@"C:\Users\Collin\Dropbox\Current Classes\Independent Study\RunF1.text"))
                {
                    using (System.IO.StreamWriter f2Writer = new System.IO.StreamWriter(@"C:\Users\Collin\Dropbox\Current Classes\Independent Study\RunF2.text"))
                    {
                        using (System.IO.StreamWriter rawAreaWriter = new System.IO.StreamWriter(@"C:\Users\Collin\Dropbox\Current Classes\Independent Study\RunRawArea.text"))
                        {
                            using (System.IO.StreamWriter rawNetLengthWriter = new System.IO.StreamWriter(@"C:\Users\Collin\Dropbox\Current Classes\Independent Study\RunRawNetLength.text"))
                            {
                                for (int gen = 1; gen <= GA.MaxGeneration; gen++)
                                {
                                    logProgress((100 * gen) / GA.MaxGeneration, "Evaluating Generation: " + gen + "\n");
                                    FitnessEvaluator.EvaluateGenerationFitness(GA, generation);

                                    foreach (GALayout layout in generation)
                                    {
                                        fitnessWriter.Write(layout.Fitness);
                                        fitnessWriter.Write(' ');
                                        f1Writer.Write(layout.F1);
                                        f1Writer.Write(' ');
                                        f2Writer.Write(layout.F2);
                                        f2Writer.Write(' ');
                                        rawAreaWriter.Write(layout.RawAreaFitness);
                                        rawAreaWriter.Write(' ');
                                        rawNetLengthWriter.Write(layout.RawNetFitness);
                                        rawNetLengthWriter.Write(' ');
                                    }
                                    fitnessWriter.WriteLine();
                                    f1Writer.WriteLine();
                                    f2Writer.WriteLine();
                                    rawAreaWriter.WriteLine();
                                    rawNetLengthWriter.WriteLine();


                                    logProgress("Creating next generation\n");
                                    generation = Selection.GenerateNextGeneration(GA, generation);
                                }
                            }
                        }
                    }
                }
            }
            FitnessEvaluator.EvaluateGenerationFitness(GA, generation);

            using (System.IO.StreamWriter solutionWriter = new System.IO.StreamWriter(@"C:\Users\Collin\Dropbox\Current Classes\Independent Study\Solution.text"))
            {
                using (System.IO.StreamWriter connectionswriter = new System.IO.StreamWriter(@"C:\Users\Collin\Dropbox\Current Classes\Independent Study\SolutionNets.text"))
                {
                    double minFitness = Double.MaxValue;
                    GALayout best = null;
                    for (int i = 0; i < generation.Length; i++)
                    {
                        if (generation[i].Fitness < minFitness)
                        {
                            best = generation[i];
                        }
                    }
                    //Compact solution
                    best.ModuleLocations = Compaction.Compact(best.ModuleLocations);


                    print2DArray(best.GenerateArray(), solutionWriter);

                    //print Connections
                    printConnections(best, connectionswriter);
                }
            }
            logProgress("Run finished");
        }

        public void printConnections(GALayout best, StreamWriter connectionsWriter)
        {
            List<Connection> connections = new List<Connection>();

            foreach (GANet net in GA.Nets)
            {
                List<Vertex> points = FitnessEvaluator.GetPinLocations(GA, net, best.ModuleLocations);
                switch (points.Count)
                {
                    case 1://Error
                        break;
                    case 2:
                        connections.Add(new Connection(points[0], points[1]));
                        break;
                    case 3:
                        //Special case, find shortest two nets
                        AddShortestConnections3(connections, points);

                        break;
                    default:
                        //use euclidian min-spanning tree approach
                        GenerateEMSTEdges(connections, net, points);
                        break;
                }
            }

            foreach (Connection con in connections)
            {
                connectionsWriter.Write(con.P1.x);
                connectionsWriter.Write(' ');
                connectionsWriter.Write(con.P1.y);
                connectionsWriter.Write(' ');
                connectionsWriter.Write(con.P2.x);
                connectionsWriter.Write(' ');
                connectionsWriter.Write(con.P2.y);
                connectionsWriter.Write(' ');
                connectionsWriter.WriteLine();
            }
        }

        private static void GenerateEMSTEdges(List<Connection> connections, GANet net, List<Vertex> points)
        {
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

                List<FitnessEvaluator.Edge> EMST = FitnessEvaluator.FindEMST(points.Count, nodes, adjMatrix);

                foreach (FitnessEvaluator.Edge edge in EMST)
                {
                    connections.Add(new Connection(points[edge.U], points[edge.V]));
                }

            }
            catch (Exception e)
            {
            }
        }

        private static void AddShortestConnections3(List<Connection> connections, List<Vertex> points)
        {
            double len0 = points[0].distanceTo(points[1]);
            double len1 = points[1].distanceTo(points[2]);
            double len2 = points[2].distanceTo(points[0]);

            if (len0 > len1)
            {
                if (len2 > len0)
                {
                    //Len2 max
                    connections.Add(new Connection(points[0], points[1]));
                    connections.Add(new Connection(points[1], points[2]));
                }
                else
                {
                    //Len0 max
                    connections.Add(new Connection(points[2], points[0]));
                    connections.Add(new Connection(points[1], points[2]));
                }
            }
            else
            {
                if (len2 > len1)
                {
                    //Len2 max
                    connections.Add(new Connection(points[0], points[1]));
                    connections.Add(new Connection(points[1], points[2]));
                }
                else
                {
                    //Len1 max
                    connections.Add(new Connection(points[0], points[1]));
                    connections.Add(new Connection(points[2], points[0]));
                }
            }
        }

        private class Connection
        {
            public Vertex P1 {get;set;}
            public Vertex P2 {get;set;}

            public Connection(Vertex p1, Vertex p2)
            {
                P1 = p1;
                P2 = p2;
            }

        }


        public static void print2DArray(ushort[,] array, StreamWriter writer)
        {
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    writer.Write(array[j, i] + " ");
                }
                writer.WriteLine();
            }
        }

        int mProgress = 0;
        public void logProgress(string log)
        {
            logProgress(mProgress, log);
        }

        public void logProgress(int percent, string log)
        {
            mProgress = percent;
            gaBackgroundWorker.ReportProgress(percent, log);
        }

        
        private void gaBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            if (e.UserState is string)
            {
                logBox.AppendText((string)e.UserState);
            }
        }

        private void gaBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

    }
}
