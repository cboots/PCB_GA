namespace PCBGeneticAlgorithm
{
    partial class NetWeightEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.netNameLabel = new System.Windows.Forms.Label();
            this.numPinsLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.weightTB = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Net Name: ";
            // 
            // netNameLabel
            // 
            this.netNameLabel.AutoSize = true;
            this.netNameLabel.Location = new System.Drawing.Point(80, 13);
            this.netNameLabel.Name = "netNameLabel";
            this.netNameLabel.Size = new System.Drawing.Size(38, 13);
            this.netNameLabel.TabIndex = 1;
            this.netNameLabel.Text = "NAME";
            // 
            // numPinsLabel
            // 
            this.numPinsLabel.AutoSize = true;
            this.numPinsLabel.Location = new System.Drawing.Point(80, 42);
            this.numPinsLabel.Name = "numPinsLabel";
            this.numPinsLabel.Size = new System.Drawing.Size(13, 13);
            this.numPinsLabel.TabIndex = 3;
            this.numPinsLabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "# Pins:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Weight:";
            // 
            // weightTB
            // 
            this.weightTB.Location = new System.Drawing.Point(19, 96);
            this.weightTB.Name = "weightTB";
            this.weightTB.Size = new System.Drawing.Size(142, 20);
            this.weightTB.TabIndex = 5;
            this.weightTB.Text = "1.0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 123);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Save Changes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NetWeightEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(173, 156);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.weightTB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numPinsLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.netNameLabel);
            this.Controls.Add(this.label1);
            this.Name = "NetWeightEditor";
            this.Text = "NetWeightEditor";
            this.Load += new System.EventHandler(this.NetWeightEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label netNameLabel;
        private System.Windows.Forms.Label numPinsLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox weightTB;
        private System.Windows.Forms.Button button1;
    }
}