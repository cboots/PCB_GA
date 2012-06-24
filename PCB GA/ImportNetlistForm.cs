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
    public partial class ImportNetlistForm : Form
    {
        public string NetlistPath { get; set; }

        private ModuleLibrary modules = new ModuleLibrary();

        public NetList NetListResult { get; set; }

        public ImportNetlistForm()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //TODO
            string netlistpath = (string) e.Argument;
            logProgress("Reading Netlist File: " + netlistpath + "\n");
            // load netlist and build connections and part references
            NetList netlist = NetList.parseNetlistFile(netlistpath);

            logProgress(20, "Netlist Read Done\n");

            // load components file and get module requirements
            string componentpath = Path.ChangeExtension(netlistpath, ".cmp");

            logProgress("Loading Components File: " + componentpath + "\n");
            Dictionary<string, string> cmp_mod = NetList.parseComponentsFile(componentpath);

            logProgress(50, "Loading Component File Done\n");

            // find and load required modules

            logProgress("Scanning Module Library Indecies\n");
            //use search paths to index modules.  Libraries will be lazily initialized
            modules.RefreshModuleIndex();
            logProgress(60, "Module Library Index Built\n");

            logProgress("Linking Components to Modules\n");
            foreach (string componentName in cmp_mod.Keys)
            {
                string moduleLibName;
                //Find reference in cmp-mod map
                cmp_mod.TryGetValue(componentName, out moduleLibName);

                //Find same component in netlist
                NetList.Component component;
                bool found = netlist.mComponents.TryGetValue(componentName, out component);

                if (found)
                {
                    //link up the appropriate module to the netlist
                    component.Mod = modules.FindModule(moduleLibName);
                    if (component.Mod == null)
                    {
                        logProgress("Component: " + component.ID + " module not found: " + moduleLibName);
                    }
                }

            }
            logProgress(100, "Modules Linked.\n");
            logProgress("Done\n");
            //Return Netlist object
            e.Result = netlist;
        }

        int mProgress = 0;
        public void logProgress(string log)
        {
            logProgress(mProgress, log);
        }

        public void logProgress(int percent, string log)
        {
            mProgress = percent;
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
            NetListResult = (NetList) e.Result;
        }

        private void ImportNetlistForm_Load(object sender, EventArgs e)
        {
            label1.Text = "Importing Netlist: " + NetlistPath;
            backgroundWorker1.RunWorkerAsync(NetlistPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (NetListResult != null)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.Abort;
            }
            Close();
        }

    }
}
