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
using Airstream.Feedback.Q_and_A_Logic;
using Airstream.Feedback.Database;
using Airstream.Feedback.Voters;
using System.Diagnostics;
using System.Windows.Media.Effects;

namespace Airstream
{
    public partial class UI_Feedback : Form
    {
        //--- Evaluation Elements
        #region
        private Label _labelFeedbackQuestion;
        private string _labelFeedbackQuestionText = SetLabelFeedbackQuestionText();
        private Font _labelFeedbackQuestionFont = new Font("Arial Bold", 23.0f);
        private Color _labelFeedbackQuestionForeColor = Color.FromArgb(255, 255, 255);

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

        private Color _buttonOptionBackColor = Color.FromArgb(255, 255, 255);
        private  List<Question> _questions;
        private  int _currentQuestion = 0;
        private int _feedbackID = 0;
        private string projectFolder = "..\\..\\..\\";
        PictureBox _backBtnFeedback;
        PictureBox _fwdBtnFeedback;
        #endregion

        public UI_Feedback(List<Question> questions)
        {
            InitializeComponent();

            Click += NextFeedback_Click;

            UI_General.SetGeneralElements(this);
            UI_General.GetLabelGray().Click += NextFeedback_Click;
            _questions = questions;
            UpdateFeedbackID();
            // START---FEEDBACK

#region
            // AUSWERTUNGS-ELEMENTE
            _labelFeedbackMoreInfo = new Label();
            _labelFeedbackMoreInfo.BackColor = UI_General.GetLabelGrayBackColor();
            _labelFeedbackMoreInfo.Click += NextFeedback_Click;
            _labelFeedbackMoreInfo.ForeColor = UI_General.GetLabelGrayForeColor();
            _labelFeedbackMoreInfo.Font = UI_General.GetLabelGrayFont();
            _labelFeedbackMoreInfo.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.698), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.40));
            _labelFeedbackMoreInfo.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.16), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.04));
            _labelFeedbackMoreInfo.Text = _labelFeedbackMoreInfoText;
            _labelFeedbackMoreInfo.Visible = false;
            Controls.Add(_labelFeedbackMoreInfo);

            _textBoxFeedbackMoreInfo = new RichTextBox();
            _textBoxFeedbackMoreInfo.BackColor = UI_General.GetLabelGrayBackColor();
            _textBoxFeedbackMoreInfo.Click += NextFeedback_Click;
            _textBoxFeedbackMoreInfo.Cursor = Cursors.No;
            _textBoxFeedbackMoreInfo.ForeColor = UI_General.GetLabelGrayForeColor();
            _textBoxFeedbackMoreInfo.Font = _textBoxFeedbackMoreInfoFont;
            _textBoxFeedbackMoreInfo.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.7), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.50));
            _textBoxFeedbackMoreInfo.ReadOnly = true;
            _textBoxFeedbackMoreInfo.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.278));
            _textBoxFeedbackMoreInfo.Text = SetTextBoxFeedbackMoreInfoText();
            _textBoxFeedbackMoreInfo.Visible = false;

            Controls.Add(_textBoxFeedbackMoreInfo);

            _labelFeedbackFavorites = new Label();
            _labelFeedbackFavorites.BackColor = UI_General.GetLabelGrayBackColor();
            _labelFeedbackFavorites.Click += NextFeedback_Click;
            _labelFeedbackFavorites.ForeColor = UI_General.GetLabelGrayForeColor();
            _labelFeedbackFavorites.Font = UI_General.GetLabelGrayFont();
            _labelFeedbackFavorites.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.45), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.40));
            _labelFeedbackFavorites.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.04));
            _labelFeedbackFavorites.Text = _labelFeedbackFavoritesText;
            _labelFeedbackFavorites.Visible = false;

            Controls.Add(_labelFeedbackFavorites);

            _pictureBoxFeedbackFavorites = new PictureBox();
            _pictureBoxFeedbackFavorites.BackColor = UI_General.GetLabelGrayBackColor();
            _pictureBoxFeedbackFavorites.Click += Favourites_Click;
            _pictureBoxFeedbackFavorites.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.4), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.50));
            _pictureBoxFeedbackFavorites.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.156), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.278));
            _pictureBoxFeedbackFavorites.Visible = false;

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
            _labelFeedbackWhatOthersHadToSay.Click += NextFeedback_Click;
            _labelFeedbackWhatOthersHadToSay.ForeColor = UI_General.GetLabelGrayForeColor();
            _labelFeedbackWhatOthersHadToSay.Font = UI_General.GetLabelGrayFont();
            _labelFeedbackWhatOthersHadToSay.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.40));
            _labelFeedbackWhatOthersHadToSay.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.8), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.04));
            _labelFeedbackWhatOthersHadToSay.Text = _labelFeedbackWhatOthersHadToSayText;
            _labelFeedbackWhatOthersHadToSay.Visible = false;

            Controls.Add(_labelFeedbackWhatOthersHadToSay);

            _labelBarGraphOption1 = new Label();
            _labelBarGraphOption1.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption1.Click += NextFeedback_Click;
            _labelBarGraphOption1.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.51));
            _labelBarGraphOption1.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.16), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption1.Visible = false;

            Controls.Add(_labelBarGraphOption1);

            _labelBarGraphOption2 = new Label();
            _labelBarGraphOption2.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption2.Click += NextFeedback_Click;
            _labelBarGraphOption2.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.56));
            _labelBarGraphOption2.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.016), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption2.Visible = false;

            Controls.Add(_labelBarGraphOption2);

            _labelBarGraphOption3 = new Label();
            _labelBarGraphOption3.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption3.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.61));
            _labelBarGraphOption3.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.076), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption3.Visible = false;

            Controls.Add(_labelBarGraphOption3);

            _labelBarGraphOption4 = new Label();
            _labelBarGraphOption4.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption4.Click += NextFeedback_Click;
            _labelBarGraphOption4.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.66));
            _labelBarGraphOption4.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.122), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption4.Visible = false;

            Controls.Add(_labelBarGraphOption4);

            _labelBarGraphOption5 = new Label();
            _labelBarGraphOption5.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption5.Click += NextFeedback_Click;
            _labelBarGraphOption5.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.71));
            _labelBarGraphOption5.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.092), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption5.Visible = false;

            Controls.Add(_labelBarGraphOption5);

            _backBtnFeedback = new PictureBox();
            _backBtnFeedback.BackColor = Color.Transparent;
            _backBtnFeedback.BackgroundImage = new Bitmap(projectFolder + @"Pictures\back.png");
            _backBtnFeedback.BackgroundImageLayout = ImageLayout.Stretch;
            _backBtnFeedback.Size = new Size(100, 100);
            _backBtnFeedback.Top += 115;
            _backBtnFeedback.Click += backBtn_Click;
            _fwdBtnFeedback = new PictureBox();
            _fwdBtnFeedback.BackColor = Color.Transparent;
            _fwdBtnFeedback.BackgroundImage = new Bitmap(projectFolder + @"Pictures\forward.png");
            _fwdBtnFeedback.BackgroundImageLayout = ImageLayout.Stretch;
            _fwdBtnFeedback.Size = new Size(100, 100);
            _fwdBtnFeedback.Top += 115;
            _fwdBtnFeedback.Left += 1150;
            _fwdBtnFeedback.Click += fwdBtn_Click;
            _fwdBtnFeedback.Visible = true;
            _backBtnFeedback.Visible = true;

            Controls.Add(_backBtnFeedback);
            Controls.Add(_fwdBtnFeedback);

            // Feedback Question element
            _labelFeedbackQuestion = new Label();
            _labelFeedbackQuestion.Click += NextFeedback_Click;
            _labelFeedbackQuestion.Font = _labelFeedbackQuestionFont;
            _labelFeedbackQuestion.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.75), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.14));
            _labelFeedbackQuestion.Text = _labelFeedbackQuestionText;
            _labelFeedbackQuestion.TextAlign = ContentAlignment.MiddleCenter;
            _labelFeedbackQuestion.Top += Convert.ToInt32(UI_General.GetSizeScreen().Height / 2) - 150;
            _labelFeedbackQuestion.Left += 150;

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
            _buttonOption0.BackColor = _buttonOptionBackColor;
            _buttonOption0.Click += Option_Click;
            _buttonOption0.ForeColor = UI_General.GetLabelGrayForeColor();

            _buttonOption0.Font = UI_General.GetLabelGrayFont();
            _buttonOption0.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.06), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.44));
            _buttonOption0.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.135), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.24));
            _buttonOption0.Text = "Option 1";
            _buttonOption0.TextAlign = ContentAlignment.TopCenter;

            Controls.Add(_buttonOption0);

            _buttonOption1 = new Button();
            _buttonOption1.BackColor = _buttonOptionBackColor;
            _buttonOption1.Click += Option_Click;
            _buttonOption1.ForeColor = UI_General.GetLabelGrayForeColor();
            _buttonOption1.Font = UI_General.GetLabelGrayFont();
            _buttonOption1.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.244), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.44));
            _buttonOption1.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.135), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.24));
            _buttonOption1.Text = "Option 2";
            _buttonOption1.TextAlign = ContentAlignment.TopCenter;

            Controls.Add(_buttonOption1);

            _buttonOption2 = new Button();
            _buttonOption2.BackColor = _buttonOptionBackColor;
            _buttonOption2.Click += Option_Click;
            _buttonOption2.ForeColor = UI_General.GetLabelGrayForeColor();
            _buttonOption2.Font = UI_General.GetLabelGrayFont();
            _buttonOption2.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.428), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.44));
            _buttonOption2.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.135), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.24));
            _buttonOption2.Text = "Option 3";
            _buttonOption2.TextAlign = ContentAlignment.TopCenter;

            Controls.Add(_buttonOption2);

            _buttonOption3 = new Button();
            _buttonOption3.BackColor = _buttonOptionBackColor;
            _buttonOption3.Click += Option_Click;
            _buttonOption3.ForeColor = UI_General.GetLabelGrayForeColor();
            _buttonOption3.Font = UI_General.GetLabelGrayFont();
            _buttonOption3.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.612), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.44));
            _buttonOption3.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.135), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.24));
            _buttonOption3.Text = "Option 4";
            _buttonOption3.TextAlign = ContentAlignment.TopCenter;

            Controls.Add(_buttonOption3);

            _buttonOption4 = new Button();
            _buttonOption4.BackColor = _buttonOptionBackColor;
            _buttonOption4.Click += Option_Click;
            _buttonOption4.ForeColor = UI_General.GetLabelGrayForeColor();
            _buttonOption4.Font = UI_General.GetLabelGrayFont();
            _buttonOption4.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.796), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.44));
            _buttonOption4.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.135), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.24));
            _buttonOption4.Text = "Option 5";
            _buttonOption4.TextAlign = ContentAlignment.TopCenter;

            Controls.Add(_buttonOption4);

            _buttonOption0.BringToFront();
            _buttonOption1.BringToFront();
            _buttonOption2.BringToFront();
            _buttonOption3.BringToFront();
            _buttonOption4.BringToFront();
