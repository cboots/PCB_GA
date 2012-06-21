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

    }
}
