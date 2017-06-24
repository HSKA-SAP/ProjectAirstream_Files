using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Airstream.Feedback.Statistics;
using Airstream.Feedback.QuestionsAndAnswers;
using Airstream.Feedback.Q_and_A_Logic;
using Airstream.Feedback.Database;
using Airstream.Feedback.Voters;
using System.Diagnostics;


namespace Airstream.User_Interfaces
{
    public partial class UI_DYK : Form
    {
        private Label _labelFeedbackQuestion;
        private Label _labelFeedbackFavorites;
        private Label _labelFeedbackMoreInfo;


        private string _labelFeedbackQuestionText;
        private string projectFolder = UI_General.GetProjectFolder();

        private int _currentQuestion = 0;
        private int _localOptionClick = 0;
        private int _feedbackSummaryCnter = 0;
        private int _feedbackID = 0;

        private Font _labelFeedbackQuestionFont = new Font("Arial Bold", 23.0f);

        private Color _labelFeedbackQuestionForeColor = Color.FromArgb(255, 255, 255);
        private Color _buttonOptionBackColor = Color.FromArgb(255, 255, 255);


        private PictureBox _pictureBoxFeedbackFavorites;
        private PictureBox _backBtnFeedback;
        private PictureBox _fwdBtnFeedback;

        private Button _buttonOption0;
        private Button _buttonOption1;
        private Button _buttonOption2;
        private Button _buttonOption3;
        private Button _buttonOption4;

        public UI_DYK()
        {
            InitializeComponent();
            UI_General.SetGeneralElements(this);
           
        }
    }
}
