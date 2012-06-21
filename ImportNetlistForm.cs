using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCB_Layout_GA
{
    public partial class ImportNetlistForm : Form
    {
        public string NetlistPath { get; set; }

        private ModuleLibrary modules = new ModuleLibrary();

        public ImportNetlistForm()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //TODO
            string netlistpath = (string) e.Argument;
            // load netlist and build connections and part references
            NetList netlist = NetList.parseNetlistFile(netlistpath);

            // load components file and get module requirements
            logProgress(50, "Test Log");

            // find and load required modules
            

            // assemble final circuit listing
            

        }

        private void logProgress(int percent, string log)
        {
            backgroundWorker1.ReportProgress(percent, log);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            if (e.UserState is string)
            {
                textBox1.AppendText((string)e.UserState);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Save results, enable finish button
            button1.Enabled = true;
            
        }

        private void ImportNetlistForm_Load(object sender, EventArgs e)
        {
            label1.Text = "Importing Netlist: " + NetlistPath;
            backgroundWorker1.RunWorkerAsync(NetlistPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //TODO set final state
            Close();
        }
    }
}
