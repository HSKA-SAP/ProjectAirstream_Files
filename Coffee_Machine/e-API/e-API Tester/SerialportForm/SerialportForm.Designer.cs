namespace eApi
{
    partial class SerialportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxPorts = new System.Windows.Forms.ComboBox();
            this.textBoxBaudRate = new System.Windows.Forms.TextBox();
            this.enter = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnAutomatic = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 28F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(332, 53);
            this.label1.TabIndex = 12;
            this.label1.Text = "Serialport-Settings";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBoxPorts
            // 
            this.comboBoxPorts.Font = new System.Drawing.Font("Tahoma", 14F);
            this.comboBoxPorts.Location = new System.Drawing.Point(69, 61);
            this.comboBoxPorts.Name = "comboBoxPorts";
            this.comboBoxPorts.Size = new System.Drawing.Size(200, 31);
            this.comboBoxPorts.TabIndex = 8;
            this.toolTip.SetToolTip(this.comboBoxPorts, "Name des seriellen Ports");
            // 
            // textBoxBaudRate
            // 
            this.textBoxBaudRate.Font = new System.Drawing.Font("Tahoma", 14F);
            this.textBoxBaudRate.Location = new System.Drawing.Point(69, 100);
            this.textBoxBaudRate.Name = "textBoxBaudRate";
            this.textBoxBaudRate.Size = new System.Drawing.Size(200, 30);
            this.textBoxBaudRate.TabIndex = 9;
            this.toolTip.SetToolTip(this.textBoxBaudRate, "Baudrate der seriellen Übertragung. Default = 115200");
            // 
            // enter
            // 
            this.enter.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enter.Location = new System.Drawing.Point(69, 137);
            this.enter.Name = "enter";
            this.enter.Size = new System.Drawing.Size(200, 40);
            this.enter.TabIndex = 13;
            this.enter.Text = "Connect";
            this.enter.UseVisualStyleBackColor = true;
            this.enter.Click += new System.EventHandler(this.enter_Click);
            // 
            // btnAutomatic
            // 
            this.btnAutomatic.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutomatic.Location = new System.Drawing.Point(69, 183);
            this.btnAutomatic.Name = "btnAutomatic";
            this.btnAutomatic.Size = new System.Drawing.Size(200, 40);
            this.btnAutomatic.TabIndex = 14;
            this.btnAutomatic.Text = "Auto connect";
            this.btnAutomatic.UseVisualStyleBackColor = true;
            this.btnAutomatic.Click += new System.EventHandler(this.btnAutomatic_Click);
            // 
            // SerialportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(335, 230);
            this.ControlBox = false;
            this.Controls.Add(this.btnAutomatic);
            this.Controls.Add(this.enter);
            this.Controls.Add(this.textBoxBaudRate);
            this.Controls.Add(this.comboBoxPorts);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SerialportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial Port Settings";
            this.Load += new System.EventHandler(this.SerialportForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxPorts;
        private System.Windows.Forms.TextBox textBoxBaudRate;
        private System.Windows.Forms.Button enter;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnAutomatic;
    }
}