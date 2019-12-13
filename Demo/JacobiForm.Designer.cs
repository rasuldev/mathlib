namespace Demo
{
    partial class JacobiForm
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
            this.nupOrder = new System.Windows.Forms.NumericUpDown();
            this.nupAlpha = new System.Windows.Forms.NumericUpDown();
            this.nupBeta = new System.Windows.Forms.NumericUpDown();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupBeta)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.nupBeta);
            this.panelBottom.Controls.Add(this.nupAlpha);
            this.panelBottom.Controls.Add(this.nupOrder);
            this.panelBottom.Location = new System.Drawing.Point(0, 575);
            this.panelBottom.Size = new System.Drawing.Size(1249, 123);
            this.panelBottom.Controls.SetChildIndex(this.seriesListBox, 0);
            this.panelBottom.Controls.SetChildIndex(this.nupOrder, 0);
            this.panelBottom.Controls.SetChildIndex(this.nupAlpha, 0);
            this.panelBottom.Controls.SetChildIndex(this.nupBeta, 0);
            // 
            // seriesListBox
            // 
            this.seriesListBox.Size = new System.Drawing.Size(532, 100);
            // 
            // nupOrder
            // 
            this.nupOrder.Location = new System.Drawing.Point(559, 19);
            this.nupOrder.Name = "nupOrder";
            this.nupOrder.Size = new System.Drawing.Size(234, 22);
            this.nupOrder.TabIndex = 2;
            this.nupOrder.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // nupAlpha
            // 
            this.nupAlpha.DecimalPlaces = 1;
            this.nupAlpha.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nupAlpha.Location = new System.Drawing.Point(832, 19);
            this.nupAlpha.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nupAlpha.Name = "nupAlpha";
            this.nupAlpha.Size = new System.Drawing.Size(234, 22);
            this.nupAlpha.TabIndex = 3;
            this.nupAlpha.ValueChanged += new System.EventHandler(this.nupAlpha_ValueChanged);
            // 
            // nupBeta
            // 
            this.nupBeta.DecimalPlaces = 1;
            this.nupBeta.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nupBeta.Location = new System.Drawing.Point(832, 56);
            this.nupBeta.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nupBeta.Name = "nupBeta";
            this.nupBeta.Size = new System.Drawing.Size(234, 22);
            this.nupBeta.TabIndex = 4;
            this.nupBeta.ValueChanged += new System.EventHandler(this.nupBeta_ValueChanged);
            // 
            // JacobiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 698);
            this.Name = "JacobiForm";
            this.Text = "Jacobi";
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nupOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupBeta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nupOrder;
        private System.Windows.Forms.NumericUpDown nupBeta;
        private System.Windows.Forms.NumericUpDown nupAlpha;
    }
}