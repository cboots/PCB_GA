using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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

    }
}
