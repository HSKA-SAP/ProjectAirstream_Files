namespace eApi.Commands.GetProductDumpForm
{
    partial class ProductDumpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ProductDumpForm ) );
            this.lblProcDump = new System.Windows.Forms.Label( );
            this.SuspendLayout( );
            // 
            // lblProcDump
            // 
            this.lblProcDump.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
            this.lblProcDump.Location = new System.Drawing.Point( 13, 13 );
            this.lblProcDump.Name = "lblProcDump";
            this.lblProcDump.Size = new System.Drawing.Size( 259, 540 );
            this.lblProcDump.TabIndex = 0;
            // 
            // ProductDumpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 284, 562 );
            this.Controls.Add( this.lblProcDump );
            this.Icon = ( ( System.Drawing.Icon )( resources.GetObject( "$this.Icon" ) ) );
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size( 300, 600 );
            this.MinimumSize = new System.Drawing.Size( 300, 600 );
            this.Name = "ProductDumpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Get Product Dump";
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Label lblProcDump;



    }
}