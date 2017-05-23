using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Airstream.Feedback.Statistics;
using Airstream.Feedback.QuestionsAndAnswers;
using Airstream.Feedback.Voters;

namespace Airstream
{
    public partial class UI_Feedback : Form
    {
        //--- Evaluation Elements

        private Label _labelFeedbackQuestion;
        private string _labelFeedbackQuestionText = SetLabelFeedbackQuestionText();

        private Label _labelFeedbackWhatOthersHadToSay;
        private string _labelFeedbackWhatOthersHadToSayText = "Was andere gesagt haben";

        private Label _labelBarGraphOption1;
        private Label _labelBarGraphOption2;
        private Label _labelBarGraphOption3;
        private Label _labelBarGraphOption4;
        private Label _labelBarGraphOption5;

        private Label _labelFeedbackFavorites;
        private string _labelFeedbackFavoritesText = "Favoriten";

        private PictureBox _pictureBoxFeedbackFavorites;

        private Label _labelFeedbackMoreInfo;
        private string _labelFeedbackMoreInfoText = "Mehr Informationen";

        private RichTextBox _textBoxFeedbackMoreInfo;
        private Font _textBoxFeedbackMoreInfoFont = new Font("Arial Bold", 13.5f);

        //--- Question and Answer Elements

        private Button _buttonOption0;
        private Button _buttonOption1;
        private Button _buttonOption2;
        private Button _buttonOption3;
        private Button _buttonOption4;

        private bool _choseOption = false;

        Answer answer = new Answer("default");

