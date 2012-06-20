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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Program Files (x86)\KiCad\share\modules\led.mod");
            List<Module> modules = new List<Module>();
            int currentLine = 0;
            while (currentLine < lines.Length)
            {
                if (lines[currentLine].StartsWith("$MODULE"))
                {
                    Module mod = Module.parse(lines, ref currentLine);
                    modules.Add(mod);
                }
                currentLine++;
            }
            currentLine++;
        }

        private void editModulePaths_Click(object sender, EventArgs e)
        {
            ModulePathEditor editorForm = new ModulePathEditor();
            editorForm.ShowDialog();
        }
    }
}
