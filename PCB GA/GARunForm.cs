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
            using (System.IO.StreamWriter solutionWriter = new System.IO.StreamWriter(@"C:\Users\Collin\Dropbox\Current Classes\Independent Study\Solution.text"))
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
                print2DArray(best.GenerateArray(), solutionWriter);

            }
            logProgress("Run finished");
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
