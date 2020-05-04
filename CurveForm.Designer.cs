namespace Grade_Calculator_3
{
    partial class CurveForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurveForm));
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TabBasic = new System.Windows.Forms.TabPage();
            this.TabAdvanced = new System.Windows.Forms.TabPage();
            this.Tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.TabBasic);
            this.Tabs.Controls.Add(this.TabAdvanced);
            this.Tabs.Enabled = false;
            this.Tabs.Location = new System.Drawing.Point(12, 7);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(230, 279);
            this.Tabs.TabIndex = 0;
            // 
            // TabBasic
            // 
            this.TabBasic.BackColor = System.Drawing.Color.White;
            this.TabBasic.Location = new System.Drawing.Point(4, 22);
            this.TabBasic.Name = "TabBasic";
            this.TabBasic.Padding = new System.Windows.Forms.Padding(3);
            this.TabBasic.Size = new System.Drawing.Size(222, 253);
            this.TabBasic.TabIndex = 0;
            this.TabBasic.Text = "Basic";
            // 
            // TabAdvanced
            // 
            this.TabAdvanced.BackColor = System.Drawing.Color.White;
            this.TabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.TabAdvanced.Name = "TabAdvanced";
            this.TabAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.TabAdvanced.Size = new System.Drawing.Size(222, 253);
            this.TabAdvanced.TabIndex = 1;
            this.TabAdvanced.Text = "Advanced (Coming Soon)";
            // 
            // CurveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 296);
            this.Controls.Add(this.Tabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CurveForm";
            this.Text = "Curve";
            this.Load += new System.EventHandler(this.CurveForm_Load);
            this.Tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage TabBasic;
        private System.Windows.Forms.TabPage TabAdvanced;
    }
}