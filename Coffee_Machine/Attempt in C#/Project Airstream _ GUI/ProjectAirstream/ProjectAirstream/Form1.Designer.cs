namespace ProjectAirstream
{
    partial class serialForm
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
            this.raspberryRTB = new System.Windows.Forms.RichTextBox();
            this.coffeeRTB = new System.Windows.Forms.RichTextBox();
            this.raspbLabel = new System.Windows.Forms.Label();
            this.coffLabel = new System.Windows.Forms.Label();
            this.doRinseBtn = new System.Windows.Forms.Button();
            this.commandsGB = new System.Windows.Forms.GroupBox();
            this.prstLbl = new System.Windows.Forms.Label();
            this.prtsOpenLbl = new System.Windows.Forms.Label();
            this.openPortsBtn = new System.Windows.Forms.Button();
            this.commandsGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // raspberryRTB
            // 
            this.raspberryRTB.Location = new System.Drawing.Point(23, 269);
            this.raspberryRTB.Name = "raspberryRTB";
            this.raspberryRTB.Size = new System.Drawing.Size(235, 203);
            this.raspberryRTB.TabIndex = 0;
            this.raspberryRTB.Text = "";
            // 
            // coffeeRTB
            // 
            this.coffeeRTB.Location = new System.Drawing.Point(320, 269);
            this.coffeeRTB.Name = "coffeeRTB";
            this.coffeeRTB.Size = new System.Drawing.Size(235, 203);
            this.coffeeRTB.TabIndex = 1;
            this.coffeeRTB.Text = "";
            // 
            // raspbLabel
            // 
            this.raspbLabel.AutoSize = true;
            this.raspbLabel.Location = new System.Drawing.Point(47, 246);
            this.raspbLabel.Name = "raspbLabel";
            this.raspbLabel.Size = new System.Drawing.Size(171, 20);
            this.raspbLabel.TabIndex = 2;
            this.raspbLabel.Text = "Output from Raspberry";
            // 
            // coffLabel
            // 
            this.coffLabel.AutoSize = true;
            this.coffLabel.Location = new System.Drawing.Point(330, 246);
            this.coffLabel.Name = "coffLabel";
            this.coffLabel.Size = new System.Drawing.Size(210, 20);
            this.coffLabel.TabIndex = 3;
            this.coffLabel.Text = "Output from Coffee Machine";
            this.coffLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // doRinseBtn
            // 
            this.doRinseBtn.Location = new System.Drawing.Point(17, 42);
            this.doRinseBtn.Name = "doRinseBtn";
            this.doRinseBtn.Size = new System.Drawing.Size(107, 38);
            this.doRinseBtn.TabIndex = 4;
            this.doRinseBtn.Text = "Do Rinse";
            this.doRinseBtn.UseVisualStyleBackColor = true;
            // 
            // commandsGB
            // 
            this.commandsGB.Controls.Add(this.doRinseBtn);
            this.commandsGB.Location = new System.Drawing.Point(58, 70);
            this.commandsGB.Name = "commandsGB";
            this.commandsGB.Size = new System.Drawing.Size(430, 150);
            this.commandsGB.TabIndex = 6;
            this.commandsGB.TabStop = false;
            this.commandsGB.Text = "Commands";
            // 
            // prstLbl
            // 
            this.prstLbl.AutoSize = true;
            this.prstLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prstLbl.Location = new System.Drawing.Point(506, 35);
            this.prstLbl.Name = "prstLbl";
            this.prstLbl.Size = new System.Drawing.Size(61, 20);
            this.prstLbl.TabIndex = 7;
            this.prstLbl.Text = "Ports :";
            // 
            // prtsOpenLbl
            // 
            this.prtsOpenLbl.AutoSize = true;
            this.prtsOpenLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prtsOpenLbl.ForeColor = System.Drawing.Color.Red;
            this.prtsOpenLbl.Location = new System.Drawing.Point(506, 55);
            this.prtsOpenLbl.Name = "prtsOpenLbl";
            this.prtsOpenLbl.Size = new System.Drawing.Size(64, 20);
            this.prtsOpenLbl.TabIndex = 8;
            this.prtsOpenLbl.Text = "Closed";
            // 
            // openPortsBtn
            // 
            this.openPortsBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.openPortsBtn.Location = new System.Drawing.Point(230, 12);
            this.openPortsBtn.Name = "openPortsBtn";
            this.openPortsBtn.Size = new System.Drawing.Size(129, 51);
            this.openPortsBtn.TabIndex = 9;
            this.openPortsBtn.Text = "Open Ports";
            this.openPortsBtn.UseVisualStyleBackColor = true;
            this.openPortsBtn.Click += new System.EventHandler(this.openPortsBtn_Click);
            // 
            // serialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 587);
            this.Controls.Add(this.openPortsBtn);
            this.Controls.Add(this.prtsOpenLbl);
            this.Controls.Add(this.prstLbl);
            this.Controls.Add(this.commandsGB);
            this.Controls.Add(this.coffLabel);
            this.Controls.Add(this.raspbLabel);
            this.Controls.Add(this.coffeeRTB);
            this.Controls.Add(this.raspberryRTB);
            this.Name = "serialForm";
            this.Text = "EverSys API";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.commandsGB.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox raspberryRTB;
        private System.Windows.Forms.RichTextBox coffeeRTB;
        private System.Windows.Forms.Label raspbLabel;
        private System.Windows.Forms.Label coffLabel;
        private System.Windows.Forms.Button doRinseBtn;
        private System.Windows.Forms.GroupBox commandsGB;
        private System.Windows.Forms.Label prstLbl;
        private System.Windows.Forms.Label prtsOpenLbl;
        private System.Windows.Forms.Button openPortsBtn;
    }
}

