namespace Grade_Calculator_3
{
    partial class Assignments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Assignments));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.DataGridView = new System.Windows.Forms.DataGridView();
            this.Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cOutOf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cReal = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TextBoxMeanPoints = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CheckBoxActive = new System.Windows.Forms.CheckBox();
            this.RadioButtonTheo = new System.Windows.Forms.RadioButton();
            this.RadioButtonReal = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TextBoxPoints = new System.Windows.Forms.TextBox();
            this.TextBoxOutOf = new System.Windows.Forms.TextBox();
            this.TextBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ComboBoxCats = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ButtonDelete = new System.Windows.Forms.Button();
            this.ButtonNew = new System.Windows.Forms.Button();
            this.ButtonClear = new System.Windows.Forms.Button();
            this.ButtonSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.DataGridView);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(766, 418);
            this.splitContainer1.SplitterDistance = 458;
            this.splitContainer1.TabIndex = 1;
            // 
            // DataGridView
            // 
            this.DataGridView.AllowUserToAddRows = false;
            this.DataGridView.AllowUserToDeleteRows = false;
            this.DataGridView.AllowUserToResizeRows = false;
            this.DataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Active,
            this.cName,
            this.cCat,
            this.cPoints,
            this.cOutOf,
            this.cPercent,
            this.cReal});
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(0, 24);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.RowHeadersVisible = false;
            this.DataGridView.Size = new System.Drawing.Size(458, 394);
            this.DataGridView.TabIndex = 1;
            this.DataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellContentClick);
            // 
            // Active
            // 
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            this.Active.ReadOnly = true;
            this.Active.Width = 45;
            // 
            // cName
            // 
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            this.cName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cName.Width = 85;
            // 
            // cCat
            // 
            this.cCat.HeaderText = "Category";
            this.cCat.Name = "cCat";
            this.cCat.ReadOnly = true;
            this.cCat.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cCat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cCat.Width = 85;
            // 
            // cPoints
            // 
            this.cPoints.HeaderText = "Points";
            this.cPoints.Name = "cPoints";
            this.cPoints.ReadOnly = true;
            this.cPoints.Width = 65;
            // 
            // cOutOf
            // 
            this.cOutOf.HeaderText = "Out Of";
            this.cOutOf.Name = "cOutOf";
            this.cOutOf.ReadOnly = true;
            this.cOutOf.Width = 65;
            // 
            // cPercent
            // 
            this.cPercent.HeaderText = "Percent";
            this.cPercent.Name = "cPercent";
            this.cPercent.ReadOnly = true;
            this.cPercent.Width = 65;
            // 
            // cReal
            // 
            this.cReal.HeaderText = "Real";
            this.cReal.Name = "cReal";
            this.cReal.ReadOnly = true;
            this.cReal.Width = 45;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(458, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.ButtonDelete);
            this.groupBox1.Controls.Add(this.ButtonNew);
            this.groupBox1.Controls.Add(this.ButtonClear);
            this.groupBox1.Controls.Add(this.ButtonSave);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 412);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Edit";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(84, 317);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 26);
            this.label7.TabIndex = 15;
            this.label7.Text = "Layout not yet finalized\r\nThis form is currently in alpha\r\n";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.TextBoxMeanPoints);
            this.groupBox3.Location = new System.Drawing.Point(87, 212);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(209, 50);
            this.groupBox3.TabIndex = 60;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Optional";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Mean Points Earned";
            // 
            // TextBoxMeanPoints
            // 
            this.TextBoxMeanPoints.Location = new System.Drawing.Point(123, 19);
            this.TextBoxMeanPoints.Name = "TextBoxMeanPoints";
            this.TextBoxMeanPoints.Size = new System.Drawing.Size(64, 20);
            this.TextBoxMeanPoints.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CheckBoxActive);
            this.groupBox2.Controls.Add(this.RadioButtonTheo);
            this.groupBox2.Controls.Add(this.RadioButtonReal);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.TextBoxPoints);
            this.groupBox2.Controls.Add(this.TextBoxOutOf);
            this.groupBox2.Controls.Add(this.TextBoxName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.ComboBoxCats);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(87, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(209, 197);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Required";
            // 
            // CheckBoxActive
            // 
            this.CheckBoxActive.AutoSize = true;
            this.CheckBoxActive.Location = new System.Drawing.Point(146, 161);
            this.CheckBoxActive.Name = "CheckBoxActive";
            this.CheckBoxActive.Size = new System.Drawing.Size(56, 17);
            this.CheckBoxActive.TabIndex = 70;
            this.CheckBoxActive.Text = "Active";
            this.CheckBoxActive.UseVisualStyleBackColor = true;
            // 
            // RadioButtonTheo
            // 
            this.RadioButtonTheo.AutoSize = true;
            this.RadioButtonTheo.Location = new System.Drawing.Point(62, 160);
            this.RadioButtonTheo.Name = "RadioButtonTheo";
            this.RadioButtonTheo.Size = new System.Drawing.Size(78, 17);
            this.RadioButtonTheo.TabIndex = 60;
            this.RadioButtonTheo.TabStop = true;
            this.RadioButtonTheo.Text = "Theoretical";
            this.RadioButtonTheo.UseVisualStyleBackColor = true;
            // 
            // RadioButtonReal
            // 
            this.RadioButtonReal.AutoSize = true;
            this.RadioButtonReal.Location = new System.Drawing.Point(9, 160);
            this.RadioButtonReal.Name = "RadioButtonReal";
            this.RadioButtonReal.Size = new System.Drawing.Size(47, 17);
            this.RadioButtonReal.TabIndex = 50;
            this.RadioButtonReal.TabStop = true;
            this.RadioButtonReal.Text = "Real";
            this.RadioButtonReal.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Category";
            // 
            // TextBoxPoints
            // 
            this.TextBoxPoints.Location = new System.Drawing.Point(90, 74);
            this.TextBoxPoints.Name = "TextBoxPoints";
            this.TextBoxPoints.Size = new System.Drawing.Size(97, 20);
            this.TextBoxPoints.TabIndex = 30;
            // 
            // TextBoxOutOf
            // 
            this.TextBoxOutOf.Location = new System.Drawing.Point(90, 125);
            this.TextBoxOutOf.Name = "TextBoxOutOf";
            this.TextBoxOutOf.Size = new System.Drawing.Size(97, 20);
            this.TextBoxOutOf.TabIndex = 40;
            // 
            // TextBoxName
            // 
            this.TextBoxName.Location = new System.Drawing.Point(66, 17);
            this.TextBoxName.Name = "TextBoxName";
            this.TextBoxName.Size = new System.Drawing.Size(121, 20);
            this.TextBoxName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(127, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "÷";
            // 
            // ComboBoxCats
            // 
            this.ComboBoxCats.FormattingEnabled = true;
            this.ComboBoxCats.Location = new System.Drawing.Point(66, 45);
            this.ComboBoxCats.Name = "ComboBoxCats";
            this.ComboBoxCats.Size = new System.Drawing.Size(121, 21);
            this.ComboBoxCats.Sorted = true;
            this.ComboBoxCats.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Points Possible";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Points Earned";
            // 
            // ButtonDelete
            // 
            this.ButtonDelete.Location = new System.Drawing.Point(6, 106);
            this.ButtonDelete.Name = "ButtonDelete";
            this.ButtonDelete.Size = new System.Drawing.Size(75, 23);
            this.ButtonDelete.TabIndex = 30;
            this.ButtonDelete.Text = "Delete";
            this.ButtonDelete.UseVisualStyleBackColor = true;
            this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // ButtonNew
            // 
            this.ButtonNew.Location = new System.Drawing.Point(6, 48);
            this.ButtonNew.Name = "ButtonNew";
            this.ButtonNew.Size = new System.Drawing.Size(75, 23);
            this.ButtonNew.TabIndex = 10;
            this.ButtonNew.Text = "New";
            this.ButtonNew.UseVisualStyleBackColor = true;
            this.ButtonNew.Click += new System.EventHandler(this.ButtonNew_Click);
            // 
            // ButtonClear
            // 
            this.ButtonClear.Location = new System.Drawing.Point(6, 77);
            this.ButtonClear.Name = "ButtonClear";
            this.ButtonClear.Size = new System.Drawing.Size(75, 23);
            this.ButtonClear.TabIndex = 20;
            this.ButtonClear.Text = "Clear";
            this.ButtonClear.UseVisualStyleBackColor = true;
            this.ButtonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // ButtonSave
            // 
            this.ButtonSave.Location = new System.Drawing.Point(6, 19);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(75, 23);
            this.ButtonSave.TabIndex = 0;
            this.ButtonSave.Text = "Save";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // Assignments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(766, 418);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Assignments";
            this.Text = "Assignments";
            this.Load += new System.EventHandler(this.Assignments_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ButtonDelete;
        private System.Windows.Forms.Button ButtonNew;
        private System.Windows.Forms.Button ButtonClear;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.TextBox TextBoxOutOf;
        private System.Windows.Forms.TextBox TextBoxPoints;
        private System.Windows.Forms.ComboBox ComboBoxCats;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TextBoxMeanPoints;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox CheckBoxActive;
        private System.Windows.Forms.RadioButton RadioButtonTheo;
        private System.Windows.Forms.RadioButton RadioButtonReal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.DataGridView DataGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Active;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCat;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn cOutOf;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPercent;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cReal;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}