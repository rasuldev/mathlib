﻿namespace Demo
{
    partial class MixHaar2
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
            this.nupN = new System.Windows.Forms.NumericUpDown();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupN)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.nupN);
            this.panelBottom.Location = new System.Drawing.Point(0, 673);
            this.panelBottom.Size = new System.Drawing.Size(1430, 123);
            this.panelBottom.Controls.SetChildIndex(this.seriesListBox, 0);
            this.panelBottom.Controls.SetChildIndex(this.nupN, 0);
            // 
            // seriesListBox
            // 
            this.seriesListBox.Size = new System.Drawing.Size(532, 100);
            // 
            // nupN
            // 
            this.nupN.Location = new System.Drawing.Point(568, 20);
            this.nupN.Name = "nupN";
            this.nupN.Size = new System.Drawing.Size(120, 22);
            this.nupN.TabIndex = 2;
            this.nupN.ValueChanged += new System.EventHandler(this.nupN_ValueChanged);
            // 
            // MixHaar2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1430, 796);
            this.Name = "MixHaar2";
            this.Text = "MixHaar2";
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nupN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nupN;
    }
}