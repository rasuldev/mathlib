namespace Demo
{
    partial class OdeSpectralLegendre
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
            components = new System.ComponentModel.Container();
            nupIterCount = new NumericUpDown();
            contextMenuStrip1 = new ContextMenuStrip(components);
            label1 = new Label();
            label2 = new Label();
            nupOrder = new NumericUpDown();
            label3 = new Label();
            nupNodesCount = new NumericUpDown();
            label4 = new Label();
            nupChunksCount = new NumericUpDown();
            label5 = new Label();
            tbLog = new TextBox();
            button1 = new Button();
            panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nupIterCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nupOrder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nupNodesCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nupChunksCount).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Size = new Size(1194, 468);
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(button1);
            panelBottom.Controls.Add(tbLog);
            panelBottom.Controls.Add(label5);
            panelBottom.Controls.Add(label4);
            panelBottom.Controls.Add(nupChunksCount);
            panelBottom.Controls.Add(label3);
            panelBottom.Controls.Add(nupNodesCount);
            panelBottom.Controls.Add(label2);
            panelBottom.Controls.Add(nupOrder);
            panelBottom.Controls.Add(label1);
            panelBottom.Controls.Add(nupIterCount);
            panelBottom.Location = new Point(0, 563);
            panelBottom.Size = new Size(1194, 180);
            panelBottom.Controls.SetChildIndex(seriesListBox, 0);
            panelBottom.Controls.SetChildIndex(nupIterCount, 0);
            panelBottom.Controls.SetChildIndex(label1, 0);
            panelBottom.Controls.SetChildIndex(nupOrder, 0);
            panelBottom.Controls.SetChildIndex(label2, 0);
            panelBottom.Controls.SetChildIndex(nupNodesCount, 0);
            panelBottom.Controls.SetChildIndex(label3, 0);
            panelBottom.Controls.SetChildIndex(nupChunksCount, 0);
            panelBottom.Controls.SetChildIndex(label4, 0);
            panelBottom.Controls.SetChildIndex(label5, 0);
            panelBottom.Controls.SetChildIndex(tbLog, 0);
            panelBottom.Controls.SetChildIndex(button1, 0);
            // 
            // seriesListBox
            // 
            seriesListBox.Location = new Point(0, 15);
            seriesListBox.Margin = new Padding(4, 5, 4, 5);
            seriesListBox.Size = new Size(466, 124);
            // 
            // nupIterCount
            // 
            nupIterCount.Location = new Point(479, 30);
            nupIterCount.Margin = new Padding(4);
            nupIterCount.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nupIterCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nupIterCount.Name = "nupIterCount";
            nupIterCount.Size = new Size(140, 23);
            nupIterCount.TabIndex = 2;
            nupIterCount.Value = new decimal(new int[] { 10, 0, 0, 0 });
            nupIterCount.ValueChanged += ValueChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(480, 9);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(90, 15);
            label1.TabIndex = 3;
            label1.Text = "Iterations count";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(480, 58);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(102, 15);
            label2.TabIndex = 5;
            label2.Text = "Partial sums order";
            // 
            // nupOrder
            // 
            nupOrder.Location = new Point(479, 79);
            nupOrder.Margin = new Padding(4);
            nupOrder.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nupOrder.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nupOrder.Name = "nupOrder";
            nupOrder.Size = new Size(140, 23);
            nupOrder.TabIndex = 4;
            nupOrder.Value = new decimal(new int[] { 10, 0, 0, 0 });
            nupOrder.ValueChanged += ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(640, 9);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(75, 15);
            label3.TabIndex = 7;
            label3.Text = "Nodes count";
            // 
            // nupNodesCount
            // 
            nupNodesCount.Location = new Point(640, 30);
            nupNodesCount.Margin = new Padding(4);
            nupNodesCount.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nupNodesCount.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            nupNodesCount.Name = "nupNodesCount";
            nupNodesCount.Size = new Size(78, 23);
            nupNodesCount.TabIndex = 6;
            nupNodesCount.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            nupNodesCount.ValueChanged += ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(640, 58);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(81, 15);
            label4.TabIndex = 9;
            label4.Text = "Chunks count";
            // 
            // nupChunksCount
            // 
            nupChunksCount.Location = new Point(640, 79);
            nupChunksCount.Margin = new Padding(4);
            nupChunksCount.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nupChunksCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nupChunksCount.Name = "nupChunksCount";
            nupChunksCount.Size = new Size(78, 23);
            nupChunksCount.TabIndex = 8;
            nupChunksCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nupChunksCount.ValueChanged += ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(724, 35);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(28, 15);
            label5.TabIndex = 10;
            label5.Text = "info";
            // 
            // tbLog
            // 
            tbLog.Location = new Point(796, 29);
            tbLog.Multiline = true;
            tbLog.Name = "tbLog";
            tbLog.Size = new Size(363, 71);
            tbLog.TabIndex = 11;
            // 
            // button1
            // 
            button1.Location = new Point(729, 73);
            button1.Name = "button1";
            button1.Size = new Size(54, 26);
            button1.TabIndex = 12;
            button1.Text = "Batch";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // OdeSpectralLegendre
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1194, 693);
            Margin = new Padding(4, 5, 4, 5);
            Name = "OdeSpectralLegendre";
            Text = "OdeSpectral";
            Controls.SetChildIndex(panelMain, 0);
            Controls.SetChildIndex(panelBottom, 0);
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nupIterCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)nupOrder).EndInit();
            ((System.ComponentModel.ISupportInitialize)nupNodesCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)nupChunksCount).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nupIterCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nupNodesCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nupOrder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nupChunksCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button button1;
    }
}