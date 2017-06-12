namespace eApi.Analyzer
{
    partial class AnalyzerForm
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
            this.flowMachine = new System.Windows.Forms.FlowLayoutPanel();
            this.gbxMachine = new System.Windows.Forms.GroupBox();
            this.gbxApi = new System.Windows.Forms.GroupBox();
            this.flowApi = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbxProcessLevel = new System.Windows.Forms.ComboBox();
            this.gbxMachine.SuspendLayout();
            this.gbxApi.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowMachine
            // 
            this.flowMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowMachine.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowMachine.Location = new System.Drawing.Point(6, 25);
            this.flowMachine.MinimumSize = new System.Drawing.Size(530, 150);
            this.flowMachine.Name = "flowMachine";
            this.flowMachine.Size = new System.Drawing.Size(544, 536);
            this.flowMachine.TabIndex = 0;
            // 
            // gbxMachine
            // 
            this.gbxMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxMachine.Controls.Add(this.flowMachine);
            this.gbxMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxMachine.Location = new System.Drawing.Point(574, 39);
            this.gbxMachine.MinimumSize = new System.Drawing.Size(540, 160);
            this.gbxMachine.Name = "gbxMachine";
            this.gbxMachine.Size = new System.Drawing.Size(556, 570);
            this.gbxMachine.TabIndex = 8;
            this.gbxMachine.TabStop = false;
            this.gbxMachine.Text = "Machine";
            // 
            // gbxApi
            // 
            this.gbxApi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxApi.Controls.Add(this.flowApi);
            this.gbxApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxApi.Location = new System.Drawing.Point(12, 39);
            this.gbxApi.MinimumSize = new System.Drawing.Size(540, 160);
            this.gbxApi.Name = "gbxApi";
            this.gbxApi.Size = new System.Drawing.Size(556, 570);
            this.gbxApi.TabIndex = 9;
            this.gbxApi.TabStop = false;
            this.gbxApi.Text = "API";
            // 
            // flowApi
            // 
            this.flowApi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.flowApi.AutoScroll = true;
            this.flowApi.Location = new System.Drawing.Point(6, 25);
            this.flowApi.MinimumSize = new System.Drawing.Size(530, 150);
            this.flowApi.Name = "flowApi";
            this.flowApi.Size = new System.Drawing.Size(544, 536);
            this.flowApi.TabIndex = 0;
            // 
            // cmbxProcessLevel
            // 
            this.cmbxProcessLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbxProcessLevel.FormattingEnabled = true;
            this.cmbxProcessLevel.Items.AddRange(new object[] {
            "processed packets",
            "raw packet"});
            this.cmbxProcessLevel.Location = new System.Drawing.Point(12, 10);
            this.cmbxProcessLevel.Name = "cmbxProcessLevel";
            this.cmbxProcessLevel.Size = new System.Drawing.Size(142, 21);
            this.cmbxProcessLevel.TabIndex = 4;
            this.cmbxProcessLevel.Text = "select processing level";
            // 
            // AnalyzerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 613);
            this.Controls.Add(this.gbxApi);
            this.Controls.Add(this.gbxMachine);
            this.Controls.Add(this.cmbxProcessLevel);
            this.MaximumSize = new System.Drawing.Size(1158, 2000);
            this.MinimumSize = new System.Drawing.Size(1158, 200);
            this.Name = "AnalyzerForm";
            this.Text = "AnalyzerForm";
            this.gbxMachine.ResumeLayout(false);
            this.gbxApi.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowMachine;
        private System.Windows.Forms.GroupBox gbxMachine;
        private System.Windows.Forms.GroupBox gbxApi;
        private System.Windows.Forms.FlowLayoutPanel flowApi;
        private System.Windows.Forms.ComboBox cmbxProcessLevel;
    }
}