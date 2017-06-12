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

namespace Airstream.User_Interfaces
{
    //private string projectFolder = "..\\..\\..\\";
    public partial class UI_Default : Form
    {
        public UI_Default()
        {
            InitializeComponent();
            UI_General.SetGeneralElements(this);
            Button _interestingFactsBtn = new Button();
            Button _whatsInsideBtn = new Button();
            Button _leaveSomeFeedbackBtn = new Button();

            _interestingFactsBtn.Size = new Size(200, 200);
            _interestingFactsBtn.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.65), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.45));

            _whatsInsideBtn.Size = new Size(200, 200);
            _whatsInsideBtn.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.15), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.45));

            _leaveSomeFeedbackBtn.Size = new Size(200, 200);
            _leaveSomeFeedbackBtn.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.40), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.45));

            _interestingFactsBtn.BackColor = Color.FromArgb(255, 255, 255);
            _interestingFactsBtn.ForeColor = UI_General.GetLabelGrayForeColor();

            _whatsInsideBtn.BackColor = Color.FromArgb(255, 255, 255);
            _whatsInsideBtn.ForeColor = UI_General.GetLabelGrayForeColor();


            _leaveSomeFeedbackBtn.BackColor = Color.FromArgb(255, 255, 255);
            _leaveSomeFeedbackBtn.ForeColor = UI_General.GetLabelGrayForeColor();


            _interestingFactsBtn.Text = "Did you know?";
            _interestingFactsBtn.TextAlign = ContentAlignment.MiddleCenter;

            _whatsInsideBtn.Text = "What's inside?";
            _whatsInsideBtn.TextAlign = ContentAlignment.MiddleCenter;

            _leaveSomeFeedbackBtn.Text = "Leave some feedback!";
            _leaveSomeFeedbackBtn.TextAlign = ContentAlignment.MiddleCenter;
            _leaveSomeFeedbackBtn.Click += _leaveSomeFeedbackBtn_Click;


            Controls.Add(_interestingFactsBtn);
            Controls.Add(_whatsInsideBtn);
            Controls.Add(_leaveSomeFeedbackBtn);

            _interestingFactsBtn.BringToFront();
            _whatsInsideBtn.BringToFront();
            _leaveSomeFeedbackBtn.BringToFront();


        }

        private void _leaveSomeFeedbackBtn_Click(object sender, EventArgs e)
        {
            List<Question> questions = QuestionLogic.CreateTheFeedbackQuestions();
            Voter TestVoter = new Voter(12345);
            UI_Feedback newFeedbackForm = new UI_Feedback(questions, TestVoter);
            newFeedbackForm.ShowDialog();
        }
    }
}
