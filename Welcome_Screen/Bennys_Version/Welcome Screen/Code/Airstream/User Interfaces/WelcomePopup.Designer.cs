using System.Windows.Forms;
using System.Drawing;
using System;

namespace Airstream.User_Interfaces
{
    partial class WelcomePopup
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Label _labelName;
        private PictureBox _companyLogo;
        private Label _labelOccupation;
        string projectFolder = UI_General.GetProjectFolder();


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

            

            this._labelName = new Label();
            this._companyLogo = new PictureBox();
            this._labelOccupation = new Label();

            // Main Settings for the Welcome Popup
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Size = new Size(Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.6), Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.75));
            this.Controls.Add(this._labelName);
            this.Controls.Add(this._companyLogo);
            this.Controls.Add(this._labelOccupation);
            this.ResumeLayout(false);
            this.BringToFront();
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;


            //PictureBox for the company logo or the default image (in case the rfid tag is not registered or image is missing)
            this._companyLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this._companyLogo.Size = new Size(600, 300);
            this._companyLogo.Location = new Point((this.ClientSize.Width / 2) - (this._companyLogo.Width / 2), 10);
            this._companyLogo.Image = new Bitmap(projectFolder + @"Pictures\default_welcome_image.png");


            // Label for Name of the customer. If RFID Chip is not registered, there is a Welcome Text instead of the name

            this._labelName.AutoSize = true;
            this._labelName.MinimumSize = new Size(600, 0);
            this._labelName.Location = new Point((this.ClientSize.Width / 2) - (this._labelName.Width / 2), this._companyLogo.Size.Height + 30);
            this._labelName.Text = "Herzlich Willkommen";
            this._labelName.TextAlign = ContentAlignment.MiddleCenter;
            this._labelName.Font = new Font("Serif", 24);


            // Label for Occupation. If RFID Chip is not registered, there is a hint instead of Occupation

            this._labelOccupation.AutoSize = true;
            this._labelOccupation.MinimumSize = new Size(600, 0);
            this._labelOccupation.Location = new Point((this.ClientSize.Width / 2) - (this._labelOccupation.Width / 2), this._labelName.Location.Y + this._labelName.Height + 10);
            this._labelOccupation.Text = "Ihr RFID Chip ist noch nicht registriert";
            this._labelOccupation.TextAlign = ContentAlignment.MiddleCenter;
            this._labelOccupation.Font = new Font("Serif", 12);

        }

        #endregion
    }
}