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
    public partial class ModulePathEditor : Form
    {
        public ModulePathEditor()
        {
            InitializeComponent();
        }

        private void ModulePathEditor_Load(object sender, EventArgs e)
        {
            string pathString = Properties.Settings.Default.LibrarySearchPaths;
            string[] paths = pathString.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);

            listView1.Clear();
            foreach(string path in paths)
            {
                listView1.Items.Add(path, path, "");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.RootFolder =
                System.Environment.SpecialFolder.MyComputer;

            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                //Add to list
                string path = folderBrowserDialog.SelectedPath;

                if (!listView1.Items.ContainsKey(path))
                {
                    listView1.Items.Add(path, path, "");
                }
                else
                {
                    MessageBox.Show("That Path is already in the List");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listView1.SelectedItems)
            {
                listView1.Items.Remove(eachItem);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ListViewItem eachItem in listView1.Items)
            {
                builder.Append(eachItem.Text).Append(';');
            }
            Properties.Settings.Default.LibrarySearchPaths = builder.ToString();
            Properties.Settings.Default.Save();
            this.Close();
        }


    }
}