        public UI_Feedback()
        {
            InitializeComponent();

            Click += NextQuestion_Click;

            UI_General.SetGeneralElements(this);
            UI_General.GetLabelGray().Click += NextQuestion_Click;
            

            // START---FEEDBACK

            // AUSWERTUNGS-ELEMENTE
            _labelFeedbackMoreInfo = new Label();
            _labelFeedbackMoreInfo.BackColor = UI_General.GetLabelGrayBackColor();
            _labelFeedbackMoreInfo.ForeColor = UI_General.GetLabelGrayForeColor();
            _labelFeedbackMoreInfo.Font = UI_General.GetLabelGrayFont();
            _labelFeedbackMoreInfo.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.698), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.40));
            _labelFeedbackMoreInfo.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.16), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.04));
            _labelFeedbackMoreInfo.Text = _labelFeedbackMoreInfoText;

            Controls.Add(_labelFeedbackMoreInfo);

            _textBoxFeedbackMoreInfo = new RichTextBox();
            _textBoxFeedbackMoreInfo.BackColor = UI_General.GetLabelGrayBackColor();
            _textBoxFeedbackMoreInfo.Cursor = Cursors.No;
            _textBoxFeedbackMoreInfo.ForeColor = UI_General.GetLabelGrayForeColor();
            _textBoxFeedbackMoreInfo.Font = _textBoxFeedbackMoreInfoFont;
            _textBoxFeedbackMoreInfo.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.7), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.50));
            _textBoxFeedbackMoreInfo.ReadOnly = true;
            _textBoxFeedbackMoreInfo.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.278));
            _textBoxFeedbackMoreInfo.Text = SetTextBoxFeedbackMoreInfoText();

            Controls.Add(_textBoxFeedbackMoreInfo);

            _labelFeedbackFavorites = new Label();
            _labelFeedbackFavorites.BackColor = UI_General.GetLabelGrayBackColor();
            _labelFeedbackFavorites.ForeColor = UI_General.GetLabelGrayForeColor();
            _labelFeedbackFavorites.Font = UI_General.GetLabelGrayFont();
            _labelFeedbackFavorites.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.45), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.40));
            _labelFeedbackFavorites.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.04));
            _labelFeedbackFavorites.Text = _labelFeedbackFavoritesText;

            Controls.Add(_labelFeedbackFavorites);

            _pictureBoxFeedbackFavorites = new PictureBox();
            _pictureBoxFeedbackFavorites.BackColor = UI_General.GetLabelGrayBackColor();
            _pictureBoxFeedbackFavorites.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.4), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.50));
            _pictureBoxFeedbackFavorites.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.156), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.278));

            PieChart A = new PieChart();
            List<Tuple<string, float>> B = new List<Tuple<string, float>>();

            B.Add(new Tuple<string, float>("A", 12));
            B.Add(new Tuple<string, float>("A", 2));
            B.Add(new Tuple<string, float>("A", 6));
            B.Add(new Tuple<string, float>("A", 9));
            B.Add(new Tuple<string, float>("A", 7));

            _pictureBoxFeedbackFavorites.BackgroundImage = A.DrawPieChart(B);

            Controls.Add(_pictureBoxFeedbackFavorites);

            _labelFeedbackWhatOthersHadToSay = new Label();
            _labelFeedbackWhatOthersHadToSay.BackColor = UI_General.GetLabelGrayBackColor();
            _labelFeedbackWhatOthersHadToSay.ForeColor = UI_General.GetLabelGrayForeColor();
            _labelFeedbackWhatOthersHadToSay.Font = UI_General.GetLabelGrayFont();
            _labelFeedbackWhatOthersHadToSay.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.40));
            _labelFeedbackWhatOthersHadToSay.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.8), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.04));
            _labelFeedbackWhatOthersHadToSay.Text = _labelFeedbackWhatOthersHadToSayText;

            Controls.Add(_labelFeedbackWhatOthersHadToSay);

            _labelBarGraphOption1 = new Label();
            _labelBarGraphOption1.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption1.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.51));
            _labelBarGraphOption1.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.16), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));

            Controls.Add(_labelBarGraphOption1);

            _labelBarGraphOption2 = new Label();
            _labelBarGraphOption2.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption2.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.56));
            _labelBarGraphOption2.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.016), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));

            Controls.Add(_labelBarGraphOption2);

            _labelBarGraphOption3 = new Label();
            _labelBarGraphOption3.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption3.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.61));
            _labelBarGraphOption3.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.076), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));

            Controls.Add(_labelBarGraphOption3);

            _labelBarGraphOption4 = new Label();
            _labelBarGraphOption4.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption4.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.66));
            _labelBarGraphOption4.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.122), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));

            Controls.Add(_labelBarGraphOption4);

            _labelBarGraphOption5 = new Label();
            _labelBarGraphOption5.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption5.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.71));
            _labelBarGraphOption5.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.092), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));

            Controls.Add(_labelBarGraphOption5);

            _labelFeedbackQuestion = new Label();
            _labelFeedbackQuestion.BackColor = UI_General.GetLabelGrayBackColor();
            _labelFeedbackQuestion.ForeColor = UI_General.GetLabelGrayForeColor();
            _labelFeedbackQuestion.Font = UI_General.GetScreenDefaultFont();
            _labelFeedbackQuestion.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.09), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.28));
            _labelFeedbackQuestion.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.8), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.14));
            _labelFeedbackQuestion.Text = _labelFeedbackQuestionText;

            Controls.Add(_labelFeedbackQuestion);

            
            _labelBarGraphOption1.BringToFront();
            _labelBarGraphOption2.BringToFront();
            _labelBarGraphOption3.BringToFront();
            _labelBarGraphOption4.BringToFront();
            _labelBarGraphOption5.BringToFront();
            _labelFeedbackQuestion.BringToFront();
            _labelFeedbackWhatOthersHadToSay.BringToFront();
            _pictureBoxFeedbackFavorites.BringToFront();
            _labelFeedbackFavorites.BringToFront();
            _textBoxFeedbackMoreInfo.BringToFront();
            _labelFeedbackMoreInfo.BringToFront();

            ShowStatistics.getUsedColors().Clear();

            // AUSWERTUNGS-ELEMENTE --- ENDE

            // QUESTION & ANSWER - ELEMENTS
            HideEvaluationElements();

            _buttonOption0 = new Button();
            _buttonOption0.BackColor = UI_General.GetLabelGrayBackColor();
            _buttonOption0.Click += Option_Click;
            _buttonOption0.ForeColor = UI_General.GetLabelGrayForeColor();
            _buttonOption0.Font = UI_General.GetLabelGrayFont();
            _buttonOption0.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.40));
            _buttonOption0.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.06));
            _buttonOption0.Text = "Option 1";

            Controls.Add(_buttonOption0);

            _buttonOption1 = new Button();
            _buttonOption1.BackColor = UI_General.GetLabelGrayBackColor();
            _buttonOption1.Click += Option_Click;
            _buttonOption1.ForeColor = UI_General.GetLabelGrayForeColor();
            _buttonOption1.Font = UI_General.GetLabelGrayFont();
            _buttonOption1.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.48));
            _buttonOption1.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.06));
            _buttonOption1.Text = "Option 2";

            Controls.Add(_buttonOption1);

            _buttonOption2 = new Button();
            _buttonOption2.BackColor = UI_General.GetLabelGrayBackColor();
            _buttonOption2.Click += Option_Click;
            _buttonOption2.ForeColor = UI_General.GetLabelGrayForeColor();
            _buttonOption2.Font = UI_General.GetLabelGrayFont();
            _buttonOption2.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.56));
            _buttonOption2.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.06));
            _buttonOption2.Text = "Option 3";

            Controls.Add(_buttonOption2);

            _buttonOption3 = new Button();
            _buttonOption3.BackColor = UI_General.GetLabelGrayBackColor();
            _buttonOption3.Click += Option_Click;
            _buttonOption3.ForeColor = UI_General.GetLabelGrayForeColor();
            _buttonOption3.Font = UI_General.GetLabelGrayFont();
            _buttonOption3.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.64));
            _buttonOption3.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.06));
            _buttonOption3.Text = "Option 4";

            Controls.Add(_buttonOption3);

            _buttonOption4 = new Button();
            _buttonOption4.BackColor = UI_General.GetLabelGrayBackColor();
            _buttonOption4.Click += Option_Click;
            _buttonOption4.ForeColor = UI_General.GetLabelGrayForeColor();
            _buttonOption4.Font = UI_General.GetLabelGrayFont();
            _buttonOption4.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.72));
            _buttonOption4.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.06));
            _buttonOption4.Text = "Option 5";

            Controls.Add(_buttonOption4);

            _buttonOption0.BringToFront();
            _buttonOption1.BringToFront();
            _buttonOption2.BringToFront();
            _buttonOption3.BringToFront();
            _buttonOption4.BringToFront();

            StartFeedback();
        }

        private static string SetLabelFeedbackQuestionText()
        {
            return "HIER KÖNNTE IHRE FRAGE STEHEN? ;)";
        }

        private static string SetTextBoxFeedbackMoreInfoText()
        {
            return "Hier noch mehr Interessantes:\n\n35% der Wähler denken das X das Beste hier im Airstream ist.\n\n25% denken Y ist das interessanteste und\n\n21% bevorzugen Z.\n\nVielen Dank für Ihr Feedback. Es hilft uns dabei uns und unsere Arbeit hier im Airstream zu verbessern.";
        }

        private void HideEvaluationElements()
        {
            _labelFeedbackMoreInfo.Visible = false;
            _textBoxFeedbackMoreInfo.Visible = false;
            _labelFeedbackFavorites.Visible = false;
            _pictureBoxFeedbackFavorites.Visible = false;
            _labelFeedbackWhatOthersHadToSay.Visible = false;
            _labelBarGraphOption1.Visible = false;
            _labelBarGraphOption2.Visible = false;
            _labelBarGraphOption3.Visible = false;
            _labelBarGraphOption4.Visible = false;
            _labelBarGraphOption5.Visible = false;
            //_labelFeedbackQuestion.Visible = false;
        }

        private void ShowEvaluationElements()
        {
            _labelFeedbackMoreInfo.Visible = true;
            _textBoxFeedbackMoreInfo.Visible = true;
            _labelFeedbackFavorites.Visible = true;
            _pictureBoxFeedbackFavorites.Visible = true;
            _labelFeedbackWhatOthersHadToSay.Visible = true;
            _labelBarGraphOption1.Visible = true;
            _labelBarGraphOption2.Visible = true;
            _labelBarGraphOption3.Visible = true;
            _labelBarGraphOption4.Visible = true;
            _labelBarGraphOption5.Visible = true;
            _labelFeedbackQuestion.Visible = true;
        }

        private void HideQandAElements()
        {
            _buttonOption0.Visible = false;
            _buttonOption1.Visible = false;
            _buttonOption2.Visible = false;
            _buttonOption3.Visible = false;
            _buttonOption4.Visible = false;
        }

        private void ShowQandAElements()
        {
            _buttonOption0.Visible = true;
            _buttonOption1.Visible = true;
            _buttonOption2.Visible = true;
            _buttonOption3.Visible = true;
            _buttonOption4.Visible = true;
        }

        public void StartFeedback()
        {
            foreach (Question q in Question.GetAllQuestion())
            {
                if (_choseOption == false)
                {
                    _labelFeedbackQuestion.Text = q.question;
                    _buttonOption0.Text = q.options[0].text;
                    _buttonOption1.Text = q.options[1].text;
                    _buttonOption2.Text = q.options[2].text;
                    _buttonOption3.Text = q.options[3].text;
                    _buttonOption4.Text = q.options[4].text;
                }
            }
        }

        private void Option_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            foreach(Answer a in Answer.GetAllAnswers())
            {
                if (clickedButton.Text == a.text)
                {
                    answer = a;
                    Voter.GetAllVoters()[0].AddGivenAnswer(Voter.GetAllVoters()[0], a);

                    //MessageBox.Show("Votes für '" + clickedButton.Text + "': " + answer.countVotes);

                    ShowEvaluation();
                }
            }
        }

        private void NextQuestion_Click(object sender, EventArgs e)
        {
            if (_labelFeedbackMoreInfo.Visible == true)
            {
                ShowQandAElements();
                HideEvaluationElements();
            }
        }

        public void ShowEvaluation()
        {
            HideQandAElements();
            ShowEvaluationElements();
        }
    }
}
