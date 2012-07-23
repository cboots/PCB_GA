using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PCBGeneticAlgorithm
{
    public partial class ConstraintsEditor : Form
    {
        public NetList ImportedNetList { get; set; }

        public string FilePath { get; set; }

        public ConstraintsEditor()
        {
            InitializeComponent();
        }


        private void ConstraintsEditor_Load(object sender, EventArgs e)
        {
            FillNetListListView();
        }

        private void FillNetListListView()
        {
            int i = 0;
            listView1.Items.Clear();
            foreach (KeyValuePair<int, NetList.Net> pair in ImportedNetList.mNets)
            {
                NetList.Net net = pair.Value;
                ListViewItem lvi = new ListViewItem();

                //Index
                lvi.Text = i.ToString();
                lvi.SubItems.Add(net.ID.ToString());
                lvi.SubItems.Add(net.FullName);
                lvi.SubItems.Add(net.Pins.Count.ToString());
                lvi.SubItems.Add(net.Weight.ToString());
                listView1.Items.Add(lvi);

                i++;//increment index
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                ImportConstraints(this.openFileDialog1.FileName);
            }
        }

        private void ImportConstraints(string path)
        {
            if (File.Exists(path))
            {
                
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                
                int ID = int.Parse(item.SubItems[1].Text);
                NetList.Net net = ImportedNetList.mNets[ID];
                NetWeightEditor editor = new NetWeightEditor();
                editor.NetToEdit = net;
                DialogResult result = editor.ShowDialog();
                if (result == DialogResult.OK)
                {
                    FillNetListListView();
                }
            }
        }

    }
}
