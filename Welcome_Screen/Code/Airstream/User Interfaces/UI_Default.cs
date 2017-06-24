using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Airstream.Feedback.QuestionsAndAnswers;
using Airstream.Feedback.Q_and_A_Logic;
using Airstream.Feedback.Database;
using Airstream.Feedback.Voters;
using System.Diagnostics;
using Airstream.User_Interfaces;


namespace Airstream.User_Interfaces
{
    public partial class UI_Default : Form
    {
        static Button _didYouKnowBtn;
        static Button _whatsInsideBtn;
        static Button _leaveSomeFeedbackBtn;
        static string projectFolder;
        static Label _didYouKnowLabel;
        static Label _whatsInsideLabel;
        static Label _leaveSomeFeedbackLabel;
        static Panel _didYouKnowPanel;
        static Panel _whatsInsidePanel;
        static Panel _leaveSomeFeedbackPanel;
        static Button _rfidScanImage;
        static Label _rfidScanLabel;



        public UI_Default()
        {
            InitializeComponent();
            UI_General.SetGeneralElements(this);
            SetUniqueElements();
            AddAndBringToFrontControls();
            AddEventHandlers();


        }

        public void AddEventHandlers()
        {
            _leaveSomeFeedbackBtn.Click += _leaveSomeFeedbackBtn_Click;
            _didYouKnowBtn.Click += _didYouKnowBtn_Click;
            _whatsInsideBtn.Click += _whatsInsideBtn_Click;
        }

        public  void AddAndBringToFrontControls()
        {
            Controls.Add(_rfidScanImage);
            Controls.Add(_rfidScanLabel);

            _rfidScanImage.BringToFront();
            _rfidScanLabel.BringToFront();


        }

