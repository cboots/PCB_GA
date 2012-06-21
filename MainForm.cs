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
            importForm.ShowDialog();

            //TODO get result
            
            //string[] lines = System.IO.File.ReadAllLines(@"C:\Program Files (x86)\KiCad\share\modules\libcms.mod");
            //List<Module> modules = new List<Module>();
            //int currentLine = 0;
            //while (currentLine < lines.Length)
            //{
            //    if (lines[currentLine].StartsWith("$MODULE"))
            //    {
            //        Module mod = Module.parse(lines, ref currentLine);
            //        modules.Add(mod);
            //    }
            //    currentLine++;
            //}
            //currentLine++;
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
