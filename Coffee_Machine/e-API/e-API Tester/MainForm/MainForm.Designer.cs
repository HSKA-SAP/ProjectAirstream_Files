namespace eApi
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pnlCmd = new System.Windows.Forms.Panel();
            this.flowCmdList = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCmdBtn = new System.Windows.Forms.Panel();
            this.btnSort = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnChangePort = new System.Windows.Forms.Button();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblBaud = new System.Windows.Forms.Label();
            this.gbxIn = new System.Windows.Forms.GroupBox();
            this.lblInEOT = new System.Windows.Forms.Label();
            this.txtInEOT = new System.Windows.Forms.TextBox();
            this.lblInCRC = new System.Windows.Forms.Label();
            this.txtInCRC = new System.Windows.Forms.TextBox();
            this.lblInSOH = new System.Windows.Forms.Label();
            this.txtInSOH = new System.Windows.Forms.TextBox();
            this.txtInDL = new System.Windows.Forms.TextBox();
            this.txtInMP = new System.Windows.Forms.TextBox();
            this.lblInDL = new System.Windows.Forms.Label();
            this.lblInMP = new System.Windows.Forms.Label();
            this.lblInMI = new System.Windows.Forms.Label();
            this.txtInMI = new System.Windows.Forms.TextBox();
            this.lblInDA = new System.Windows.Forms.Label();
            this.txtInDA = new System.Windows.Forms.TextBox();
            this.lblInSA = new System.Windows.Forms.Label();
            this.txtInSA = new System.Windows.Forms.TextBox();
            this.lblInPN = new System.Windows.Forms.Label();
            this.txtInPN = new System.Windows.Forms.TextBox();
            this.lblInPIE = new System.Windows.Forms.Label();
            this.txtInPIE = new System.Windows.Forms.TextBox();
            this.lblInPIP = new System.Windows.Forms.Label();
            this.txtInPIP = new System.Windows.Forms.TextBox();
            this.txtInRawData = new System.Windows.Forms.TextBox();
            this.lblInRawData = new System.Windows.Forms.Label();
            this.txtInData = new System.Windows.Forms.TextBox();
            this.lblInData = new System.Windows.Forms.Label();
            this.gbxOut = new System.Windows.Forms.GroupBox();
            this.lblOutEOT = new System.Windows.Forms.Label();
            this.txtOutEOT = new System.Windows.Forms.TextBox();
            this.lblOutCRC = new System.Windows.Forms.Label();
            this.txtOutCRC = new System.Windows.Forms.TextBox();
            this.lblOutSOH = new System.Windows.Forms.Label();
            this.txtOutSOH = new System.Windows.Forms.TextBox();
            this.txtOutDL = new System.Windows.Forms.TextBox();
            this.txtOutMP = new System.Windows.Forms.TextBox();
            this.lblOutDL = new System.Windows.Forms.Label();
            this.lblOutMP = new System.Windows.Forms.Label();
            this.lblOutMI = new System.Windows.Forms.Label();
            this.txtOutMI = new System.Windows.Forms.TextBox();
            this.lblOutDA = new System.Windows.Forms.Label();
            this.txtOutDA = new System.Windows.Forms.TextBox();
            this.lblOutSA = new System.Windows.Forms.Label();
            this.txtOutSA = new System.Windows.Forms.TextBox();
            this.lblOutPN = new System.Windows.Forms.Label();
            this.txtOutPN = new System.Windows.Forms.TextBox();
            this.lblOutPIE = new System.Windows.Forms.Label();
            this.txtOutPIE = new System.Windows.Forms.TextBox();
            this.lblOutPIP = new System.Windows.Forms.Label();
            this.txtOutPIP = new System.Windows.Forms.TextBox();
            this.txtOutRawData = new System.Windows.Forms.TextBox();
            this.lblOutRawData = new System.Windows.Forms.Label();
            this.txtOutData = new System.Windows.Forms.TextBox();
            this.lblOutData = new System.Windows.Forms.Label();
            this.pngSearch = new System.Windows.Forms.PictureBox();
            this.pngLogoBig = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.flowCommStatus = new System.Windows.Forms.TableLayoutPanel();
            this.btnAnalyzerPng = new System.Windows.Forms.Button();
            this.btnConnection = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlComm = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pnlCmd.SuspendLayout();
            this.pnlCmdBtn.SuspendLayout();
            this.gbxIn.SuspendLayout();
            this.gbxOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pngSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pngLogoBig)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlComm.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(12, 168);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(209, 29);
            this.txtSearch.TabIndex = 0;
            this.toolTip.SetToolTip(this.txtSearch, "search for a command with a specific name. If it is a exact match a click on ente" +
                    "r will execute the command.");
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // pnlCmd
            // 
            this.pnlCmd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCmd.Controls.Add(this.flowCmdList);
            this.pnlCmd.Location = new System.Drawing.Point(12, 202);
            this.pnlCmd.Name = "pnlCmd";
            this.pnlCmd.Size = new System.Drawing.Size(209, 392);
            this.pnlCmd.TabIndex = 14;
            // 
            // flowCmdList
            // 
            this.flowCmdList.ColumnCount = 1;
            this.flowCmdList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.flowCmdList.Location = new System.Drawing.Point(-1, -1);
            this.flowCmdList.MinimumSize = new System.Drawing.Size(209, 392);
            this.flowCmdList.Name = "flowCmdList";
            this.flowCmdList.RowCount = 1;
            this.flowCmdList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.flowCmdList.Size = new System.Drawing.Size(209, 392);
            this.flowCmdList.TabIndex = 5;
            this.toolTip.SetToolTip(this.flowCmdList, "all available commands");
            // 
            // pnlCmdBtn
            // 
            this.pnlCmdBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCmdBtn.Controls.Add(this.btnSort);
            this.pnlCmdBtn.Controls.Add(this.btnFilter);
            this.pnlCmdBtn.Controls.Add(this.btnExpand);
            this.pnlCmdBtn.Location = new System.Drawing.Point(12, 593);
            this.pnlCmdBtn.Name = "pnlCmdBtn";
            this.pnlCmdBtn.Size = new System.Drawing.Size(209, 53);
            this.pnlCmdBtn.TabIndex = 18;
            // 
            // btnSort
            // 
            this.btnSort.BackColor = System.Drawing.SystemColors.Control;
            this.btnSort.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSort.BackgroundImage")));
            this.btnSort.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSort.FlatAppearance.BorderColor = System.Drawing.SystemColors.Window;
            this.btnSort.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSort.Location = new System.Drawing.Point(3, 3);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(45, 45);
            this.btnSort.TabIndex = 14;
            this.btnSort.TabStop = false;
            this.toolTip.SetToolTip(this.btnSort, "sort commands alphabeticaly");
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFilter.BackgroundImage")));
            this.btnFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFilter.Enabled = false;
            this.btnFilter.Location = new System.Drawing.Point(75, 3);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(45, 45);
            this.btnFilter.TabIndex = 16;
            this.btnFilter.TabStop = false;
            this.toolTip.SetToolTip(this.btnFilter, "filter commands");
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Visible = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnExpand
            // 
            this.btnExpand.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExpand.BackgroundImage")));
            this.btnExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExpand.Location = new System.Drawing.Point(158, 3);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(45, 45);
            this.btnExpand.TabIndex = 15;
            this.btnExpand.TabStop = false;
            this.toolTip.SetToolTip(this.btnExpand, "  ");
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnChangePort
            // 
            this.btnChangePort.Location = new System.Drawing.Point(115, 7);
            this.btnChangePort.Name = "btnChangePort";
            this.btnChangePort.Size = new System.Drawing.Size(77, 35);
            this.btnChangePort.TabIndex = 2;
            this.btnChangePort.TabStop = false;
            this.btnChangePort.Text = "change";
            this.toolTip.SetToolTip(this.btnChangePort, "change port and/or baudrate");
            this.btnChangePort.UseVisualStyleBackColor = true;
            this.btnChangePort.Click += new System.EventHandler(this.btnChangePort_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(1, 12);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(32, 13);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port: ";
            // 
            // lblBaud
            // 
            this.lblBaud.AutoSize = true;
            this.lblBaud.Location = new System.Drawing.Point(1, 28);
            this.lblBaud.Name = "lblBaud";
            this.lblBaud.Size = new System.Drawing.Size(81, 13);
            this.lblBaud.TabIndex = 0;
            this.lblBaud.Text = "Baudrate: xxxxx";
            // 
            // gbxIn
            // 
            this.gbxIn.Controls.Add(this.lblInEOT);
            this.gbxIn.Controls.Add(this.txtInEOT);
            this.gbxIn.Controls.Add(this.lblInCRC);
            this.gbxIn.Controls.Add(this.txtInCRC);
            this.gbxIn.Controls.Add(this.lblInSOH);
            this.gbxIn.Controls.Add(this.txtInSOH);
            this.gbxIn.Controls.Add(this.txtInDL);
            this.gbxIn.Controls.Add(this.txtInMP);
            this.gbxIn.Controls.Add(this.lblInDL);
            this.gbxIn.Controls.Add(this.lblInMP);
            this.gbxIn.Controls.Add(this.lblInMI);
            this.gbxIn.Controls.Add(this.txtInMI);
            this.gbxIn.Controls.Add(this.lblInDA);
            this.gbxIn.Controls.Add(this.txtInDA);
            this.gbxIn.Controls.Add(this.lblInSA);
            this.gbxIn.Controls.Add(this.txtInSA);
            this.gbxIn.Controls.Add(this.lblInPN);
            this.gbxIn.Controls.Add(this.txtInPN);
            this.gbxIn.Controls.Add(this.lblInPIE);
            this.gbxIn.Controls.Add(this.txtInPIE);
            this.gbxIn.Controls.Add(this.lblInPIP);
            this.gbxIn.Controls.Add(this.txtInPIP);
            this.gbxIn.Controls.Add(this.txtInRawData);
            this.gbxIn.Controls.Add(this.lblInRawData);
            this.gbxIn.Controls.Add(this.txtInData);
            this.gbxIn.Controls.Add(this.lblInData);
            this.gbxIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxIn.Location = new System.Drawing.Point(227, 329);
            this.gbxIn.Name = "gbxIn";
            this.gbxIn.Size = new System.Drawing.Size(422, 317);
            this.gbxIn.TabIndex = 23;
            this.gbxIn.TabStop = false;
            this.gbxIn.Tag = "";
            this.gbxIn.Text = "in";
            // 
            // lblInEOT
            // 
            this.lblInEOT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInEOT.Location = new System.Drawing.Point(68, 141);
            this.lblInEOT.Name = "lblInEOT";
            this.lblInEOT.Size = new System.Drawing.Size(43, 15);
            this.lblInEOT.TabIndex = 88;
            this.lblInEOT.Text = "EOT";
            this.lblInEOT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInEOT
            // 
            this.txtInEOT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInEOT.Location = new System.Drawing.Point(71, 159);
            this.txtInEOT.Name = "txtInEOT";
            this.txtInEOT.ReadOnly = true;
            this.txtInEOT.Size = new System.Drawing.Size(33, 26);
            this.txtInEOT.TabIndex = 87;
            this.txtInEOT.Text = "01";
            this.txtInEOT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblInCRC
            // 
            this.lblInCRC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInCRC.Location = new System.Drawing.Point(14, 141);
            this.lblInCRC.Name = "lblInCRC";
            this.lblInCRC.Size = new System.Drawing.Size(53, 15);
            this.lblInCRC.TabIndex = 86;
            this.lblInCRC.Text = "CRC";
            this.lblInCRC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInCRC
            // 
            this.txtInCRC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInCRC.Location = new System.Drawing.Point(12, 159);
            this.txtInCRC.Name = "txtInCRC";
            this.txtInCRC.ReadOnly = true;
            this.txtInCRC.Size = new System.Drawing.Size(53, 26);
            this.txtInCRC.TabIndex = 85;
            this.txtInCRC.Text = "FFFF";
            this.txtInCRC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblInSOH
            // 
            this.lblInSOH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInSOH.Location = new System.Drawing.Point(7, 22);
            this.lblInSOH.Name = "lblInSOH";
            this.lblInSOH.Size = new System.Drawing.Size(41, 15);
            this.lblInSOH.TabIndex = 84;
            this.lblInSOH.Text = "SOH";
            this.lblInSOH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInSOH
            // 
            this.txtInSOH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInSOH.Location = new System.Drawing.Point(10, 39);
            this.txtInSOH.Name = "txtInSOH";
            this.txtInSOH.ReadOnly = true;
            this.txtInSOH.Size = new System.Drawing.Size(33, 26);
            this.txtInSOH.TabIndex = 83;
            this.txtInSOH.Text = "01";
            this.txtInSOH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtInDL
            // 
            this.txtInDL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInDL.Location = new System.Drawing.Point(361, 39);
            this.txtInDL.Name = "txtInDL";
            this.txtInDL.ReadOnly = true;
            this.txtInDL.Size = new System.Drawing.Size(53, 26);
            this.txtInDL.TabIndex = 82;
            this.txtInDL.Text = "FFFF";
            this.txtInDL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtInMP
            // 
            this.txtInMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInMP.Location = new System.Drawing.Point(302, 39);
            this.txtInMP.Name = "txtInMP";
            this.txtInMP.ReadOnly = true;
            this.txtInMP.Size = new System.Drawing.Size(53, 26);
            this.txtInMP.TabIndex = 81;
            this.txtInMP.Text = "FFFF";
            this.txtInMP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblInDL
            // 
            this.lblInDL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInDL.Location = new System.Drawing.Point(361, 22);
            this.lblInDL.Name = "lblInDL";
            this.lblInDL.Size = new System.Drawing.Size(53, 15);
            this.lblInDL.TabIndex = 80;
            this.lblInDL.Text = "DL";
            this.lblInDL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInMP
            // 
            this.lblInMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInMP.Location = new System.Drawing.Point(302, 22);
            this.lblInMP.Name = "lblInMP";
            this.lblInMP.Size = new System.Drawing.Size(53, 15);
            this.lblInMP.TabIndex = 79;
            this.lblInMP.Text = "MP";
            this.lblInMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInMI
            // 
            this.lblInMI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInMI.Location = new System.Drawing.Point(265, 22);
            this.lblInMI.Name = "lblInMI";
            this.lblInMI.Size = new System.Drawing.Size(33, 15);
            this.lblInMI.TabIndex = 78;
            this.lblInMI.Text = "MI";
            this.lblInMI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInMI
            // 
            this.txtInMI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInMI.Location = new System.Drawing.Point(263, 39);
            this.txtInMI.Name = "txtInMI";
            this.txtInMI.ReadOnly = true;
            this.txtInMI.Size = new System.Drawing.Size(33, 26);
            this.txtInMI.TabIndex = 77;
            this.txtInMI.Text = "FF";
            this.txtInMI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblInDA
            // 
            this.lblInDA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInDA.Location = new System.Drawing.Point(205, 22);
            this.lblInDA.Name = "lblInDA";
            this.lblInDA.Size = new System.Drawing.Size(33, 15);
            this.lblInDA.TabIndex = 76;
            this.lblInDA.Text = "DA";
            this.lblInDA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInDA
            // 
            this.txtInDA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInDA.Location = new System.Drawing.Point(205, 39);
            this.txtInDA.Name = "txtInDA";
            this.txtInDA.ReadOnly = true;
            this.txtInDA.Size = new System.Drawing.Size(33, 26);
            this.txtInDA.TabIndex = 75;
            this.txtInDA.Text = "FF";
            this.txtInDA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblInSA
            // 
            this.lblInSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInSA.Location = new System.Drawing.Point(167, 22);
            this.lblInSA.Name = "lblInSA";
            this.lblInSA.Size = new System.Drawing.Size(33, 15);
            this.lblInSA.TabIndex = 74;
            this.lblInSA.Text = "SA";
            this.lblInSA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInSA
            // 
            this.txtInSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInSA.Location = new System.Drawing.Point(166, 39);
            this.txtInSA.Name = "txtInSA";
            this.txtInSA.ReadOnly = true;
            this.txtInSA.Size = new System.Drawing.Size(33, 26);
            this.txtInSA.TabIndex = 73;
            this.txtInSA.Text = "FF";
            this.txtInSA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblInPN
            // 
            this.lblInPN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInPN.Location = new System.Drawing.Point(128, 22);
            this.lblInPN.Name = "lblInPN";
            this.lblInPN.Size = new System.Drawing.Size(33, 14);
            this.lblInPN.TabIndex = 72;
            this.lblInPN.Text = "PN";
            this.lblInPN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInPN
            // 
            this.txtInPN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInPN.Location = new System.Drawing.Point(127, 39);
            this.txtInPN.Name = "txtInPN";
            this.txtInPN.ReadOnly = true;
            this.txtInPN.Size = new System.Drawing.Size(33, 26);
            this.txtInPN.TabIndex = 71;
            this.txtInPN.Text = "FF";
            this.txtInPN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblInPIE
            // 
            this.lblInPIE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInPIE.Location = new System.Drawing.Point(91, 22);
            this.lblInPIE.Name = "lblInPIE";
            this.lblInPIE.Size = new System.Drawing.Size(29, 15);
            this.lblInPIE.TabIndex = 70;
            this.lblInPIE.Text = "PIE";
            this.lblInPIE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInPIE
            // 
            this.txtInPIE.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInPIE.Location = new System.Drawing.Point(88, 39);
            this.txtInPIE.Name = "txtInPIE";
            this.txtInPIE.ReadOnly = true;
            this.txtInPIE.Size = new System.Drawing.Size(33, 26);
            this.txtInPIE.TabIndex = 69;
            this.txtInPIE.Text = "FF";
            this.txtInPIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblInPIP
            // 
            this.lblInPIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInPIP.Location = new System.Drawing.Point(52, 22);
            this.lblInPIP.Name = "lblInPIP";
            this.lblInPIP.Size = new System.Drawing.Size(29, 15);
            this.lblInPIP.TabIndex = 68;
            this.lblInPIP.Text = "PIP";
            this.lblInPIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInPIP
            // 
            this.txtInPIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInPIP.Location = new System.Drawing.Point(49, 39);
            this.txtInPIP.Name = "txtInPIP";
            this.txtInPIP.ReadOnly = true;
            this.txtInPIP.Size = new System.Drawing.Size(33, 26);
            this.txtInPIP.TabIndex = 67;
            this.txtInPIP.Text = "FF";
            this.txtInPIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtInRawData
            // 
            this.txtInRawData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInRawData.Location = new System.Drawing.Point(12, 216);
            this.txtInRawData.Multiline = true;
            this.txtInRawData.Name = "txtInRawData";
            this.txtInRawData.ReadOnly = true;
            this.txtInRawData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInRawData.Size = new System.Drawing.Size(402, 91);
            this.txtInRawData.TabIndex = 66;
            this.txtInRawData.TabStop = false;
            // 
            // lblInRawData
            // 
            this.lblInRawData.AutoSize = true;
            this.lblInRawData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInRawData.Location = new System.Drawing.Point(9, 197);
            this.lblInRawData.Name = "lblInRawData";
            this.lblInRawData.Size = new System.Drawing.Size(68, 16);
            this.lblInRawData.TabIndex = 65;
            this.lblInRawData.Text = "Raw data:";
            // 
            // txtInData
            // 
            this.txtInData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInData.Location = new System.Drawing.Point(11, 93);
            this.txtInData.Multiline = true;
            this.txtInData.Name = "txtInData";
            this.txtInData.ReadOnly = true;
            this.txtInData.Size = new System.Drawing.Size(403, 41);
            this.txtInData.TabIndex = 64;
            this.txtInData.TabStop = false;
            // 
            // lblInData
            // 
            this.lblInData.AutoSize = true;
            this.lblInData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInData.Location = new System.Drawing.Point(9, 74);
            this.lblInData.Name = "lblInData";
            this.lblInData.Size = new System.Drawing.Size(40, 16);
            this.lblInData.TabIndex = 63;
            this.lblInData.Text = "Data:";
            // 
            // gbxOut
            // 
            this.gbxOut.Controls.Add(this.lblOutEOT);
            this.gbxOut.Controls.Add(this.txtOutEOT);
            this.gbxOut.Controls.Add(this.lblOutCRC);
            this.gbxOut.Controls.Add(this.txtOutCRC);
            this.gbxOut.Controls.Add(this.lblOutSOH);
            this.gbxOut.Controls.Add(this.txtOutSOH);
            this.gbxOut.Controls.Add(this.txtOutDL);
            this.gbxOut.Controls.Add(this.txtOutMP);
            this.gbxOut.Controls.Add(this.lblOutDL);
            this.gbxOut.Controls.Add(this.lblOutMP);
            this.gbxOut.Controls.Add(this.lblOutMI);
            this.gbxOut.Controls.Add(this.txtOutMI);
            this.gbxOut.Controls.Add(this.lblOutDA);
            this.gbxOut.Controls.Add(this.txtOutDA);
            this.gbxOut.Controls.Add(this.lblOutSA);
            this.gbxOut.Controls.Add(this.txtOutSA);
            this.gbxOut.Controls.Add(this.lblOutPN);
            this.gbxOut.Controls.Add(this.txtOutPN);
            this.gbxOut.Controls.Add(this.lblOutPIE);
            this.gbxOut.Controls.Add(this.txtOutPIE);
            this.gbxOut.Controls.Add(this.lblOutPIP);
            this.gbxOut.Controls.Add(this.txtOutPIP);
            this.gbxOut.Controls.Add(this.txtOutRawData);
            this.gbxOut.Controls.Add(this.lblOutRawData);
            this.gbxOut.Controls.Add(this.txtOutData);
            this.gbxOut.Controls.Add(this.lblOutData);
            this.gbxOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxOut.Location = new System.Drawing.Point(227, 12);
            this.gbxOut.Name = "gbxOut";
            this.gbxOut.Size = new System.Drawing.Size(422, 315);
            this.gbxOut.TabIndex = 24;
            this.gbxOut.TabStop = false;
            this.gbxOut.Text = "out";
            // 
            // lblOutEOT
            // 
            this.lblOutEOT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutEOT.Location = new System.Drawing.Point(68, 141);
            this.lblOutEOT.Name = "lblOutEOT";
            this.lblOutEOT.Size = new System.Drawing.Size(43, 15);
            this.lblOutEOT.TabIndex = 62;
            this.lblOutEOT.Text = "EOT";
            this.lblOutEOT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOutEOT
            // 
            this.txtOutEOT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutEOT.Location = new System.Drawing.Point(71, 159);
            this.txtOutEOT.Name = "txtOutEOT";
            this.txtOutEOT.ReadOnly = true;
            this.txtOutEOT.Size = new System.Drawing.Size(33, 26);
            this.txtOutEOT.TabIndex = 61;
            this.txtOutEOT.Text = "01";
            this.txtOutEOT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOutCRC
            // 
            this.lblOutCRC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutCRC.Location = new System.Drawing.Point(14, 141);
            this.lblOutCRC.Name = "lblOutCRC";
            this.lblOutCRC.Size = new System.Drawing.Size(53, 15);
            this.lblOutCRC.TabIndex = 60;
            this.lblOutCRC.Text = "CRC";
            this.lblOutCRC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOutCRC
            // 
            this.txtOutCRC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutCRC.Location = new System.Drawing.Point(12, 159);
            this.txtOutCRC.Name = "txtOutCRC";
            this.txtOutCRC.ReadOnly = true;
            this.txtOutCRC.Size = new System.Drawing.Size(53, 26);
            this.txtOutCRC.TabIndex = 59;
            this.txtOutCRC.Text = "FFFF";
            this.txtOutCRC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOutSOH
            // 
            this.lblOutSOH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutSOH.Location = new System.Drawing.Point(7, 22);
            this.lblOutSOH.Name = "lblOutSOH";
            this.lblOutSOH.Size = new System.Drawing.Size(41, 15);
            this.lblOutSOH.TabIndex = 58;
            this.lblOutSOH.Text = "SOH";
            this.lblOutSOH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOutSOH
            // 
            this.txtOutSOH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutSOH.Location = new System.Drawing.Point(10, 39);
            this.txtOutSOH.Name = "txtOutSOH";
            this.txtOutSOH.ReadOnly = true;
            this.txtOutSOH.Size = new System.Drawing.Size(33, 26);
            this.txtOutSOH.TabIndex = 57;
            this.txtOutSOH.Text = "01";
            this.txtOutSOH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOutDL
            // 
            this.txtOutDL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutDL.Location = new System.Drawing.Point(361, 39);
            this.txtOutDL.Name = "txtOutDL";
            this.txtOutDL.ReadOnly = true;
            this.txtOutDL.Size = new System.Drawing.Size(53, 26);
            this.txtOutDL.TabIndex = 56;
            this.txtOutDL.Text = "FFFF";
            this.txtOutDL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOutMP
            // 
            this.txtOutMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutMP.Location = new System.Drawing.Point(302, 39);
            this.txtOutMP.Name = "txtOutMP";
            this.txtOutMP.ReadOnly = true;
            this.txtOutMP.Size = new System.Drawing.Size(53, 26);
            this.txtOutMP.TabIndex = 55;
            this.txtOutMP.Text = "FFFF";
            this.txtOutMP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOutDL
            // 
            this.lblOutDL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutDL.Location = new System.Drawing.Point(361, 22);
            this.lblOutDL.Name = "lblOutDL";
            this.lblOutDL.Size = new System.Drawing.Size(53, 15);
            this.lblOutDL.TabIndex = 54;
            this.lblOutDL.Text = "DL";
            this.lblOutDL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutMP
            // 
            this.lblOutMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutMP.Location = new System.Drawing.Point(302, 22);
            this.lblOutMP.Name = "lblOutMP";
            this.lblOutMP.Size = new System.Drawing.Size(53, 15);
            this.lblOutMP.TabIndex = 53;
            this.lblOutMP.Text = "MP";
            this.lblOutMP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutMI
            // 
            this.lblOutMI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutMI.Location = new System.Drawing.Point(265, 22);
            this.lblOutMI.Name = "lblOutMI";
            this.lblOutMI.Size = new System.Drawing.Size(33, 15);
            this.lblOutMI.TabIndex = 52;
            this.lblOutMI.Text = "MI";
            this.lblOutMI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOutMI
            // 
            this.txtOutMI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutMI.Location = new System.Drawing.Point(263, 39);
            this.txtOutMI.Name = "txtOutMI";
            this.txtOutMI.ReadOnly = true;
            this.txtOutMI.Size = new System.Drawing.Size(33, 26);
            this.txtOutMI.TabIndex = 51;
            this.txtOutMI.Text = "FF";
            this.txtOutMI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOutDA
            // 
            this.lblOutDA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutDA.Location = new System.Drawing.Point(205, 22);
            this.lblOutDA.Name = "lblOutDA";
            this.lblOutDA.Size = new System.Drawing.Size(33, 15);
            this.lblOutDA.TabIndex = 50;
            this.lblOutDA.Text = "DA";
            this.lblOutDA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOutDA
            // 
            this.txtOutDA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutDA.Location = new System.Drawing.Point(205, 39);
            this.txtOutDA.Name = "txtOutDA";
            this.txtOutDA.ReadOnly = true;
            this.txtOutDA.Size = new System.Drawing.Size(33, 26);
            this.txtOutDA.TabIndex = 49;
            this.txtOutDA.Text = "FF";
            this.txtOutDA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOutSA
            // 
            this.lblOutSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutSA.Location = new System.Drawing.Point(167, 22);
            this.lblOutSA.Name = "lblOutSA";
            this.lblOutSA.Size = new System.Drawing.Size(33, 15);
            this.lblOutSA.TabIndex = 48;
            this.lblOutSA.Text = "SA";
            this.lblOutSA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOutSA
            // 
            this.txtOutSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutSA.Location = new System.Drawing.Point(166, 39);
            this.txtOutSA.Name = "txtOutSA";
            this.txtOutSA.ReadOnly = true;
            this.txtOutSA.Size = new System.Drawing.Size(33, 26);
            this.txtOutSA.TabIndex = 47;
            this.txtOutSA.Text = "FF";
            this.txtOutSA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOutPN
            // 
            this.lblOutPN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutPN.Location = new System.Drawing.Point(128, 22);
            this.lblOutPN.Name = "lblOutPN";
            this.lblOutPN.Size = new System.Drawing.Size(33, 14);
            this.lblOutPN.TabIndex = 46;
            this.lblOutPN.Text = "PN";
            this.lblOutPN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOutPN
            // 
            this.txtOutPN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutPN.Location = new System.Drawing.Point(127, 39);
            this.txtOutPN.Name = "txtOutPN";
            this.txtOutPN.ReadOnly = true;
            this.txtOutPN.Size = new System.Drawing.Size(33, 26);
            this.txtOutPN.TabIndex = 45;
            this.txtOutPN.Text = "FF";
            this.txtOutPN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOutPIE
            // 
            this.lblOutPIE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutPIE.Location = new System.Drawing.Point(91, 22);
            this.lblOutPIE.Name = "lblOutPIE";
            this.lblOutPIE.Size = new System.Drawing.Size(29, 15);
            this.lblOutPIE.TabIndex = 44;
            this.lblOutPIE.Text = "PIE";
            this.lblOutPIE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOutPIE
            // 
            this.txtOutPIE.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutPIE.Location = new System.Drawing.Point(88, 39);
            this.txtOutPIE.Name = "txtOutPIE";
            this.txtOutPIE.ReadOnly = true;
            this.txtOutPIE.Size = new System.Drawing.Size(33, 26);
            this.txtOutPIE.TabIndex = 43;
            this.txtOutPIE.Text = "FF";
            this.txtOutPIE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOutPIP
            // 
            this.lblOutPIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutPIP.Location = new System.Drawing.Point(52, 22);
            this.lblOutPIP.Name = "lblOutPIP";
            this.lblOutPIP.Size = new System.Drawing.Size(29, 15);
            this.lblOutPIP.TabIndex = 42;
            this.lblOutPIP.Text = "PIP";
            this.lblOutPIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOutPIP
            // 
            this.txtOutPIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutPIP.Location = new System.Drawing.Point(49, 39);
            this.txtOutPIP.Name = "txtOutPIP";
            this.txtOutPIP.ReadOnly = true;
            this.txtOutPIP.Size = new System.Drawing.Size(33, 26);
            this.txtOutPIP.TabIndex = 41;
            this.txtOutPIP.Text = "FF";
            this.txtOutPIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOutRawData
            // 
            this.txtOutRawData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutRawData.Location = new System.Drawing.Point(12, 217);
            this.txtOutRawData.Multiline = true;
            this.txtOutRawData.Name = "txtOutRawData";
            this.txtOutRawData.ReadOnly = true;
            this.txtOutRawData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutRawData.Size = new System.Drawing.Size(402, 91);
            this.txtOutRawData.TabIndex = 40;
            this.txtOutRawData.TabStop = false;
            // 
            // lblOutRawData
            // 
            this.lblOutRawData.AutoSize = true;
            this.lblOutRawData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutRawData.Location = new System.Drawing.Point(9, 198);
            this.lblOutRawData.Name = "lblOutRawData";
            this.lblOutRawData.Size = new System.Drawing.Size(68, 16);
            this.lblOutRawData.TabIndex = 39;
            this.lblOutRawData.Text = "Raw data:";
            // 
            // txtOutData
            // 
            this.txtOutData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutData.Location = new System.Drawing.Point(11, 93);
            this.txtOutData.Multiline = true;
            this.txtOutData.Name = "txtOutData";
            this.txtOutData.ReadOnly = true;
            this.txtOutData.Size = new System.Drawing.Size(403, 41);
            this.txtOutData.TabIndex = 38;
            this.txtOutData.TabStop = false;
            // 
            // lblOutData
            // 
            this.lblOutData.AutoSize = true;
            this.lblOutData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutData.Location = new System.Drawing.Point(9, 74);
            this.lblOutData.Name = "lblOutData";
            this.lblOutData.Size = new System.Drawing.Size(40, 16);
            this.lblOutData.TabIndex = 28;
            this.lblOutData.Text = "Data:";
            // 
            // pngSearch
            // 
            this.pngSearch.BackColor = System.Drawing.Color.White;
            this.pngSearch.Image = ((System.Drawing.Image)(resources.GetObject("pngSearch.Image")));
            this.pngSearch.Location = new System.Drawing.Point(194, 171);
            this.pngSearch.Name = "pngSearch";
            this.pngSearch.Size = new System.Drawing.Size(23, 23);
            this.pngSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pngSearch.TabIndex = 19;
            this.pngSearch.TabStop = false;
            // 
            // pngLogoBig
            // 
            this.pngLogoBig.BackColor = System.Drawing.SystemColors.Window;
            this.pngLogoBig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pngLogoBig.Image = global::eApi.Properties.Resources.eAPI_small;
            this.pngLogoBig.InitialImage = ((System.Drawing.Image)(resources.GetObject("pngLogoBig.InitialImage")));
            this.pngLogoBig.Location = new System.Drawing.Point(12, 12);
            this.pngLogoBig.Name = "pngLogoBig";
            this.pngLogoBig.Size = new System.Drawing.Size(209, 150);
            this.pngLogoBig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pngLogoBig.TabIndex = 11;
            this.pngLogoBig.TabStop = false;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // flowCommStatus
            // 
            this.flowCommStatus.AutoScroll = true;
            this.flowCommStatus.ColumnCount = 1;
            this.flowCommStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.flowCommStatus.Location = new System.Drawing.Point(-1, -1);
            this.flowCommStatus.Name = "flowCommStatus";
            this.flowCommStatus.RowCount = 4;
            this.flowCommStatus.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.flowCommStatus.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.flowCommStatus.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.flowCommStatus.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.flowCommStatus.Size = new System.Drawing.Size(199, 426);
            this.flowCommStatus.TabIndex = 28;
            this.toolTip.SetToolTip(this.flowCommStatus, "log of the last/current transmission");
            // 
            // btnAnalyzerPng
            // 
            this.btnAnalyzerPng.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.btnAnalyzerPng.BackgroundImage = global::eApi.Properties.Resources.eAPI_Analyzer;
            this.btnAnalyzerPng.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAnalyzerPng.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalyzerPng.Location = new System.Drawing.Point(655, 12);
            this.btnAnalyzerPng.Name = "btnAnalyzerPng";
            this.btnAnalyzerPng.Size = new System.Drawing.Size(199, 117);
            this.btnAnalyzerPng.TabIndex = 25;
            this.btnAnalyzerPng.TabStop = false;
            this.toolTip.SetToolTip(this.btnAnalyzerPng, "open the API Analyzer to monitor external APIs  and to see the raw transmissions");
            this.btnAnalyzerPng.UseVisualStyleBackColor = false;
            this.btnAnalyzerPng.Click += new System.EventHandler(this.btnAnalyzerPng_Click);
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(29, 48);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(146, 32);
            this.btnConnection.TabIndex = 11;
            this.btnConnection.TabStop = false;
            this.btnConnection.Text = "Disconnect this API";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnConnection);
            this.panel1.Controls.Add(this.lblBaud);
            this.panel1.Controls.Add(this.btnChangePort);
            this.panel1.Controls.Add(this.lblPort);
            this.panel1.Location = new System.Drawing.Point(655, 559);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(199, 87);
            this.panel1.TabIndex = 27;
            // 
            // pnlComm
            // 
            this.pnlComm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlComm.Controls.Add(this.flowCommStatus);
            this.pnlComm.Location = new System.Drawing.Point(655, 135);
            this.pnlComm.Name = "pnlComm";
            this.pnlComm.Size = new System.Drawing.Size(199, 426);
            this.pnlComm.TabIndex = 28;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(865, 651);
            this.Controls.Add(this.pnlCmdBtn);
            this.Controls.Add(this.pnlComm);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnAnalyzerPng);
            this.Controls.Add(this.gbxOut);
            this.Controls.Add(this.gbxIn);
            this.Controls.Add(this.pnlCmd);
            this.Controls.Add(this.pngSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.pngLogoBig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(872, 680);
            this.MinimumSize = new System.Drawing.Size(870, 621);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "e-Api Tester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.pnlCmd.ResumeLayout(false);
            this.pnlCmdBtn.ResumeLayout(false);
            this.gbxIn.ResumeLayout(false);
            this.gbxIn.PerformLayout();
            this.gbxOut.ResumeLayout(false);
            this.gbxOut.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pngSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pngLogoBig)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlComm.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.PictureBox pngSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.PictureBox pngLogoBig;
        private System.Windows.Forms.Panel pnlCmd;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblBaud;
        private System.Windows.Forms.GroupBox gbxIn;
        private System.Windows.Forms.GroupBox gbxOut;
        private System.Windows.Forms.Label lblOutData;
        private System.Windows.Forms.TextBox txtOutData;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnChangePort;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel flowCommStatus;
        private System.Windows.Forms.Panel pnlComm;
        private System.Windows.Forms.Panel pnlCmdBtn;
        private System.Windows.Forms.TableLayoutPanel flowCmdList;
        private System.Windows.Forms.TextBox txtOutRawData;
        private System.Windows.Forms.Label lblOutRawData;
        private System.Windows.Forms.Button btnAnalyzerPng;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtOutDL;
        private System.Windows.Forms.TextBox txtOutMP;
        private System.Windows.Forms.Label lblOutDL;
        private System.Windows.Forms.Label lblOutMP;
        private System.Windows.Forms.Label lblOutMI;
        private System.Windows.Forms.TextBox txtOutMI;
        private System.Windows.Forms.Label lblOutDA;
        private System.Windows.Forms.TextBox txtOutDA;
        private System.Windows.Forms.Label lblOutSA;
        private System.Windows.Forms.TextBox txtOutSA;
        private System.Windows.Forms.Label lblOutPN;
        private System.Windows.Forms.TextBox txtOutPN;
        private System.Windows.Forms.Label lblOutPIE;
        private System.Windows.Forms.TextBox txtOutPIE;
        private System.Windows.Forms.Label lblOutPIP;
        private System.Windows.Forms.TextBox txtOutPIP;
        private System.Windows.Forms.Label lblOutSOH;
        private System.Windows.Forms.TextBox txtOutSOH;
        private System.Windows.Forms.Label lblOutCRC;
        private System.Windows.Forms.TextBox txtOutCRC;
        private System.Windows.Forms.Label lblOutEOT;
        private System.Windows.Forms.TextBox txtOutEOT;
        private System.Windows.Forms.Label lblInEOT;
        private System.Windows.Forms.TextBox txtInEOT;
        private System.Windows.Forms.Label lblInCRC;
        private System.Windows.Forms.TextBox txtInCRC;
        private System.Windows.Forms.Label lblInSOH;
        private System.Windows.Forms.TextBox txtInSOH;
        private System.Windows.Forms.TextBox txtInDL;
        private System.Windows.Forms.TextBox txtInMP;
        private System.Windows.Forms.Label lblInDL;
        private System.Windows.Forms.Label lblInMP;
        private System.Windows.Forms.Label lblInMI;
        private System.Windows.Forms.TextBox txtInMI;
        private System.Windows.Forms.Label lblInDA;
        private System.Windows.Forms.TextBox txtInDA;
        private System.Windows.Forms.Label lblInSA;
        private System.Windows.Forms.TextBox txtInSA;
        private System.Windows.Forms.Label lblInPN;
        private System.Windows.Forms.TextBox txtInPN;
        private System.Windows.Forms.Label lblInPIE;
        private System.Windows.Forms.TextBox txtInPIE;
        private System.Windows.Forms.Label lblInPIP;
        private System.Windows.Forms.TextBox txtInPIP;
        private System.Windows.Forms.TextBox txtInRawData;
        private System.Windows.Forms.Label lblInRawData;
        private System.Windows.Forms.TextBox txtInData;
        private System.Windows.Forms.Label lblInData;

    }
}

