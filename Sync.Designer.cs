﻿namespace Grade_Calculator_3
{
    partial class Sync
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sync));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ButtonRefreshCourses = new System.Windows.Forms.Button();
            this.CheckedListBoxCourses = new System.Windows.Forms.CheckedListBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.TextBoxAccessToken = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxCanvasURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonNext = new System.Windows.Forms.Button();
            this.ButtonBack = new System.Windows.Forms.Button();
            this.sfScrollFrame1 = new Syncfusion.WinForms.Controls.SfScrollFrame();
            this.barItem1 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.barItem2 = new Syncfusion.Windows.Forms.Tools.XPMenus.BarItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.TextBoxAccessToken);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TextBoxCanvasURL);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 156);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Initial Setup";
            this.groupBox1.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.ButtonRefreshCourses);
            this.groupBox2.Controls.Add(this.CheckedListBoxCourses);
            this.groupBox2.Location = new System.Drawing.Point(12, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 156);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Courses to Sync";
            // 
            // ButtonRefreshCourses
            // 
            this.ButtonRefreshCourses.Location = new System.Drawing.Point(6, 120);
            this.ButtonRefreshCourses.Name = "ButtonRefreshCourses";
            this.ButtonRefreshCourses.Size = new System.Drawing.Size(286, 30);
            this.ButtonRefreshCourses.TabIndex = 2;
            this.ButtonRefreshCourses.Text = "Refresh";
            this.ButtonRefreshCourses.UseVisualStyleBackColor = true;
            this.ButtonRefreshCourses.Click += new System.EventHandler(this.ButtonRefreshCourses_Click);
            // 
            // CheckedListBoxCourses
            // 
            this.CheckedListBoxCourses.FormattingEnabled = true;
            this.CheckedListBoxCourses.Location = new System.Drawing.Point(6, 20);
            this.CheckedListBoxCourses.Name = "CheckedListBoxCourses";
            this.CheckedListBoxCourses.Size = new System.Drawing.Size(286, 94);
            this.CheckedListBoxCourses.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(126, 91);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(111, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Visit this Canvas page";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(98, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 26);
            this.label4.TabIndex = 5;
            this.label4.Text = "Scroll down to access tokens.\r\nCreate a new one, and paste it here";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TextBoxAccessToken
            // 
            this.TextBoxAccessToken.Location = new System.Drawing.Point(88, 68);
            this.TextBoxAccessToken.Name = "TextBoxAccessToken";
            this.TextBoxAccessToken.Size = new System.Drawing.Size(204, 20);
            this.TextBoxAccessToken.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Access Token";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ex. https://umn.instructure.com";
            // 
            // TextBoxCanvasURL
            // 
            this.TextBoxCanvasURL.Location = new System.Drawing.Point(88, 19);
            this.TextBoxCanvasURL.Name = "TextBoxCanvasURL";
            this.TextBoxCanvasURL.Size = new System.Drawing.Size(204, 20);
            this.TextBoxCanvasURL.TabIndex = 1;
            this.TextBoxCanvasURL.TextChanged += new System.EventHandler(this.TextBoxCanvasURL_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Canvas URL";
            // 
            // ButtonNext
            // 
            this.ButtonNext.Location = new System.Drawing.Point(164, 165);
            this.ButtonNext.Name = "ButtonNext";
            this.ButtonNext.Size = new System.Drawing.Size(146, 32);
            this.ButtonNext.TabIndex = 1;
            this.ButtonNext.Text = "Next";
            this.ButtonNext.UseVisualStyleBackColor = true;
            this.ButtonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // ButtonBack
            // 
            this.ButtonBack.Location = new System.Drawing.Point(12, 165);
            this.ButtonBack.Name = "ButtonBack";
            this.ButtonBack.Size = new System.Drawing.Size(146, 32);
            this.ButtonBack.TabIndex = 2;
            this.ButtonBack.Text = "Back";
            this.ButtonBack.UseVisualStyleBackColor = true;
            this.ButtonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // barItem1
            // 
            this.barItem1.BarName = "barItem1";
            this.barItem1.ShowToolTipInPopUp = false;
            this.barItem1.SizeToFit = true;
            // 
            // barItem2
            // 
            this.barItem2.BarName = "barItem2";
            this.barItem2.ID = "555";
            this.barItem2.ShowToolTipInPopUp = false;
            this.barItem2.SizeToFit = true;
            this.barItem2.Text = "555";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(0, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 155);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // Sync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 206);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ButtonBack);
            this.Controls.Add(this.ButtonNext);
            this.Controls.Add(this.groupBox1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Sync";
            this.Text = "Sync";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TextBoxCanvasURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxAccessToken;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button ButtonNext;
        private System.Windows.Forms.Button ButtonBack;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox CheckedListBoxCourses;
        private System.Windows.Forms.Button ButtonRefreshCourses;
        private Syncfusion.WinForms.Controls.SfScrollFrame sfScrollFrame1;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem1;
        private Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}