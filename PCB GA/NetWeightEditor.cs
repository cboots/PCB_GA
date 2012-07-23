using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCBGeneticAlgorithm
{
    public partial class NetWeightEditor : Form
    {
        public NetList.Net NetToEdit { get; set; }

        public NetWeightEditor()
        {
            InitializeComponent();
        }

        private void NetWeightEditor_Load(object sender, EventArgs e)
        {
            netNameLabel.Text = NetToEdit.FullName;
            numPinsLabel.Text = NetToEdit.Pins.Count.ToString();

            weightTB.Text = NetToEdit.Weight.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            float weight;
            float.TryParse(weightTB.Text, out weight);
            NetToEdit.Weight = weight;
            this.Close();
        }


    }
}
