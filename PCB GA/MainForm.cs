﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PCBGeneticAlgorithm;

namespace PCB_Layout_GA
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
            if (checkBox1.Checked)
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

        private void button2_Click(object sender, EventArgs e)
        {
            GeneticAlgorithm ga = new GeneticAlgorithm();
            
            //Initialize parameters
            ga.GridSize = Int32.Parse(gridSizeTextBox.Text);

            //Convert modules
            GAModule[]  gaModules = new GAModule[ImportedNetList.mComponents.Values.Count];
            GANet[]  gaNets = new GANet[ImportedNetList.mNets.Values.Count];

            int i = 0;
            foreach (KeyValuePair<string, NetList.Component> pair in ImportedNetList.mComponents)
            {
                gaModules[i] = ConvertComponent(ga, pair.Value);
                i++;
            }

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

                pins[j] = new GAModule.GAModulePin(padX, padY, convertedWidth, convertedHeight);
                j++;
            }

            return new GAModule(comp.ID, convertedWidth, convertedHeight, pins);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GeneticAlgorithm ga = new GeneticAlgorithm();
            ga.GridSize = Int32.Parse(gridSizeTextBox.Text);

            GAModule mod = ConvertComponent(ga, ImportedNetList.mComponents.Values.ElementAt(188));

            int i = 0;
            i++;

            int[,] array = new int[mod.Width, mod.Height];
            for (int j = 0; j < mod.Pins.Length; j++)
            {
                int x = mod.Pins[j].getX(0);
                int y = mod.Pins[j].getY(0);
                array[x, y] = j + 1;
            }

            for (int y = 0; y <= array.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= array.GetUpperBound(0); x++)
                {
                    Console.Write(array[x, y] + " ");
                }

                Console.WriteLine();
            }

        }

    }
}