        public static void SetUniqueElements()
        {
            // Instantiate elements 
            _didYouKnowBtn = new Button();
            _whatsInsideBtn = new Button();
            _leaveSomeFeedbackBtn = new Button();
            projectFolder = UI_General.GetProjectFolder();
            _didYouKnowLabel = new Label();
            _whatsInsideLabel = new Label();
            _leaveSomeFeedbackLabel = new Label();
            _leaveSomeFeedbackPanel = new Panel();
            _whatsInsidePanel = new Panel();
            _didYouKnowPanel = new Panel();
            _rfidScanImage = new Button();
            _rfidScanLabel = new Label();


            UI_General._labelWhite.Controls.Add(_didYouKnowPanel);
            UI_General._labelWhite.Controls.Add(_whatsInsidePanel);
            UI_General._labelWhite.Controls.Add(_leaveSomeFeedbackPanel);

            double containerWidth = UI_General._labelWhite.Width;
            double containerHeight = UI_General._labelWhite.Height;

            _didYouKnowPanel.Size = new Size(200, 250);
            _didYouKnowPanel.BackColor = Color.White;
            _didYouKnowPanel.Top += 50;
            double spacing = (containerWidth - _didYouKnowPanel.Width * 3)/4;
            _didYouKnowPanel.Left += Convert.ToInt16(spacing);

            _whatsInsidePanel.Size = new Size(200, 250);
            _whatsInsidePanel.BackColor = Color.White;
            _whatsInsidePanel.Top += 50;
            _whatsInsidePanel.Left += Convert.ToInt16(spacing + _whatsInsidePanel.Width + spacing + _whatsInsidePanel.Width + spacing);

            _leaveSomeFeedbackPanel.Size = new Size(200, 250);
            _leaveSomeFeedbackPanel.BackColor = Color.White;
            _leaveSomeFeedbackPanel.Top += 50;
            _leaveSomeFeedbackPanel.Left += Convert.ToInt16(spacing + _leaveSomeFeedbackPanel.Width + spacing);


            //Set their properties
            _rfidScanImage.Size = new Size(100, 100);
            _rfidScanImage.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.04), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.80));
            _rfidScanImage.BackgroundImage = new Bitmap(projectFolder + @"Pictures\back.png");
            _rfidScanImage.BackColor = Color.Transparent;
            _rfidScanImage.BackgroundImageLayout = ImageLayout.Stretch;
            _rfidScanImage.FlatStyle = FlatStyle.Flat;
            _rfidScanImage.FlatAppearance.BorderSize = 0;
            _rfidScanImage.FlatAppearance.MouseOverBackColor = _rfidScanImage.BackColor;
            _rfidScanImage.FlatAppearance.MouseDownBackColor = _rfidScanImage.BackColor;

            _rfidScanLabel.Size = new Size(400,100);
            _rfidScanLabel.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.1), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.83));
            _rfidScanLabel.Text = "RFID Scannen";
            _rfidScanLabel.Font = new Font("Arial Bold", 32.0f);
            _rfidScanLabel.ForeColor = Color.White;
            _rfidScanLabel.BackColor = Color.Transparent;


            _didYouKnowBtn.Size = new Size(200, 200);
            _didYouKnowBtn.BackgroundImage = new Bitmap(projectFolder + @"Pictures\did_you_know.jpg");
            _didYouKnowBtn.BackgroundImageLayout = ImageLayout.Stretch;


            _didYouKnowPanel.Controls.Add(_didYouKnowBtn);
            _didYouKnowPanel.Controls.Add(_didYouKnowLabel);
            _didYouKnowBtn.Top += 25;
            _didYouKnowLabel.Text = "Did You know?";
            _didYouKnowLabel.Left += (_didYouKnowPanel.Width - _didYouKnowLabel.Width) / 2;


            _whatsInsideBtn.Size = new Size(200, 200);
            _whatsInsideBtn.BackgroundImage = new Bitmap(projectFolder + @"Pictures\whats_inside.jpg");
            _whatsInsideBtn.BackgroundImageLayout = ImageLayout.Stretch;

            _whatsInsidePanel.Controls.Add(_whatsInsideBtn);
            _whatsInsidePanel.Controls.Add(_whatsInsideLabel);
            _whatsInsideBtn.Top += 25;
            _whatsInsideLabel.Text = "What's Inside?";
            _whatsInsideLabel.Left += (_whatsInsidePanel.Width - _whatsInsideLabel.Width) / 2;


            _leaveSomeFeedbackBtn.Size = new Size(200, 200);
            _leaveSomeFeedbackBtn.BackgroundImage = new Bitmap(projectFolder + @"Pictures\FeedbackHQ.png");
            _leaveSomeFeedbackBtn.BackgroundImageLayout = ImageLayout.Stretch;
            _leaveSomeFeedbackPanel.Controls.Add(_leaveSomeFeedbackBtn);
            _leaveSomeFeedbackPanel.Controls.Add(_leaveSomeFeedbackLabel);
            _leaveSomeFeedbackBtn.Top += 25;
            _leaveSomeFeedbackLabel.Text = "Leave feedback!";
            _leaveSomeFeedbackLabel.Left += (_leaveSomeFeedbackPanel.Width - _leaveSomeFeedbackLabel.Width) / 2;


            _didYouKnowBtn.BackColor = Color.FromArgb(255, 255, 255);
            _didYouKnowBtn.ForeColor = UI_General.GetLabelGrayForeColor();

            _whatsInsideBtn.BackColor = Color.FromArgb(255, 255, 255);
            _whatsInsideBtn.ForeColor = UI_General.GetLabelGrayForeColor();


            _leaveSomeFeedbackBtn.BackColor = Color.FromArgb(255, 255, 255);
            _leaveSomeFeedbackBtn.ForeColor = UI_General.GetLabelGrayForeColor();




        }
        private static void _leaveSomeFeedbackBtn_Click(object sender, EventArgs e)
        {
            List<Question> questions = QuestionLogic.CreateTheFeedbackQuestions();
            Voter TestVoter = new Voter(12345);
            UI_Feedback newFeedbackForm = new UI_Feedback(questions, TestVoter);
            newFeedbackForm.ShowDialog();
        }

        private static void _didYouKnowBtn_Click(object sender, EventArgs e)
        {
            UI_DYK newFeedbackForm = new UI_DYK();
            newFeedbackForm.ShowDialog();


        }
        private static void _whatsInsideBtn_Click(object sender, EventArgs e)
        {
            UI_DYK newFeedbackForm = new UI_DYK();
            newFeedbackForm.ShowDialog();


        }

    }



}
