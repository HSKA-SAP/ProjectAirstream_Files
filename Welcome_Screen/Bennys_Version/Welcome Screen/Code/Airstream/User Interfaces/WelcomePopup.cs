using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Airstream.Feedback.Database;

namespace Airstream.User_Interfaces
{
    public partial class WelcomePopup : Form
    {


        private string _rfidNumber;
        
        public WelcomePopup(string rfidNumber)
        {
            this._rfidNumber = rfidNumber;

            InitializeComponent();

            if (IsRfidRegistered())
            {
                PersonalizeWelcomePopup();
            }

            this.Show();


            Timer timerToClose = new Timer();
            timerToClose.Interval = 6000;
            timerToClose.Tick += new EventHandler(timer_Tick);
            timerToClose.Start();

        }

        private void PersonalizeWelcomePopup()
        {
            DatabaseConnection db = new DatabaseConnection();

            List<string>[] _userData = db.SelectFromUsers(_rfidNumber);

            //Personalize name, occupation and company

            string customer_name = _userData[0][0]; //Customer name can not be NULL in Database, therefore no check is needed

            string occupation = _userData[1].Count > 0 ? _userData[1][0] : "";
            string company = _userData[2].Count > 0 ? _userData[2][0] : "";

            
            this._labelName.Text = customer_name;
            this._labelOccupation.Text =
                occupation +
                ((!String.IsNullOrEmpty(company) && !String.IsNullOrEmpty(occupation)) ? ", " : "") +
                company;

            ChangeDefaultImage();

        }

        private void ChangeDefaultImage()
        {
            //The companyLogos will be saved with the same name as the rfid number. For more graphic formats just extend the List
            List<string> _fileExtensions = new List<string>() { "png", "jpg", "jpeg", "img", "gif" };
            
            foreach (string imgFormat in _fileExtensions)
            {
                try
                {
                    this._companyLogo.Image = new Bitmap(projectFolder + @"Pictures\" + _rfidNumber + "." + imgFormat);
                    break;
                }
                catch
                {

                }
            }

        }

        private bool IsRfidRegistered()
        {
            DatabaseConnection db = new DatabaseConnection();

            if (db.Count(String.Format("SELECT COUNT(*) FROM rfid_storage WHERE rfid_number = '{0}';", _rfidNumber)) > 0)
            {
                return true;
            }
            else
                return false;
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            UI_Default.setRfidScanImage("rfid_scan.png");
            this.Close();
        }

    }
}
