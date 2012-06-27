using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PCBGeneticAlgorithm;

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

        }
    }
}