#endregion
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
            UI_General.GetLabelGray().Visible = false;
            _labelFeedbackQuestion.BackColor = Color.Transparent;
            _labelFeedbackQuestion.ForeColor = _labelFeedbackQuestionForeColor;
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
            UI_General.GetLabelGray().Visible = true;
            _labelFeedbackQuestion.BackColor = UI_General.GetLabelGrayBackColor();
            _labelFeedbackQuestion.ForeColor = Color.FromArgb(45, 45, 45);
            HideQandAElements();
            _backBtnFeedback.Visible = false;
            _fwdBtnFeedback.Visible = false;
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
            //Will start the question phase 
            NextQuestion();
            //Currently in the evaluation screen


        }

        private void Option_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Answer answer;

            foreach(Answer a in Answer.GetAllAnswers())
            {
                if (clickedButton.Text == a.text)
                {
                    answer = a;
                    Voter.GetAllVoters()[0].AddGivenAnswer(Voter.GetAllVoters()[0], a);

                    //MessageBox.Show("Votes für '" + clickedButton.Text + "': " + answer.countVotes);

                    //if (_choseOptionQuestion1 == true)
                    //{
                    //    ShowEvaluation();
                    //}

                    //else
                    //{
                    //    //StartFeedback(0);
                    //}
                }
            }
            _currentQuestion++;

            Button answerGiven = (Button)sender;
            DatabaseConnection db = new DatabaseConnection();
            string q1 = "INSERT INTO feedback VALUES (";
            string q2 = _feedbackID.ToString() + "," + _currentQuestion.ToString() + ",'" + answerGiven.Text + "'";
            string q3 = ")";
            string q = q1 + q2 + q3;
            Debug.Print(q);
            db.InsertUpdateDelete(q);
            UpdateFeedbackID();
            NextQuestion();
        }

        private void Favourites_Click(object sender, EventArgs e)
        {
            ClearForFavourites();
            // Array to store percentages
            Dictionary<string, float> data = new Dictionary<string, float>();
            data = GetFavoritesData();

            PieChart A = new PieChart();
            List<Tuple<string, float>> B = new List<Tuple<string, float>>();

            B.Add(new Tuple<string, float>("Alexa", data["Alexa"]));
            B.Add(new Tuple<string, float>("NetAtmo", data["NetAtmo"]));
            B.Add(new Tuple<string, float>("Digital Boardroom", data["Digital Boardroom"]));
            B.Add(new Tuple<string, float>("55 Zoll-Tisch", data["55 Zoll-Tisch"]));
            B.Add(new Tuple<string, float>("Welcome-Screen", data["Welcome-Screen"]));
            Bitmap pieChart = A.DrawPieChart(B);
            
            _pictureBoxFeedbackFavorites.BackgroundImage = pieChart;
            _pictureBoxFeedbackFavorites.Visible = true;

            _labelBarGraphOption1.Visible = true;
            _labelBarGraphOption2.Visible = true;
            _labelBarGraphOption3.Visible = true;
            _labelBarGraphOption4.Visible = true;
            _labelBarGraphOption5.Visible = true;

            _labelBarGraphOption1.Size = _labelBarGraphOption2.Size = _labelBarGraphOption3.Size = _labelBarGraphOption4.Size = _labelBarGraphOption5.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.122), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption1.TextAlign = _labelBarGraphOption2.TextAlign = _labelBarGraphOption3.TextAlign = _labelBarGraphOption4.TextAlign = _labelBarGraphOption5.TextAlign = ContentAlignment.MiddleRight;
            _labelBarGraphOption1.Text = "Alexa";
            _labelBarGraphOption2.Text = "NetAtmo";
            _labelBarGraphOption3.Text = "Digital Boardroom";
            _labelBarGraphOption4.Text = "55 Zoll-Tisch";
            _labelBarGraphOption5.Text = "Welcome-Screen";

            _textBoxFeedbackMoreInfo.Visible = true;

            string[] answersQuestion1 = { "Alexa", "NetAtmo", "Digital Boardroom", "55 Zoll-Tisch", "Welcome-Screen" };
            string[] votes = { data["Alexa"] * 100 + "%", data["NetAtmo"] * 100 + "%", data["Digital Boardroom"] * 100 + "%", data["55 Zoll-Tisch"] * 100 + "%", data["Welcome-Screen"] * 100 + "%" };
            String s = ""; //String.Format("{1,-10} {0,-18}\n\n", "Optionen", "Stimmen in %");

            for (int index = 0; index < answersQuestion1.Length; index++)
            {
                if (index < answersQuestion1.Length - 1)
                    s += String.Format("{1,-10} {0,-18:N0}\n\n", answersQuestion1[index], votes[index]);
                else
                    s += String.Format("{1,-10} {0,-18:N0}", answersQuestion1[index], votes[index]);
            }

            _textBoxFeedbackMoreInfo.Text = s;
        }

        public Dictionary<string, float> GetFavoritesData()
        {
            int[] pData = new int[5]; 
            string[] answers = new string[] { "Alexa", "NetAtmo", "Digital Boardroom", "55 Zoll-Tisch", "Welcome-Screen"};
            string defaultQuery;
            defaultQuery = @"SELECT Count(feedback.feedbackAnswer)
                             FROM feedback
                             WHERE feedback.feedbackAnswer =";
            DatabaseConnection db = new DatabaseConnection();

            int i = 0;
            float total = 0;
            foreach(string answer in answers)
            {
                string q = answer;
                string query = defaultQuery + " '" + q + "';";
                int numFavourites = db.Count(query);
                pData[i] = numFavourites;
                total += numFavourites;
                i += 1;
            }
            Dictionary<string, float> dictionary = new Dictionary<string, float>();
            dictionary.Add(answers[0], (pData[0] / total));
            dictionary.Add(answers[1], (pData[1] / total));
            dictionary.Add(answers[2], (pData[2] / total));
            dictionary.Add(answers[3], (pData[3] / total));
            dictionary.Add(answers[4], (pData[4] / total));

            return dictionary;
        }

        public void ClearForFavourites()
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
            _labelFeedbackQuestionText = "Lieblings-Features des Airstream";
            _labelFeedbackQuestion.Text = _labelFeedbackQuestionText;
        }
        public void UpdateFeedbackID()
        {
            DatabaseConnection db = new DatabaseConnection();
            string q = "SELECT * from feedback";
            List<string>[] result = db.SelectFromFeedback(q);
            int length = Convert.ToInt16((result[0].Count().ToString()));
            _feedbackID = length;
        }

        public  void NextQuestion()
        {
            if (_currentQuestion < _questions.Count)
            {
                _labelFeedbackQuestion.Text = _questions[_currentQuestion].question;
                if (_currentQuestion == 0)
                {
                    string projectFolder = "..\\..\\..\\";

                    _buttonOption0.Text = _questions[_currentQuestion].options[0].text;
                    _buttonOption0.BackgroundImage = new Bitmap(projectFolder+ @"Pictures\Compressed\alexalogodone.jpg");
                    _buttonOption0.BackgroundImageLayout = ImageLayout.Stretch;
                    _buttonOption1.Text = _questions[_currentQuestion].options[1].text;
                    _buttonOption1.BackgroundImage = new Bitmap(projectFolder + @"Pictures\Compressed\netatmologodone.jpg");
                    _buttonOption1.BackgroundImageLayout = ImageLayout.Stretch;
                    _buttonOption2.Text = _questions[_currentQuestion].options[2].text;
                    _buttonOption2.BackgroundImage = new Bitmap(projectFolder + @"Pictures\Compressed\touchtablelogodone.jpg");
                    _buttonOption2.BackgroundImageLayout = ImageLayout.Stretch;
                    _buttonOption3.Text = _questions[_currentQuestion].options[3].text;
                    _buttonOption3.BackgroundImage = new Bitmap(projectFolder + @"Pictures\Compressed\welcomescreenlogodone.jpg");
                    _buttonOption3.BackgroundImageLayout = ImageLayout.Stretch;
                    _buttonOption4.Text = _questions[_currentQuestion].options[4].text;
                    _buttonOption4.BackgroundImage = new Bitmap(projectFolder + @"Pictures\Compressed\digitalboardroomlogodone.jpg");
                    _buttonOption4.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    _buttonOption0.Text = _questions[_currentQuestion].options[0].text;
                    _buttonOption0.BackgroundImage = null;
                    _buttonOption1.Text = _questions[_currentQuestion].options[1].text;
                    _buttonOption1.BackgroundImage = null;
                    _buttonOption2.Text = _questions[_currentQuestion].options[2].text;
                    _buttonOption2.BackgroundImage = null;
                    _buttonOption3.Text = _questions[_currentQuestion].options[3].text;
                    _buttonOption3.BackgroundImage = null;
                    _buttonOption4.Text = _questions[_currentQuestion].options[4].text;
                    _buttonOption4.BackgroundImage = null;
                }
            }
            else if (_currentQuestion == _questions.Count)
            {
                ShowEvaluation();
            }
        }
        private void NextFeedback_Click(object sender, EventArgs e)
        {
            //if (_labelFeedbackMoreInfo.Visible == true)
            //{
            //    _choseOptionQuestion0 = false;
            //    _choseOptionQuestion1 = false;

            //    ShowQandAElements();
            //    HideEvaluationElements();
            //}
        }

        private void fwdBtn_Click(object sender, EventArgs e)
        {
            //if (_labelFeedbackMoreInfo.Visible == true)
            //{
            //    _choseOptionQuestion0 = false;
            //    _choseOptionQuestion1 = false;

            //    ShowQandAElements();
            //    HideEvaluationElements();
            //}
        }
        private void backBtn_Click(object sender, EventArgs e)
        {
            //if (_labelFeedbackMoreInfo.Visible == true)
            //{
            //    _choseOptionQuestion0 = false;
            //    _choseOptionQuestion1 = false;

            //    ShowQandAElements();
            //    HideEvaluationElements();
            //}
        }


        public void ShowEvaluation()
        {
            ShowEvaluationElements();
        }


    }
}
