namespace PCB_Layout_GA
{
    partial class MainForm
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
            this.netlistTextBox = new System.Windows.Forms.TextBox();
            this.netlistBrowseButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.editModulePaths = new System.Windows.Forms.Button();
            this.netlistOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.totalModAreaLabel = new System.Windows.Forms.Label();
            this.minModuleDimLabel = new System.Windows.Forms.Label();
            this.adjustConstraintsButton = new System.Windows.Forms.Button();
            this.numComponentsLabel = new System.Windows.Forms.Label();
            this.numNetsLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.runGaButton = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.gridSizeTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.workspaceSizeTextbox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.crossoverRateTB = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.crossoverWidthTextbox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.swapRateTextbox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.transposeRateTextbox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rotationRateTextbox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gammaTextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.alphaTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.useRandomSelectionCheckbox = new System.Windows.Forms.CheckBox();
            this.betaTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.xstdTextBox = new System.Windows.Forms.TextBox();
            this.ystdTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.maxGenTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.genSizeTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Netlist Path";
            // 
            // netlistTextBox
            // 
            this.netlistTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.netlistTextBox.Location = new System.Drawing.Point(6, 36);
            this.netlistTextBox.Name = "netlistTextBox";
            this.netlistTextBox.Size = new System.Drawing.Size(371, 20);
            this.netlistTextBox.TabIndex = 1;
            // 
            // netlistBrowseButton
            // 
            this.netlistBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.netlistBrowseButton.Location = new System.Drawing.Point(383, 34);
            this.netlistBrowseButton.Name = "netlistBrowseButton";
            this.netlistBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.netlistBrowseButton.TabIndex = 2;
            this.netlistBrowseButton.Text = "Browse";
            this.netlistBrowseButton.UseVisualStyleBackColor = true;
            this.netlistBrowseButton.Click += new System.EventHandler(this.netlistBrowseButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Import Netlist";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.editModulePaths);
            this.groupBox1.Controls.Add(this.netlistTextBox);
            this.groupBox1.Controls.Add(this.netlistBrowseButton);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 91);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Netlist";
            // 
            // editModulePaths
            // 
            this.editModulePaths.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editModulePaths.Location = new System.Drawing.Point(342, 62);
            this.editModulePaths.Name = "editModulePaths";
            this.editModulePaths.Size = new System.Drawing.Size(116, 23);
            this.editModulePaths.TabIndex = 4;
            this.editModulePaths.Text = "Edit Module Paths";
            this.editModulePaths.UseVisualStyleBackColor = true;
            this.editModulePaths.Click += new System.EventHandler(this.editModulePaths_Click);
            // 
            // netlistOpenFileDialog
            // 
            this.netlistOpenFileDialog.FileName = ".net";
            this.netlistOpenFileDialog.Filter = "kicad netlist files (*.net)|*.net|All files (*.*)|*.*";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.totalModAreaLabel);
            this.groupBox2.Controls.Add(this.minModuleDimLabel);
            this.groupBox2.Controls.Add(this.adjustConstraintsButton);
            this.groupBox2.Controls.Add(this.numComponentsLabel);
            this.groupBox2.Controls.Add(this.numNetsLabel);
            this.groupBox2.Location = new System.Drawing.Point(13, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(463, 72);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Circuit Data";
            // 
            // totalModAreaLabel
            // 
            this.totalModAreaLabel.AutoSize = true;
            this.totalModAreaLabel.Location = new System.Drawing.Point(152, 43);
            this.totalModAreaLabel.Name = "totalModAreaLabel";
            this.totalModAreaLabel.Size = new System.Drawing.Size(100, 13);
            this.totalModAreaLabel.TabIndex = 4;
            this.totalModAreaLabel.Text = "Total Module Area: ";
            // 
            // minModuleDimLabel
            // 
            this.minModuleDimLabel.AutoSize = true;
            this.minModuleDimLabel.Location = new System.Drawing.Point(7, 43);
            this.minModuleDimLabel.Name = "minModuleDimLabel";
            this.minModuleDimLabel.Size = new System.Drawing.Size(89, 13);
            this.minModuleDimLabel.TabIndex = 3;
            this.minModuleDimLabel.Text = "Min Module Dim: ";
            // 
            // adjustConstraintsButton
            // 
            this.adjustConstraintsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.adjustConstraintsButton.Enabled = false;
            this.adjustConstraintsButton.Location = new System.Drawing.Point(341, 19);
            this.adjustConstraintsButton.Name = "adjustConstraintsButton";
            this.adjustConstraintsButton.Size = new System.Drawing.Size(116, 23);
            this.adjustConstraintsButton.TabIndex = 2;
            this.adjustConstraintsButton.Text = "Adjust Constraints";
            this.adjustConstraintsButton.UseVisualStyleBackColor = true;
            this.adjustConstraintsButton.Click += new System.EventHandler(this.adjustConstraintsButton_Click);
            // 
            // numComponentsLabel
            // 
            this.numComponentsLabel.AutoSize = true;
            this.numComponentsLabel.Location = new System.Drawing.Point(128, 20);
            this.numComponentsLabel.Name = "numComponentsLabel";
            this.numComponentsLabel.Size = new System.Drawing.Size(124, 13);
            this.numComponentsLabel.TabIndex = 1;
            this.numComponentsLabel.Text = "Number of Components: ";
            // 
            // numNetsLabel
            // 
            this.numNetsLabel.AutoSize = true;
            this.numNetsLabel.Location = new System.Drawing.Point(7, 20);
            this.numNetsLabel.Name = "numNetsLabel";
            this.numNetsLabel.Size = new System.Drawing.Size(87, 13);
            this.numNetsLabel.TabIndex = 0;
            this.numNetsLabel.Text = "Number of Nets: ";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.runGaButton);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.gridSizeTextBox);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.workspaceSizeTextbox);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.maxGenTextBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.genSizeTextBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(13, 189);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(463, 231);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Genetic Algorithm Parameters";
            // 
            // runGaButton
            // 
            this.runGaButton.Location = new System.Drawing.Point(323, 202);
            this.runGaButton.Name = "runGaButton";
            this.runGaButton.Size = new System.Drawing.Size(134, 23);
            this.runGaButton.TabIndex = 22;
            this.runGaButton.Text = "Run Genetic Algorithm";
            this.runGaButton.UseVisualStyleBackColor = true;
            this.runGaButton.Click += new System.EventHandler(this.runGaButton_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(103, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 13);
            this.label14.TabIndex = 21;
            this.label14.Text = "x10^-4\"";
            // 
            // gridSizeTextBox
            // 
            this.gridSizeTextBox.Location = new System.Drawing.Point(59, 45);
            this.gridSizeTextBox.Name = "gridSizeTextBox";
            this.gridSizeTextBox.Size = new System.Drawing.Size(37, 20);
            this.gridSizeTextBox.TabIndex = 20;
            this.gridSizeTextBox.Text = "200";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Grid Size: ";
            // 
            // workspaceSizeTextbox
            // 
            this.workspaceSizeTextbox.Location = new System.Drawing.Point(397, 19);
            this.workspaceSizeTextbox.Name = "workspaceSizeTextbox";
            this.workspaceSizeTextbox.Size = new System.Drawing.Size(46, 20);
            this.workspaceSizeTextbox.TabIndex = 18;
            this.workspaceSizeTextbox.Text = "5";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(301, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Workspace Size x";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.crossoverRateTB);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.crossoverWidthTextbox);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Location = new System.Drawing.Point(349, 71);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(99, 71);
            this.groupBox6.TabIndex = 16;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Crossover";
            // 
            // crossoverRateTB
            // 
            this.crossoverRateTB.Location = new System.Drawing.Point(43, 45);
            this.crossoverRateTB.Name = "crossoverRateTB";
            this.crossoverRateTB.Size = new System.Drawing.Size(46, 20);
            this.crossoverRateTB.TabIndex = 10;
            this.crossoverRateTB.Text = "0.90";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 48);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(30, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "Rate";
            // 
            // crossoverWidthTextbox
            // 
            this.crossoverWidthTextbox.Location = new System.Drawing.Point(43, 13);
            this.crossoverWidthTextbox.Name = "crossoverWidthTextbox";
            this.crossoverWidthTextbox.Size = new System.Drawing.Size(46, 20);
            this.crossoverWidthTextbox.TabIndex = 8;
            this.crossoverWidthTextbox.Text = "0.5";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Width";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.swapRateTextbox);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.transposeRateTextbox);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.rotationRateTextbox);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Location = new System.Drawing.Point(212, 71);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(131, 100);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Mutation Rates";
            // 
            // swapRateTextbox
            // 
            this.swapRateTextbox.Location = new System.Drawing.Point(69, 71);
            this.swapRateTextbox.Name = "swapRateTextbox";
            this.swapRateTextbox.Size = new System.Drawing.Size(46, 20);
            this.swapRateTextbox.TabIndex = 10;
            this.swapRateTextbox.Text = "0.01";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Swap";
            // 
            // transposeRateTextbox
            // 
            this.transposeRateTextbox.Location = new System.Drawing.Point(69, 45);
            this.transposeRateTextbox.Name = "transposeRateTextbox";
            this.transposeRateTextbox.Size = new System.Drawing.Size(46, 20);
            this.transposeRateTextbox.TabIndex = 8;
            this.transposeRateTextbox.Text = "0.01";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Transpose";
            // 
            // rotationRateTextbox
            // 
            this.rotationRateTextbox.Location = new System.Drawing.Point(69, 19);
            this.rotationRateTextbox.Name = "rotationRateTextbox";
            this.rotationRateTextbox.Size = new System.Drawing.Size(46, 20);
            this.rotationRateTextbox.TabIndex = 6;
            this.rotationRateTextbox.Text = "0.01";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Rotation";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.gammaTextBox);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.alphaTextBox);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.useRandomSelectionCheckbox);
            this.groupBox4.Controls.Add(this.betaTextBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.xstdTextBox);
            this.groupBox4.Controls.Add(this.ystdTextBox);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(6, 71);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 126);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Fitness Evaluation";
            // 
            // gammaTextBox
            // 
            this.gammaTextBox.Location = new System.Drawing.Point(53, 45);
            this.gammaTextBox.Name = "gammaTextBox";
            this.gammaTextBox.Size = new System.Drawing.Size(46, 20);
            this.gammaTextBox.TabIndex = 13;
            this.gammaTextBox.Text = "100.0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(43, 13);
            this.label15.TabIndex = 12;
            this.label15.Text = "Gamma";
            // 
            // alphaTextBox
            // 
            this.alphaTextBox.Location = new System.Drawing.Point(53, 19);
            this.alphaTextBox.Name = "alphaTextBox";
            this.alphaTextBox.Size = new System.Drawing.Size(46, 20);
            this.alphaTextBox.TabIndex = 2;
            this.alphaTextBox.Text = "1.0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Alpha";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(105, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Beta";
            // 
            // useRandomSelectionCheckbox
            // 
            this.useRandomSelectionCheckbox.AutoSize = true;
            this.useRandomSelectionCheckbox.Location = new System.Drawing.Point(15, 68);
            this.useRandomSelectionCheckbox.Name = "useRandomSelectionCheckbox";
            this.useRandomSelectionCheckbox.Size = new System.Drawing.Size(179, 17);
            this.useRandomSelectionCheckbox.TabIndex = 11;
            this.useRandomSelectionCheckbox.Text = "Use Random Selection Variation";
            this.useRandomSelectionCheckbox.UseVisualStyleBackColor = true;
            this.useRandomSelectionCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // betaTextBox
            // 
            this.betaTextBox.Location = new System.Drawing.Point(140, 19);
            this.betaTextBox.Name = "betaTextBox";
            this.betaTextBox.Size = new System.Drawing.Size(46, 20);
            this.betaTextBox.TabIndex = 4;
            this.betaTextBox.Text = "1.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "X std";
            // 
            // xstdTextBox
            // 
            this.xstdTextBox.Enabled = false;
            this.xstdTextBox.Location = new System.Drawing.Point(53, 91);
            this.xstdTextBox.Name = "xstdTextBox";
            this.xstdTextBox.Size = new System.Drawing.Size(46, 20);
            this.xstdTextBox.TabIndex = 6;
            this.xstdTextBox.Text = "0.3";
            // 
            // ystdTextBox
            // 
            this.ystdTextBox.Enabled = false;
            this.ystdTextBox.Location = new System.Drawing.Point(140, 91);
            this.ystdTextBox.Name = "ystdTextBox";
            this.ystdTextBox.Size = new System.Drawing.Size(46, 20);
            this.ystdTextBox.TabIndex = 8;
            this.ystdTextBox.Text = "0.3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(105, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Y std";
            // 
            // maxGenTextBox
            // 
            this.maxGenTextBox.Location = new System.Drawing.Point(249, 19);
            this.maxGenTextBox.Name = "maxGenTextBox";
            this.maxGenTextBox.Size = new System.Drawing.Size(46, 20);
            this.maxGenTextBox.TabIndex = 13;
            this.maxGenTextBox.Text = "500";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(153, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Max Generations ";
            // 
            // genSizeTextBox
            // 
            this.genSizeTextBox.Location = new System.Drawing.Point(101, 19);
            this.genSizeTextBox.Name = "genSizeTextBox";
            this.genSizeTextBox.Size = new System.Drawing.Size(46, 20);
            this.genSizeTextBox.TabIndex = 10;
            this.genSizeTextBox.Text = "500";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Generation Size: ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 432);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Gentic Algorithm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox netlistTextBox;
        private System.Windows.Forms.Button netlistBrowseButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button editModulePaths;
        private System.Windows.Forms.OpenFileDialog netlistOpenFileDialog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button adjustConstraintsButton;
        private System.Windows.Forms.Label numComponentsLabel;
        private System.Windows.Forms.Label numNetsLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox swapRateTextbox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox transposeRateTextbox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox rotationRateTextbox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox alphaTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox useRandomSelectionCheckbox;
        private System.Windows.Forms.TextBox betaTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox xstdTextBox;
        private System.Windows.Forms.TextBox ystdTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox maxGenTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox genSizeTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox crossoverWidthTextbox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox workspaceSizeTextbox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label minModuleDimLabel;
        private System.Windows.Forms.Label totalModAreaLabel;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox gridSizeTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button runGaButton;
        private System.Windows.Forms.TextBox gammaTextBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox crossoverRateTB;
        private System.Windows.Forms.Label label16;
    }
}

