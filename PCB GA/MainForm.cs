using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PCBGeneticAlgorithm;
using System.Diagnostics;

namespace PCBGeneticAlgorithm
{
    public partial class MainForm : Form
    {

        internal NetList ImportedNetList { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(netlistTextBox.Text))
            {
                MessageBox.Show("Netlist file not found");
                return;
            }

            ImportNetlistForm importForm = new ImportNetlistForm();
            importForm.NetlistPath = netlistTextBox.Text;
            outputPathTextBox.Text = new FileInfo(netlistTextBox.Text).Directory.FullName + "\\GAOutput";
            DialogResult result = importForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                ImportedNetList = importForm.NetListResult;
                adjustConstraintsButton.Enabled = true;
                numComponentsLabel.Text = "Number of Components: " + ImportedNetList.mComponents.Keys.Count;
                numNetsLabel.Text = "Number of Nets: " + ImportedNetList.mNets.Keys.Count;

                int areaTotal = 0;
                int minDimension = Int32.MaxValue;
                foreach (NetList.Component comp in ImportedNetList.mComponents.Values)
                {
                    Module mod = comp.Mod;
                    if (mod == null)
                    {
                        continue;
                    }
                    Rectangle rect = mod.getBoundingRectangle();
                    areaTotal += rect.Width * rect.Height;

                    int min = Math.Min(rect.Width, rect.Height);
                    if (min < minDimension)
                        minDimension = min;

                }

                totalModAreaLabel.Text = "Total Module Area: " + areaTotal;
                minModuleDimLabel.Text = "Min Module Dim: " + minDimension;

