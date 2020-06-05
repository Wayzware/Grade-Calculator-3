using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Grade_Calculator_3
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.addClassToolStripMenuItem = new ToolStripMenuItem();
            this.editClassToolStripMenuItem = new ToolStripMenuItem();
            this.removeClassToolStripMenuItem = new ToolStripMenuItem();
            this.refreshClassListToolStripMenuItem = new ToolStripMenuItem();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.assignmentsToolStripMenuItem = new ToolStripMenuItem();
            this.basicModeToolStripMenuItem = new ToolStripMenuItem();
            this.advancedModeToolStripMenuItem = new ToolStripMenuItem();
            this.TextBoxCat1 = new TextBox();
            this.groupBox1 = new GroupBox();
            this.TextBoxCat5 = new TextBox();
            this.TextBoxCat4 = new TextBox();
            this.TextBoxCat3 = new TextBox();
            this.TextBoxCat2 = new TextBox();
            this.label1 = new Label();
            this.ButtonForward = new Button();
            this.ButtonBack = new Button();
            this.TextBoxP1 = new TextBox();
            this.TextBoxP2 = new TextBox();
            this.TextBoxP3 = new TextBox();
            this.TextBoxP4 = new TextBox();
            this.label2 = new Label();
            this.ButtonA1 = new Button();
            this.ButtonA2 = new Button();
            this.ButtonA3 = new Button();
            this.ButtonA4 = new Button();
            this.label3 = new Label();
            this.TextBoxOutOf4 = new TextBox();
            this.TextBoxOutOf3 = new TextBox();
            this.TextBoxOutOf2 = new TextBox();
            this.TextBoxOutOf1 = new TextBox();
            this.label4 = new Label();
            this.TextBoxW4 = new TextBox();
            this.TextBoxW3 = new TextBox();
            this.TextBoxW2 = new TextBox();
            this.TextBoxW1 = new TextBox();
            this.label5 = new Label();
            this.TextBoxPer4 = new TextBox();
            this.TextBoxPer3 = new TextBox();
            this.TextBoxPer2 = new TextBox();
            this.TextBoxPer1 = new TextBox();
            this.TextBoxPer5 = new TextBox();
            this.TextBoxW5 = new TextBox();
            this.TextBoxOutOf5 = new TextBox();
            this.ButtonA5 = new Button();
            this.TextBoxP5 = new TextBox();
            this.groupBox2 = new GroupBox();
            this.LabelMeanZero = new Label();
            this.label8 = new Label();
            this.TextBoxMeanGrade = new TextBox();
            this.TextBoxMeanPercent = new TextBox();
            this.ButtonCurve = new Button();
            this.ButtonCalculate = new Button();
            this.label7 = new Label();
            this.comboBoxClasses = new ComboBox();
            this.TextBoxGrade = new TextBox();
            this.TextBoxTotalPer = new TextBox();
            this.TextBoxT5 = new TextBox();
            this.label6 = new Label();
            this.TextBoxT4 = new TextBox();
            this.TextBoxT3 = new TextBox();
            this.TextBoxT2 = new TextBox();
            this.TextBoxT1 = new TextBox();
            this.ButtonClear = new Button();
            this.groupBox3 = new GroupBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.assignmentsToolStripMenuItem});
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(655, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.addClassToolStripMenuItem,
            this.editClassToolStripMenuItem,
            this.removeClassToolStripMenuItem,
            this.refreshClassListToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addClassToolStripMenuItem
            // 
            this.addClassToolStripMenuItem.Name = "addClassToolStripMenuItem";
            this.addClassToolStripMenuItem.Size = new Size(164, 22);
            this.addClassToolStripMenuItem.Text = "Add Class";
            this.addClassToolStripMenuItem.Click += new EventHandler(this.addClassToolStripMenuItem_Click);
            // 
            // editClassToolStripMenuItem
            // 
            this.editClassToolStripMenuItem.Enabled = false;
            this.editClassToolStripMenuItem.Name = "editClassToolStripMenuItem";
            this.editClassToolStripMenuItem.Size = new Size(164, 22);
            this.editClassToolStripMenuItem.Text = "Edit Class...";
            // 
            // removeClassToolStripMenuItem
            // 
            this.removeClassToolStripMenuItem.Enabled = false;
            this.removeClassToolStripMenuItem.Name = "removeClassToolStripMenuItem";
            this.removeClassToolStripMenuItem.Size = new Size(164, 22);
            this.removeClassToolStripMenuItem.Text = "Remove Class...";
            // 
            // refreshClassListToolStripMenuItem
            // 
            this.refreshClassListToolStripMenuItem.Name = "refreshClassListToolStripMenuItem";
            this.refreshClassListToolStripMenuItem.Size = new Size(164, 22);
            this.refreshClassListToolStripMenuItem.Text = "Refresh Class List";
            this.refreshClassListToolStripMenuItem.Click += new EventHandler(this.refreshClassListToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(164, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // assignmentsToolStripMenuItem
            // 
            this.assignmentsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.basicModeToolStripMenuItem,
            this.advancedModeToolStripMenuItem});
            this.assignmentsToolStripMenuItem.Name = "assignmentsToolStripMenuItem";
            this.assignmentsToolStripMenuItem.Size = new Size(87, 20);
            this.assignmentsToolStripMenuItem.Text = "Assignments";
            // 
            // basicModeToolStripMenuItem
            // 
            this.basicModeToolStripMenuItem.Name = "basicModeToolStripMenuItem";
            this.basicModeToolStripMenuItem.Size = new Size(161, 22);
            this.basicModeToolStripMenuItem.Text = "Basic Mode";
            this.basicModeToolStripMenuItem.Click += new EventHandler(this.basicModeToolStripMenuItem_Click);
            // 
            // advancedModeToolStripMenuItem
            // 
            this.advancedModeToolStripMenuItem.Name = "advancedModeToolStripMenuItem";
            this.advancedModeToolStripMenuItem.Size = new Size(161, 22);
            this.advancedModeToolStripMenuItem.Text = "Advanced Mode";
            this.advancedModeToolStripMenuItem.Click += new EventHandler(this.advancedModeToolStripMenuItem_Click);
            // 
            // TextBoxCat1
            // 
            this.TextBoxCat1.Location = new Point(6, 32);
            this.TextBoxCat1.Name = "TextBoxCat1";
            this.TextBoxCat1.Size = new Size(156, 20);
            this.TextBoxCat1.TabIndex = 63;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TextBoxCat5);
            this.groupBox1.Controls.Add(this.TextBoxCat4);
            this.groupBox1.Controls.Add(this.TextBoxCat3);
            this.groupBox1.Controls.Add(this.TextBoxCat2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TextBoxCat1);
            this.groupBox1.Controls.Add(this.ButtonForward);
            this.groupBox1.Controls.Add(this.ButtonBack);
            this.groupBox1.Location = new Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(171, 211);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Score Input";
            // 
            // TextBoxCat5
            // 
            this.TextBoxCat5.Location = new Point(6, 136);
            this.TextBoxCat5.Name = "TextBoxCat5";
            this.TextBoxCat5.Size = new Size(156, 20);
            this.TextBoxCat5.TabIndex = 69;
            // 
            // TextBoxCat4
            // 
            this.TextBoxCat4.Location = new Point(6, 110);
            this.TextBoxCat4.Name = "TextBoxCat4";
            this.TextBoxCat4.Size = new Size(156, 20);
            this.TextBoxCat4.TabIndex = 67;
            // 
            // TextBoxCat3
            // 
            this.TextBoxCat3.Location = new Point(6, 84);
            this.TextBoxCat3.Name = "TextBoxCat3";
            this.TextBoxCat3.Size = new Size(156, 20);
            this.TextBoxCat3.TabIndex = 66;
            // 
            // TextBoxCat2
            // 
            this.TextBoxCat2.Location = new Point(6, 58);
            this.TextBoxCat2.Name = "TextBoxCat2";
            this.TextBoxCat2.Size = new Size(156, 20);
            this.TextBoxCat2.TabIndex = 65;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(56, 16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(57, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "Categories";
            // 
            // ButtonForward
            // 
            this.ButtonForward.Location = new Point(87, 162);
            this.ButtonForward.Name = "ButtonForward";
            this.ButtonForward.Size = new Size(75, 38);
            this.ButtonForward.TabIndex = 72;
            this.ButtonForward.Text = "-->";
            this.ButtonForward.UseVisualStyleBackColor = true;
            this.ButtonForward.Click += new EventHandler(this.ButtonForward_Click);
            // 
            // ButtonBack
            // 
            this.ButtonBack.Location = new Point(6, 162);
            this.ButtonBack.Name = "ButtonBack";
            this.ButtonBack.Size = new Size(75, 38);
            this.ButtonBack.TabIndex = 71;
            this.ButtonBack.Text = "<--";
            this.ButtonBack.UseVisualStyleBackColor = true;
            this.ButtonBack.Click += new EventHandler(this.ButtonBack_Click);
            // 
            // TextBoxP1
            // 
            this.TextBoxP1.Location = new Point(9, 32);
            this.TextBoxP1.MaxLength = 6;
            this.TextBoxP1.Name = "TextBoxP1";
            this.TextBoxP1.Size = new Size(39, 20);
            this.TextBoxP1.TabIndex = 66;
            this.TextBoxP1.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxP2
            // 
            this.TextBoxP2.Location = new Point(9, 58);
            this.TextBoxP2.MaxLength = 6;
            this.TextBoxP2.Name = "TextBoxP2";
            this.TextBoxP2.Size = new Size(39, 20);
            this.TextBoxP2.TabIndex = 68;
            this.TextBoxP2.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxP3
            // 
            this.TextBoxP3.Location = new Point(9, 84);
            this.TextBoxP3.MaxLength = 6;
            this.TextBoxP3.Name = "TextBoxP3";
            this.TextBoxP3.Size = new Size(39, 20);
            this.TextBoxP3.TabIndex = 70;
            this.TextBoxP3.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxP4
            // 
            this.TextBoxP4.Location = new Point(9, 110);
            this.TextBoxP4.MaxLength = 6;
            this.TextBoxP4.Name = "TextBoxP4";
            this.TextBoxP4.Size = new Size(39, 20);
            this.TextBoxP4.TabIndex = 72;
            this.TextBoxP4.TextAlign = HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(10, 16);
            this.label2.Name = "label2";
            this.label2.Size = new Size(36, 13);
            this.label2.TabIndex = 73;
            this.label2.Text = "Points";
            // 
            // ButtonA1
            // 
            this.ButtonA1.Location = new Point(56, 32);
            this.ButtonA1.Name = "ButtonA1";
            this.ButtonA1.Size = new Size(48, 20);
            this.ButtonA1.TabIndex = 74;
            this.ButtonA1.TabStop = false;
            this.ButtonA1.Text = "Add";
            this.ButtonA1.UseVisualStyleBackColor = true;
            this.ButtonA1.Click += new EventHandler(this.ButtonA1_Click);
            // 
            // ButtonA2
            // 
            this.ButtonA2.Location = new Point(56, 58);
            this.ButtonA2.Name = "ButtonA2";
            this.ButtonA2.Size = new Size(48, 20);
            this.ButtonA2.TabIndex = 75;
            this.ButtonA2.TabStop = false;
            this.ButtonA2.Text = "Add";
            this.ButtonA2.UseVisualStyleBackColor = true;
            this.ButtonA2.Click += new EventHandler(this.ButtonA2_Click);
            // 
            // ButtonA3
            // 
            this.ButtonA3.Location = new Point(56, 84);
            this.ButtonA3.Name = "ButtonA3";
            this.ButtonA3.Size = new Size(48, 20);
            this.ButtonA3.TabIndex = 75;
            this.ButtonA3.TabStop = false;
            this.ButtonA3.Text = "Add";
            this.ButtonA3.UseVisualStyleBackColor = true;
            this.ButtonA3.Click += new EventHandler(this.ButtonA3_Click);
            // 
            // ButtonA4
            // 
            this.ButtonA4.Location = new Point(56, 110);
            this.ButtonA4.Name = "ButtonA4";
            this.ButtonA4.Size = new Size(48, 20);
            this.ButtonA4.TabIndex = 76;
            this.ButtonA4.TabStop = false;
            this.ButtonA4.Text = "Add";
            this.ButtonA4.UseVisualStyleBackColor = true;
            this.ButtonA4.Click += new EventHandler(this.ButtonA4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(111, 16);
            this.label3.Name = "label3";
            this.label3.Size = new Size(38, 13);
            this.label3.TabIndex = 85;
            this.label3.Text = "Out Of";
            // 
            // TextBoxOutOf4
            // 
            this.TextBoxOutOf4.Location = new Point(110, 110);
            this.TextBoxOutOf4.MaxLength = 6;
            this.TextBoxOutOf4.Name = "TextBoxOutOf4";
            this.TextBoxOutOf4.Size = new Size(39, 20);
            this.TextBoxOutOf4.TabIndex = 73;
            this.TextBoxOutOf4.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxOutOf3
            // 
            this.TextBoxOutOf3.Location = new Point(110, 84);
            this.TextBoxOutOf3.MaxLength = 6;
            this.TextBoxOutOf3.Name = "TextBoxOutOf3";
            this.TextBoxOutOf3.Size = new Size(39, 20);
            this.TextBoxOutOf3.TabIndex = 71;
            this.TextBoxOutOf3.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxOutOf2
            // 
            this.TextBoxOutOf2.Location = new Point(110, 58);
            this.TextBoxOutOf2.MaxLength = 6;
            this.TextBoxOutOf2.Name = "TextBoxOutOf2";
            this.TextBoxOutOf2.Size = new Size(39, 20);
            this.TextBoxOutOf2.TabIndex = 69;
            this.TextBoxOutOf2.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxOutOf1
            // 
            this.TextBoxOutOf1.Location = new Point(110, 32);
            this.TextBoxOutOf1.MaxLength = 6;
            this.TextBoxOutOf1.Name = "TextBoxOutOf1";
            this.TextBoxOutOf1.Size = new Size(39, 20);
            this.TextBoxOutOf1.TabIndex = 67;
            this.TextBoxOutOf1.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(153, 16);
            this.label4.Name = "label4";
            this.label4.Size = new Size(41, 13);
            this.label4.TabIndex = 90;
            this.label4.Text = "Weight";
            // 
            // TextBoxW4
            // 
            this.TextBoxW4.Location = new Point(155, 110);
            this.TextBoxW4.MaxLength = 6;
            this.TextBoxW4.Name = "TextBoxW4";
            this.TextBoxW4.Size = new Size(39, 20);
            this.TextBoxW4.TabIndex = 89;
            this.TextBoxW4.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxW3
            // 
            this.TextBoxW3.Location = new Point(155, 84);
            this.TextBoxW3.MaxLength = 6;
            this.TextBoxW3.Name = "TextBoxW3";
            this.TextBoxW3.Size = new Size(39, 20);
            this.TextBoxW3.TabIndex = 88;
            this.TextBoxW3.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxW2
            // 
            this.TextBoxW2.Location = new Point(155, 58);
            this.TextBoxW2.MaxLength = 6;
            this.TextBoxW2.Name = "TextBoxW2";
            this.TextBoxW2.Size = new Size(39, 20);
            this.TextBoxW2.TabIndex = 87;
            this.TextBoxW2.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxW1
            // 
            this.TextBoxW1.Location = new Point(155, 32);
            this.TextBoxW1.MaxLength = 6;
            this.TextBoxW1.Name = "TextBoxW1";
            this.TextBoxW1.Size = new Size(39, 20);
            this.TextBoxW1.TabIndex = 86;
            this.TextBoxW1.TextAlign = HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new Point(8, 16);
            this.label5.Name = "label5";
            this.label5.Size = new Size(44, 13);
            this.label5.TabIndex = 95;
            this.label5.Text = "Percent";
            // 
            // TextBoxPer4
            // 
            this.TextBoxPer4.Location = new Point(10, 110);
            this.TextBoxPer4.MaxLength = 6;
            this.TextBoxPer4.Name = "TextBoxPer4";
            this.TextBoxPer4.ReadOnly = true;
            this.TextBoxPer4.Size = new Size(39, 20);
            this.TextBoxPer4.TabIndex = 94;
            this.TextBoxPer4.TabStop = false;
            this.TextBoxPer4.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxPer3
            // 
            this.TextBoxPer3.Location = new Point(10, 84);
            this.TextBoxPer3.MaxLength = 6;
            this.TextBoxPer3.Name = "TextBoxPer3";
            this.TextBoxPer3.ReadOnly = true;
            this.TextBoxPer3.Size = new Size(39, 20);
            this.TextBoxPer3.TabIndex = 93;
            this.TextBoxPer3.TabStop = false;
            this.TextBoxPer3.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxPer2
            // 
            this.TextBoxPer2.Location = new Point(10, 58);
            this.TextBoxPer2.MaxLength = 6;
            this.TextBoxPer2.Name = "TextBoxPer2";
            this.TextBoxPer2.ReadOnly = true;
            this.TextBoxPer2.Size = new Size(39, 20);
            this.TextBoxPer2.TabIndex = 92;
            this.TextBoxPer2.TabStop = false;
            this.TextBoxPer2.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxPer1
            // 
            this.TextBoxPer1.Location = new Point(10, 32);
            this.TextBoxPer1.MaxLength = 6;
            this.TextBoxPer1.Name = "TextBoxPer1";
            this.TextBoxPer1.ReadOnly = true;
            this.TextBoxPer1.Size = new Size(39, 20);
            this.TextBoxPer1.TabIndex = 91;
            this.TextBoxPer1.TabStop = false;
            this.TextBoxPer1.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxPer5
            // 
            this.TextBoxPer5.Location = new Point(10, 136);
            this.TextBoxPer5.MaxLength = 6;
            this.TextBoxPer5.Name = "TextBoxPer5";
            this.TextBoxPer5.ReadOnly = true;
            this.TextBoxPer5.Size = new Size(39, 20);
            this.TextBoxPer5.TabIndex = 102;
            this.TextBoxPer5.TabStop = false;
            this.TextBoxPer5.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxW5
            // 
            this.TextBoxW5.Location = new Point(155, 136);
            this.TextBoxW5.MaxLength = 6;
            this.TextBoxW5.Name = "TextBoxW5";
            this.TextBoxW5.Size = new Size(39, 20);
            this.TextBoxW5.TabIndex = 90;
            this.TextBoxW5.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxOutOf5
            // 
            this.TextBoxOutOf5.Location = new Point(110, 136);
            this.TextBoxOutOf5.MaxLength = 6;
            this.TextBoxOutOf5.Name = "TextBoxOutOf5";
            this.TextBoxOutOf5.Size = new Size(39, 20);
            this.TextBoxOutOf5.TabIndex = 75;
            this.TextBoxOutOf5.TextAlign = HorizontalAlignment.Right;
            // 
            // ButtonA5
            // 
            this.ButtonA5.Location = new Point(56, 136);
            this.ButtonA5.Name = "ButtonA5";
            this.ButtonA5.Size = new Size(48, 20);
            this.ButtonA5.TabIndex = 98;
            this.ButtonA5.TabStop = false;
            this.ButtonA5.Text = "Add";
            this.ButtonA5.UseVisualStyleBackColor = true;
            this.ButtonA5.Click += new EventHandler(this.ButtonA5_Click);
            // 
            // TextBoxP5
            // 
            this.TextBoxP5.Location = new Point(9, 136);
            this.TextBoxP5.MaxLength = 6;
            this.TextBoxP5.Name = "TextBoxP5";
            this.TextBoxP5.Size = new Size(39, 20);
            this.TextBoxP5.TabIndex = 74;
            this.TextBoxP5.TextAlign = HorizontalAlignment.Right;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LabelMeanZero);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.TextBoxMeanGrade);
            this.groupBox2.Controls.Add(this.TextBoxMeanPercent);
            this.groupBox2.Controls.Add(this.ButtonCurve);
            this.groupBox2.Controls.Add(this.ButtonCalculate);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.comboBoxClasses);
            this.groupBox2.Controls.Add(this.TextBoxGrade);
            this.groupBox2.Controls.Add(this.TextBoxTotalPer);
            this.groupBox2.Controls.Add(this.TextBoxT5);
            this.groupBox2.Controls.Add(this.TextBoxPer5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.TextBoxPer2);
            this.groupBox2.Controls.Add(this.TextBoxT4);
            this.groupBox2.Controls.Add(this.TextBoxPer1);
            this.groupBox2.Controls.Add(this.TextBoxT3);
            this.groupBox2.Controls.Add(this.TextBoxPer3);
            this.groupBox2.Controls.Add(this.TextBoxT2);
            this.groupBox2.Controls.Add(this.TextBoxT1);
            this.groupBox2.Controls.Add(this.TextBoxPer4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new Point(398, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(242, 211);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Score Output";
            // 
            // LabelMeanZero
            // 
            this.LabelMeanZero.AutoSize = true;
            this.LabelMeanZero.BackColor = SystemColors.Control;
            this.LabelMeanZero.ForeColor = Color.Red;
            this.LabelMeanZero.Location = new Point(98, 140);
            this.LabelMeanZero.Name = "LabelMeanZero";
            this.LabelMeanZero.Size = new Size(143, 13);
            this.LabelMeanZero.TabIndex = 1003;
            this.LabelMeanZero.Text = "Mean value(s) of 0 detected!";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new Point(185, 39);
            this.label8.Name = "label8";
            this.label8.Size = new Size(34, 13);
            this.label8.TabIndex = 1002;
            this.label8.Text = "Mean";
            // 
            // TextBoxMeanGrade
            // 
            this.TextBoxMeanGrade.Location = new Point(171, 58);
            this.TextBoxMeanGrade.Name = "TextBoxMeanGrade";
            this.TextBoxMeanGrade.ReadOnly = true;
            this.TextBoxMeanGrade.Size = new Size(65, 20);
            this.TextBoxMeanGrade.TabIndex = 1001;
            this.TextBoxMeanGrade.TabStop = false;
            // 
            // TextBoxMeanPercent
            // 
            this.TextBoxMeanPercent.Location = new Point(171, 84);
            this.TextBoxMeanPercent.Name = "TextBoxMeanPercent";
            this.TextBoxMeanPercent.ReadOnly = true;
            this.TextBoxMeanPercent.Size = new Size(65, 20);
            this.TextBoxMeanPercent.TabIndex = 1000;
            this.TextBoxMeanPercent.TabStop = false;
            // 
            // ButtonCurve
            // 
            this.ButtonCurve.Enabled = false;
            this.ButtonCurve.Location = new Point(10, 162);
            this.ButtonCurve.Name = "ButtonCurve";
            this.ButtonCurve.Size = new Size(84, 38);
            this.ButtonCurve.TabIndex = 10;
            this.ButtonCurve.Text = "Curve";
            this.ButtonCurve.UseVisualStyleBackColor = true;
            this.ButtonCurve.Click += new EventHandler(this.ButtonCurve_Click);
            // 
            // ButtonCalculate
            // 
            this.ButtonCalculate.Location = new Point(100, 162);
            this.ButtonCalculate.Name = "ButtonCalculate";
            this.ButtonCalculate.Size = new Size(134, 38);
            this.ButtonCalculate.TabIndex = 999;
            this.ButtonCalculate.Text = "Calculate";
            this.ButtonCalculate.UseVisualStyleBackColor = true;
            this.ButtonCalculate.Click += new EventHandler(this.ButtonCalculate_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new Point(103, 39);
            this.label7.Name = "label7";
            this.label7.Size = new Size(56, 13);
            this.label7.TabIndex = 110;
            this.label7.Text = "Your Total";
            // 
            // comboBoxClasses
            // 
            this.comboBoxClasses.FlatStyle = FlatStyle.System;
            this.comboBoxClasses.FormattingEnabled = true;
            this.comboBoxClasses.Location = new Point(100, 109);
            this.comboBoxClasses.Name = "comboBoxClasses";
            this.comboBoxClasses.Size = new Size(136, 21);
            this.comboBoxClasses.Sorted = true;
            this.comboBoxClasses.TabIndex = 109;
            this.comboBoxClasses.TabStop = false;
            this.comboBoxClasses.SelectedIndexChanged += new EventHandler(this.comboBoxClasses_SelectedIndexChanged);
            // 
            // TextBoxGrade
            // 
            this.TextBoxGrade.Location = new Point(100, 84);
            this.TextBoxGrade.Name = "TextBoxGrade";
            this.TextBoxGrade.ReadOnly = true;
            this.TextBoxGrade.Size = new Size(65, 20);
            this.TextBoxGrade.TabIndex = 108;
            this.TextBoxGrade.TabStop = false;
            // 
            // TextBoxTotalPer
            // 
            this.TextBoxTotalPer.Location = new Point(100, 58);
            this.TextBoxTotalPer.Name = "TextBoxTotalPer";
            this.TextBoxTotalPer.ReadOnly = true;
            this.TextBoxTotalPer.Size = new Size(65, 20);
            this.TextBoxTotalPer.TabIndex = 67;
            this.TextBoxTotalPer.TabStop = false;
            // 
            // TextBoxT5
            // 
            this.TextBoxT5.Location = new Point(55, 136);
            this.TextBoxT5.MaxLength = 6;
            this.TextBoxT5.Name = "TextBoxT5";
            this.TextBoxT5.ReadOnly = true;
            this.TextBoxT5.Size = new Size(39, 20);
            this.TextBoxT5.TabIndex = 107;
            this.TextBoxT5.TabStop = false;
            this.TextBoxT5.TextAlign = HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new Point(57, 16);
            this.label6.Name = "label6";
            this.label6.Size = new Size(31, 13);
            this.label6.TabIndex = 106;
            this.label6.Text = "Total";
            // 
            // TextBoxT4
            // 
            this.TextBoxT4.Location = new Point(55, 110);
            this.TextBoxT4.MaxLength = 6;
            this.TextBoxT4.Name = "TextBoxT4";
            this.TextBoxT4.ReadOnly = true;
            this.TextBoxT4.Size = new Size(39, 20);
            this.TextBoxT4.TabIndex = 105;
            this.TextBoxT4.TabStop = false;
            this.TextBoxT4.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxT3
            // 
            this.TextBoxT3.Location = new Point(55, 84);
            this.TextBoxT3.MaxLength = 6;
            this.TextBoxT3.Name = "TextBoxT3";
            this.TextBoxT3.ReadOnly = true;
            this.TextBoxT3.Size = new Size(39, 20);
            this.TextBoxT3.TabIndex = 104;
            this.TextBoxT3.TabStop = false;
            this.TextBoxT3.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxT2
            // 
            this.TextBoxT2.Location = new Point(55, 58);
            this.TextBoxT2.MaxLength = 6;
            this.TextBoxT2.Name = "TextBoxT2";
            this.TextBoxT2.ReadOnly = true;
            this.TextBoxT2.Size = new Size(39, 20);
            this.TextBoxT2.TabIndex = 103;
            this.TextBoxT2.TabStop = false;
            this.TextBoxT2.TextAlign = HorizontalAlignment.Right;
            // 
            // TextBoxT1
            // 
            this.TextBoxT1.Location = new Point(55, 32);
            this.TextBoxT1.MaxLength = 6;
            this.TextBoxT1.Name = "TextBoxT1";
            this.TextBoxT1.ReadOnly = true;
            this.TextBoxT1.Size = new Size(39, 20);
            this.TextBoxT1.TabIndex = 102;
            this.TextBoxT1.TabStop = false;
            this.TextBoxT1.TextAlign = HorizontalAlignment.Right;
            // 
            // ButtonClear
            // 
            this.ButtonClear.Location = new Point(9, 162);
            this.ButtonClear.Name = "ButtonClear";
            this.ButtonClear.Size = new Size(185, 38);
            this.ButtonClear.TabIndex = 999;
            this.ButtonClear.TabStop = false;
            this.ButtonClear.Text = "Clear";
            this.ButtonClear.UseVisualStyleBackColor = true;
            this.ButtonClear.Click += new EventHandler(this.ButtonClear_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ButtonClear);
            this.groupBox3.Controls.Add(this.TextBoxOutOf3);
            this.groupBox3.Controls.Add(this.TextBoxW5);
            this.groupBox3.Controls.Add(this.TextBoxOutOf5);
            this.groupBox3.Controls.Add(this.ButtonA4);
            this.groupBox3.Controls.Add(this.TextBoxOutOf1);
            this.groupBox3.Controls.Add(this.ButtonA5);
            this.groupBox3.Controls.Add(this.ButtonA3);
            this.groupBox3.Controls.Add(this.ButtonA2);
            this.groupBox3.Controls.Add(this.TextBoxP5);
            this.groupBox3.Controls.Add(this.ButtonA1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.TextBoxOutOf4);
            this.groupBox3.Controls.Add(this.TextBoxOutOf2);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.TextBoxW4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.TextBoxP1);
            this.groupBox3.Controls.Add(this.TextBoxP4);
            this.groupBox3.Controls.Add(this.TextBoxW3);
            this.groupBox3.Controls.Add(this.TextBoxW1);
            this.groupBox3.Controls.Add(this.TextBoxP2);
            this.groupBox3.Controls.Add(this.TextBoxP3);
            this.groupBox3.Controls.Add(this.TextBoxW2);
            this.groupBox3.Location = new Point(189, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(203, 211);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Score Input";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(655, 247);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new Size(671, 286);
            this.MinimumSize = new Size(671, 286);
            this.Name = "Main";
            this.Text = "Grade Calculator 3";
            this.Load += new EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem addClassToolStripMenuItem;
        private ToolStripMenuItem editClassToolStripMenuItem;
        private ToolStripMenuItem removeClassToolStripMenuItem;
        private ToolStripMenuItem refreshClassListToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        internal TextBox TextBoxCat1;
        private GroupBox groupBox1;
        internal TextBox TextBoxCat4;
        internal TextBox TextBoxCat3;
        internal TextBox TextBoxCat2;
        private Label label1;
        internal TextBox TextBoxP4;
        internal TextBox TextBoxP3;
        internal TextBox TextBoxP2;
        internal TextBox TextBoxP1;
        private Button ButtonForward;
        private Button ButtonBack;
        private Label label2;
        private Button ButtonA4;
        private Button ButtonA3;
        private Button ButtonA2;
        private Button ButtonA1;
        internal TextBox TextBoxPer5;
        internal TextBox TextBoxW5;
        internal TextBox TextBoxOutOf5;
        private Button ButtonA5;
        internal TextBox TextBoxP5;
        internal TextBox TextBoxCat5;
        private Label label5;
        internal TextBox TextBoxPer4;
        internal TextBox TextBoxPer3;
        internal TextBox TextBoxPer2;
        internal TextBox TextBoxPer1;
        private Label label4;
        internal TextBox TextBoxW4;
        internal TextBox TextBoxW3;
        internal TextBox TextBoxW2;
        internal TextBox TextBoxW1;
        private Label label3;
        internal TextBox TextBoxOutOf4;
        internal TextBox TextBoxOutOf3;
        internal TextBox TextBoxOutOf2;
        internal TextBox TextBoxOutOf1;
        private GroupBox groupBox2;
        internal TextBox TextBoxT5;
        private Label label6;
        internal TextBox TextBoxT4;
        internal TextBox TextBoxT3;
        internal TextBox TextBoxT2;
        internal TextBox TextBoxT1;
        internal TextBox TextBoxGrade;
        internal TextBox TextBoxTotalPer;
        private ComboBox comboBoxClasses;
        private Label label7;
        private Button ButtonCalculate;
        private Button ButtonClear;
        private Button ButtonCurve;
        private GroupBox groupBox3;
        private ToolStripMenuItem assignmentsToolStripMenuItem;
        private ToolStripMenuItem basicModeToolStripMenuItem;
        private ToolStripMenuItem advancedModeToolStripMenuItem;
        private Label label8;
        internal TextBox TextBoxMeanGrade;
        internal TextBox TextBoxMeanPercent;
        private Label LabelMeanZero;
    }
}

