
namespace Demo
{
    partial class TchebForm
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
            this.nupT = new System.Windows.Forms.NumericUpDown();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupT)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Size = new System.Drawing.Size(1390, 635);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.nupT);
            this.panelBottom.Controls.Add(this.nupN);
            this.panelBottom.Location = new System.Drawing.Point(0, 660);
            this.panelBottom.Size = new System.Drawing.Size(1390, 123);
            this.panelBottom.Controls.SetChildIndex(this.seriesListBox, 0);
            this.panelBottom.Controls.SetChildIndex(this.nupN, 0);
            this.panelBottom.Controls.SetChildIndex(this.nupT, 0);
            // 
            // nupN
            // 
            this.nupN.Location = new System.Drawing.Point(565, 20);
            this.nupN.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nupN.Name = "nupN";
            this.nupN.Size = new System.Drawing.Size(120, 22);
            this.nupN.TabIndex = 6;
            this.nupN.ValueChanged += new System.EventHandler(this.nupN_ValueChanged);
            // 
            // nupT
            // 
            this.nupT.DecimalPlaces = 2;
            this.nupT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nupT.Location = new System.Drawing.Point(566, 56);
            this.nupT.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupT.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nupT.Name = "nupT";
            this.nupT.Size = new System.Drawing.Size(119, 22);
            this.nupT.TabIndex = 7;
            this.nupT.ValueChanged += new System.EventHandler(this.nupN_ValueChanged);
            // 
            // TchebForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1390, 783);
            this.Name = "TchebForm";
            this.Text = "Tcheb";
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nupN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupT)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nupN;
        private System.Windows.Forms.NumericUpDown nupT;
    }
}