                //int gcd = ImportedNetList.calculateModuleSideGCD();
                //gridSizeTextBox.Text = gcd.ToString();
                gridSizeTextBox.Text = (minDimension / 2).ToString();
            }
            else
            {
                MessageBox.Show("Netlist Import Failed");
            }
            
        }

        private void editModulePaths_Click(object sender, EventArgs e)
        {
            ModulePathEditor editorForm = new ModulePathEditor();
            editorForm.ShowDialog();
        }

        private void netlistBrowseButton_Click(object sender, EventArgs e)
        {
            
            DialogResult result = this.netlistOpenFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                //Add to list
                netlistTextBox.Text = this.netlistOpenFileDialog.FileName;
            }
        }

        private void adjustConstraintsButton_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (useRandomSelectionCheckbox.Checked)
            {
                xstdTextBox.Enabled = true;
                ystdTextBox.Enabled = true;
            }
            else
            {
                xstdTextBox.Enabled = false;
                ystdTextBox.Enabled = false;
            }
        }

        private void runGaButton_Click(object sender, EventArgs e)
        {
            GeneticAlgorithm ga = new GeneticAlgorithm();
            
            //Initialize parameters
            SetGAParameters(ga);

            //Convert modules
            GAModule[]  gaModules = new GAModule[ImportedNetList.mComponents.Values.Count];
            GANet[]  gaNets = new GANet[ImportedNetList.mNets.Values.Count];

            int i = 0;
            foreach (KeyValuePair<string, NetList.Component> pair in ImportedNetList.mComponents)
            {
                gaModules[i] = ConvertComponent(ga, pair.Value);
                i++;
            }


            int n = 0;
            //convert nets
            foreach (KeyValuePair<int, NetList.Net> pair in ImportedNetList.mNets)
            {
                gaNets[n] = ConvertNet(gaModules, pair.Value);
                n++;
            }

            //Store to GA
            ga.Nets = gaNets;
            ga.Modules = gaModules;


            //Finished setting up GA algorithm parameters
            //Launch ga running dialog
            GARunForm runForm = new GARunForm();
            runForm.GA = ga;
            runForm.OutputPath = outputPathTextBox.Text;
            DialogResult result = runForm.ShowDialog();

        }

        private static void printFitness(StreamWriter writer, GALayout l)
        {
            writer.Write(l.RawAreaFitness);
            writer.Write(' ');
            writer.Write(l.RawNetFitness);
            writer.Write(' ');
            writer.Write(l.RawConstraintViolations);
            writer.Write(' ');
            writer.Write(l.F1);
            writer.Write(' ');
            writer.Write(l.F2);
            writer.Write(' ');
            writer.Write(l.F3);
            writer.Write(' ');
            writer.Write(l.Fitness);
            writer.Write(' ');
            writer.Write(l.VariedFitness);
            writer.WriteLine();
        }

        private GANet ConvertNet(GAModule[] gaModules, NetList.Net net)
        {
            int[,] connections = new int[net.Pins.Count, 2];
            for (int p = 0; p < net.Pins.Count; p++)
            {
                NetList.Pin pin = net.Pins[p];
                int idx = findGAModuleIndex(pin.ComponentName, gaModules);
                if (idx == -1)
                {
                    throw new KeyNotFoundException("Could not find module: " + pin.ComponentName);
                }
                connections[p, 0] = idx;//Module index

                int pinIdx = findPinIndex(pin.PinName, gaModules[idx]);
                if (pinIdx == -1)
                {
                    throw new KeyNotFoundException("Could not find pin: " + pin.PinName + " in module " + gaModules[idx].ComponentReference);
                }
                connections[p, 1] = pinIdx;
            }
            GANet gaNet = new GANet(net.FullName, connections);
            return gaNet;
        }

        private int findGAModuleIndex(string componentName, GAModule[] gaModules)
        {
            for (int i = 0; i < gaModules.Length; i++)
            {
                if (gaModules[i].ComponentReference.Equals(componentName))
                    return i;
            }
            return -1;
        }

        private int findPinIndex(string pinName, GAModule module)
        {
            for (int i = 0; i < module.Pins.Length; i++)
            {
                if (module.Pins[i].PinName.Equals(pinName))
                    return i;
            }
            return -1;
        }

        private static GAModule ConvertComponent(GeneticAlgorithm ga, NetList.Component comp)
        {
            Module mod = comp.Mod;
            Rectangle rect = mod.getBoundingRectangle();

            int convertedWidth = rect.Width / ga.GridSize;
            if (rect.Width % ga.GridSize != 0)
            {
                //Round up
                convertedWidth++;
            }

            int convertedHeight = rect.Height / ga.GridSize;
            if (rect.Width % ga.GridSize != 0)
            {
                //Round up
                convertedHeight++;
            }

            PCBGeneticAlgorithm.GAModule.GAModulePin[] pins = new PCBGeneticAlgorithm.GAModule.GAModulePin[mod.mPads.Count];

            int j = 0;
            foreach (Module.Pad pad in mod.mPads)
            {
                //0 based position estimate
                int padX = (pad.X - rect.X) / ga.GridSize;
                int padY = (pad.Y - rect.Y) / ga.GridSize;//inverted y-axis

                pins[j] = new GAModule.GAModulePin(pad.PadName, padX, padY, convertedWidth, convertedHeight);
                j++;
            }

            return new GAModule(comp.ID, convertedWidth, convertedHeight, pins);
        }

       

        private void SetGAParameters(GeneticAlgorithm ga)
        {
            ga.GridSize = Int32.Parse(gridSizeTextBox.Text);
            ga.XStd = this.useRandomSelectionCheckbox.Checked ? Double.Parse(xstdTextBox.Text) : 0.0;
            ga.YStd = this.useRandomSelectionCheckbox.Checked ? Double.Parse(ystdTextBox.Text) : 0.0;
            ga.Alpha = Double.Parse(alphaTextBox.Text);
            ga.Beta = Double.Parse(betaTextBox.Text);
            ga.Gamma = Double.Parse(gammaTextBox.Text);
            ga.CrossoverWidth = Double.Parse(crossoverWidthTextbox.Text);
            ga.GenerationSize = Int32.Parse(genSizeTextBox.Text);
            ga.MaxGeneration = Int32.Parse(maxGenTextBox.Text);
            ga.CrossoverRate = Double.Parse(crossoverRateTB.Text);
            ga.MutationRateRotation = Double.Parse(rotationRateTextbox.Text);
            ga.MutationRateSwap = Double.Parse(swapRateTextbox.Text);
            ga.MutationRateTranspose = Double.Parse(transposeRateTextbox.Text);
            

            int areaTotal = 0;
                int minDimension = Int32.MaxValue;
                foreach (NetList.Component comp in ImportedNetList.mComponents.Values)
                {
                    Module mod = comp.Mod;
                    if (mod == null)
                    {
                        continue;
                    }
                    Rectangle rect = mod.getBoundingRectangle();
                    areaTotal += rect.Width * rect.Height;

                    int min = Math.Min(rect.Width, rect.Height);
                    if (min < minDimension)
                        minDimension = min;

                }

            int root = (int) Math.Sqrt(areaTotal);
            double multiplier = Double.Parse(workspaceSizeTextbox.Text);

            ga.WorkspaceHeight = (int)(root * multiplier) / ga.GridSize;
            ga.WorkspaceWidth = (int)(root * multiplier) / ga.GridSize;


        }

        public static void print2DArray(ushort[,] array)
        {
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    Console.Write(array[j, i] + " ");
                }
                Console.WriteLine();
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

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult result = this.netlistOpenFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                //Add to list
                outputPathTextBox.Text = this.netlistOpenFileDialog.FileName;
            }
        }

    }
}
