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

namespace Airstream
{
    public partial class UI_Feedback : Form
    {
        // Instatiate all elements

        #region Instantiate all elements
        private Label _labelFeedbackQuestion;
        private Label _labelBarGraphOption1;
        private Label _labelBarGraphOption2;
        private Label _labelBarGraphOption3;
        private Label _labelBarGraphOption4;
        private Label _labelBarGraphOption5;
        private Label _labelFeedbackFavorites;
        private Label _labelFeedbackMoreInfo;
        private Label _labelForFeedbackPercentages;


        private string _labelFeedbackQuestionText;
        private string _labelFeedbackWhatOthersHadToSayText = "What other's had to say";
        private string _labelFeedbackFavoritesText = "Favoriten";
        private string _labelFeedbackMoreInfoText = "Mehr Informationen";
        private string projectFolder = UI_General.GetProjectFolder();

        private int _currentQuestion = 0;
        private int _localOptionClick = 0;
        private int _feedbackSummaryCnter = 0;
        private int _feedbackID = 0;

        private Font _labelFeedbackQuestionFont = new Font("Arial Bold", 23.0f);
        private Font _labelForFeedbackPercentagesFont = new Font("Arial Bold", 13.5f);


        private Color _labelFeedbackQuestionForeColor = Color.FromArgb(255, 255, 255);
        private Color _buttonOptionBackColor = Color.FromArgb(255, 255, 255);


        private PictureBox _pictureBoxFeedbackFavorites;
        private PictureBox _backBtnFeedback;
        private PictureBox _fwdBtnFeedback;
        private PictureBox _homeBtn;

        private Button _buttonOption0;
        private Button _buttonOption1;
        private Button _buttonOption2;
        private Button _buttonOption3;
        private Button _buttonOption4;

        private  List<Question> _questions;
        private Voter _currentVoter;
#endregion

        /// <summary>
        /// Constructor for the Feedback UI which takes in the list of questions as its only parameter
        /// </summary>
        /// <param name="questions"></param>

        public UI_Feedback(List<Question> questions, Voter currentVoter)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            //Create a blank form to work with 
            UI_General.SetGeneralElements(this);
            _questions = questions; //instantiate form with questions from UI_Feedback.cs
            _currentVoter = currentVoter;
            UpdateFeedbackID();

            // Create all labels/elements needed
          
            _labelForFeedbackPercentages = new Label();
            _labelForFeedbackPercentages.BackColor = UI_General.GetLabelGrayBackColor();
            _labelForFeedbackPercentages.Cursor = Cursors.No;
            _labelForFeedbackPercentages.ForeColor = UI_General.GetLabelGrayForeColor();
            _labelForFeedbackPercentages.Font = _labelForFeedbackPercentagesFont;
            _labelForFeedbackPercentages.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.63), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.50));
            _labelForFeedbackPercentages.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.278));
            _labelForFeedbackPercentages.Text = SetTextBoxFeedbackMoreInfoText();
            _labelForFeedbackPercentages.Visible = false;

            Controls.Add(_labelForFeedbackPercentages);

            _labelFeedbackFavorites = new Label();
            _labelFeedbackFavorites.BackColor = UI_General.GetLabelGrayBackColor();
            _labelFeedbackFavorites.ForeColor = UI_General.GetLabelGrayForeColor();
            _labelFeedbackFavorites.Font = UI_General.GetLabelGrayFont();
            _labelFeedbackFavorites.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.45), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.40));
            _labelFeedbackFavorites.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.2), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.04));
            _labelFeedbackFavorites.Text = _labelFeedbackFavoritesText;
            _labelFeedbackFavorites.Visible = false;

            Controls.Add(_labelFeedbackFavorites);

            _pictureBoxFeedbackFavorites = new PictureBox();
            _pictureBoxFeedbackFavorites.BackColor = UI_General.GetLabelGrayBackColor();
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

            _labelBarGraphOption1 = new Label();
            _labelBarGraphOption1.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption1.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.21), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.51));
            _labelBarGraphOption1.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.16), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption1.Visible = false;

            Controls.Add(_labelBarGraphOption1);

            _labelBarGraphOption2 = new Label();
            _labelBarGraphOption2.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption2.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.21), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.56));
            _labelBarGraphOption2.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.016), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption2.Visible = false;

            Controls.Add(_labelBarGraphOption2);

            _labelBarGraphOption3 = new Label();
            _labelBarGraphOption3.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption3.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.21), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.61));
            _labelBarGraphOption3.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.076), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption3.Visible = false;

            Controls.Add(_labelBarGraphOption3);

            _labelBarGraphOption4 = new Label();
            _labelBarGraphOption4.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption4.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.21), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.66));
            _labelBarGraphOption4.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.122), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption4.Visible = false;

            Controls.Add(_labelBarGraphOption4);

            _labelBarGraphOption5 = new Label();
            _labelBarGraphOption5.BackColor = ShowStatistics.DetermineColor();
            _labelBarGraphOption5.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.21), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.71));
            _labelBarGraphOption5.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.092), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption5.Visible = false;

            Controls.Add(_labelBarGraphOption5);


            _labelFeedbackMoreInfo = new Label();

            // Feedback Question element
            _labelFeedbackQuestion = new Label();
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
            _pictureBoxFeedbackFavorites.BringToFront();
            _labelFeedbackFavorites.BringToFront();
            _labelForFeedbackPercentages.BringToFront();
            _labelFeedbackMoreInfo.BringToFront();


            _backBtnFeedback = new PictureBox();
            _backBtnFeedback.BackColor = Color.White;
            _backBtnFeedback.BackgroundImage = new Bitmap(projectFolder + @"Pictures\back.png");
            _backBtnFeedback.BackgroundImageLayout = ImageLayout.Stretch;
            _backBtnFeedback.Size = new Size(100, 100);
            _backBtnFeedback.Top += Convert.ToInt16(UI_General.GetSizeScreen().Height * 0.5);
            _backBtnFeedback.Left += Convert.ToInt16(UI_General.GetSizeScreen().Width * 0.1);
            _backBtnFeedback.Click += backBtn_Click;
            _fwdBtnFeedback = new PictureBox();
            _fwdBtnFeedback.BackColor = Color.White;
            _fwdBtnFeedback.BackgroundImage = new Bitmap(projectFolder + @"Pictures\forward.png");
            _fwdBtnFeedback.BackgroundImageLayout = ImageLayout.Stretch;
            _fwdBtnFeedback.Size = new Size(100, 100);
            _fwdBtnFeedback.Top += Convert.ToInt16(UI_General.GetSizeScreen().Height * 0.5);
            _fwdBtnFeedback.Left += Convert.ToInt16(UI_General.GetSizeScreen().Width * 0.8);
            _fwdBtnFeedback.Click += fwdBtn_Click;
            _fwdBtnFeedback.Visible = false;
            _backBtnFeedback.Visible = false;

            Controls.Add(_backBtnFeedback);
            Controls.Add(_fwdBtnFeedback);


            _homeBtn = new PictureBox();
            _homeBtn.BackColor = Color.White;
            _homeBtn.BackgroundImage = new Bitmap(projectFolder + @"Pictures\home_icon.png");
            _homeBtn.Size = new Size(100, 100);
            _homeBtn.BackgroundImageLayout = ImageLayout.Stretch;
            _homeBtn.Click += _homeBtn_Click;
            UI_General._labelWhite.Controls.Add(_homeBtn);


            ShowStatistics.getUsedColors().Clear();

            // AUSWERTUNGS-ELEMENTE --- ENDE

            // QUESTION & ANSWER - ELEMENTS
            HideEvaluationElements();

            int screenWidth = Convert.ToInt32(UI_General.GetSizeScreen().Width);
            int screenHeight = Convert.ToInt32(UI_General.GetSizeScreen().Height);
            int buttonSize = 200;
            double spacing = (screenWidth - 5 * buttonSize) / 6;

            _buttonOption0  = new Button();
            _buttonOption1 = new Button();
            _buttonOption2 = new Button();
            _buttonOption3 = new Button();
            _buttonOption4 = new Button();

            _buttonOption0.Size = _buttonOption1.Size = _buttonOption2.Size = _buttonOption3.Size = _buttonOption4.Size = new Size(buttonSize, buttonSize);
            _buttonOption0.BackColor = _buttonOption1.BackColor = _buttonOption2.BackColor =  _buttonOption3.BackColor = _buttonOption4.BackColor =  _buttonOptionBackColor;
            _buttonOption0.Click += Option_Click;
            _buttonOption0.ForeColor = _buttonOption1.ForeColor = _buttonOption2.ForeColor = _buttonOption3.ForeColor = _buttonOption4.ForeColor =  UI_General.GetLabelGrayForeColor();
            _buttonOption0.Font = _buttonOption1.Font = _buttonOption2.Font = _buttonOption3.Font = _buttonOption4.Font =  UI_General.GetLabelGrayFont();
            _buttonOption0.Top  += 25 + Convert.ToInt16((screenHeight - _buttonOption0.Height) / 2);
            _buttonOption0.Left  += Convert.ToInt16(spacing);
            _buttonOption0.TextAlign = _buttonOption1.TextAlign = _buttonOption2.TextAlign = _buttonOption3.TextAlign = _buttonOption4.TextAlign = ContentAlignment.TopCenter;

            Controls.Add(_buttonOption0);

            _buttonOption1.Click += Option_Click;
            _buttonOption1.Top += 25 + Convert.ToInt16((screenHeight - _buttonOption0.Height) / 2);
            _buttonOption1.Left += Convert.ToInt16(2*spacing + buttonSize);

            Controls.Add(_buttonOption1);

            _buttonOption2.Click += Option_Click;
            _buttonOption2.Top += 25 + Convert.ToInt16((screenHeight - _buttonOption0.Height) / 2);
            _buttonOption2.Left += Convert.ToInt16(3 * spacing + 2*buttonSize);

            Controls.Add(_buttonOption2);

            _buttonOption3.Click += Option_Click;
            _buttonOption3.Top += 25 + Convert.ToInt16((screenHeight - _buttonOption0.Height) / 2);
            _buttonOption3.Left += Convert.ToInt16(4 * spacing + 3 * buttonSize);

            Controls.Add(_buttonOption3);

            _buttonOption4.Click += Option_Click;
            _buttonOption4.Top += 25 + Convert.ToInt16((screenHeight - _buttonOption0.Height) / 2);
            _buttonOption4.Left += Convert.ToInt16(5 * spacing + 4 * buttonSize);

            Controls.Add(_buttonOption4);

            _buttonOption0.BringToFront();
            _buttonOption1.BringToFront();
            _buttonOption2.BringToFront();
            _buttonOption3.BringToFront();
            _buttonOption4.BringToFront();

            _buttonOption0.Leave += LeaveOption;
            _buttonOption1.Leave += LeaveOption;
            _buttonOption2.Leave += LeaveOption;
            _buttonOption3.Leave += LeaveOption;
            _buttonOption4.Leave += LeaveOption;

            UnsetOptions();
            //Begin the Feedback
            StartFeedback();

        }

        private void _homeBtn_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }



        /// <summary>
        /// Start the feedback process by calling the NextQuestion Function to display the first question with _currentQuestion = 0
        /// </summary>
        public void StartFeedback()
        {
            DisplayQuestion();

        }

        public void UnsetOptions()
        {
            _buttonOption0.TabStop = false;
            _buttonOption1.TabStop = false;
            _buttonOption2.TabStop = false;
            _buttonOption3.TabStop = false;
            _buttonOption4.TabStop = false;

        }
        /// <summary>
        /// A function to loop through the questions until they are all answered
        /// </summary>
        public void DisplayQuestion()
        {
            //_buttonOption0.Text = _buttonOption1.Text = _buttonOption2.Text = _buttonOption3.Text = _buttonOption4.Text = "";
            #region loop through questions
            if (_currentQuestion < _questions.Count && _currentQuestion >= 0)
            {
                _labelFeedbackQuestion.Text = _questions[_currentQuestion].question;
                if (_currentQuestion == 0) //If it is the first question, display a different type of layout (images of different features in Airstream)
                {

                    _buttonOption0.Name = _questions[_currentQuestion].options[0].text;
                    _buttonOption0.BackgroundImage = new Bitmap(projectFolder + @"Pictures\Compressed\alexalogodone.jpg");
                    _buttonOption0.BackgroundImageLayout = ImageLayout.Stretch;
                    _buttonOption1.Name = _questions[_currentQuestion].options[1].text;
                    _buttonOption1.BackgroundImage = new Bitmap(projectFolder + @"Pictures\Compressed\netatmologodone.jpg");
                    _buttonOption1.BackgroundImageLayout = ImageLayout.Stretch;
                    _buttonOption2.Name = _questions[_currentQuestion].options[2].text;
                    _buttonOption2.BackgroundImage = new Bitmap(projectFolder + @"Pictures\Compressed\touchtablelogodone.jpg");
                    _buttonOption2.BackgroundImageLayout = ImageLayout.Stretch;
                    _buttonOption3.Name = _questions[_currentQuestion].options[3].text;
                    _buttonOption3.BackgroundImage = new Bitmap(projectFolder + @"Pictures\Compressed\welcomescreenlogodone.jpg");
                    _buttonOption3.BackgroundImageLayout = ImageLayout.Stretch;
                    _buttonOption4.Name = _questions[_currentQuestion].options[4].text;
                    _buttonOption4.BackgroundImage = new Bitmap(projectFolder + @"Pictures\Compressed\digitalboardroomlogodone.jpg");
                    _buttonOption4.BackgroundImageLayout = ImageLayout.Stretch;
                }
                //yes or no questions
                else if(_currentQuestion == 2 || _currentQuestion == 3 || _currentQuestion == 5)
                {
                    _buttonOption0.Visible = false;
                    _buttonOption2.Visible = false;
                    _buttonOption4.Visible = false;
                    _buttonOption1.Name = _questions[_currentQuestion].options[0].text;
                    _buttonOption3.Name = _questions[_currentQuestion].options[1].text;


                }
                else //If it is not the first question or a yes or no question, display the default question and answer layout
                {
                    _buttonOption0.Visible = _buttonOption1.Visible = _buttonOption2.Visible = _buttonOption3.Visible = _buttonOption4.Visible = true;
                    _buttonOption0.Name = _questions[_currentQuestion].options[0].text;
                    _buttonOption0.BackgroundImage = new Bitmap(projectFolder + @"Pictures\faces\reallyHappyHQ.png");
                    _buttonOption0.ForeColor = _buttonOption0.BackColor;
                    _buttonOption1.Name = _questions[_currentQuestion].options[1].text;
                    _buttonOption1.BackgroundImage = new Bitmap(projectFolder + @"Pictures\faces\happyHQ.png"); ;
                    _buttonOption2.Name = _questions[_currentQuestion].options[2].text;
                    _buttonOption2.BackgroundImage = new Bitmap(projectFolder + @"Pictures\faces\neutralHQ.png"); ;
                    _buttonOption3.Name = _questions[_currentQuestion].options[3].text;
                    _buttonOption3.BackgroundImage = new Bitmap(projectFolder + @"Pictures\faces\sadHQ.png"); ;
                    _buttonOption4.Name = _questions[_currentQuestion].options[4].text;
                    _buttonOption4.BackgroundImage = new Bitmap(projectFolder + @"Pictures\faces\cryingHQ.png");


                }
            }
            else if(_currentQuestion < 0)
            {
                //do nothing
            }
            else if (_currentQuestion == _questions.Count) //If neither of these are true then all questions have been answered
            {
                AddAnswersToDB();
                AddHomeScreenButton();
                ShowEvaluationForFavourites();
            }
            #endregion
        }

        public void AddHomeScreenButton()
        {
            UI_General._labelWhite.Height = Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.64);
            int containerHeight = UI_General._labelWhite.Height;
            int containerWidth = UI_General._labelWhite.Width;
            _homeBtn.Top += Convert.ToInt32(containerHeight - _homeBtn.Height);
            _homeBtn.Left += 10;


        }
        public void NextQuestion()
        {
            _currentQuestion++;
            _localOptionClick = 0;
            DisplayQuestion();
            this.ActiveControl = null;

        }

        public void PrevQuestion()
        {
            _currentQuestion--;
            _localOptionClick = 0;
            if(_currentQuestion <= 0)
            {
                _currentQuestion = 0;
            }
            Debug.Print(_currentQuestion.ToString());
            DisplayQuestion();
        }


        public void AddAnswersToDB()
        {
            DatabaseConnection db = new DatabaseConnection();
            for(int i =0; i < _currentVoter.givenAnswers.ToArray().Length; i++)
            {
                string q1 = "INSERT INTO feedback VALUES (";
                string q2 = _feedbackID.ToString() + "," + i.ToString() + ",'" + _currentVoter.givenAnswers[i].text + "'";
                string q3 = ")";
                string q = q1 + q2 + q3;

                db.InsertUpdateDelete(q);
                UpdateFeedbackID();
            }

        }

        public void ShowEvaluationForFavourites()
        {
            ClearScreenForFeedbackSummary();

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
            _labelFeedbackQuestion.Text = _questions[0].question;
            _labelFeedbackQuestion.Visible = true;

            _labelBarGraphOption1.Size = _labelBarGraphOption2.Size = _labelBarGraphOption3.Size = _labelBarGraphOption4.Size = _labelBarGraphOption5.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.122), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption1.TextAlign = _labelBarGraphOption2.TextAlign = _labelBarGraphOption3.TextAlign = _labelBarGraphOption4.TextAlign = _labelBarGraphOption5.TextAlign = ContentAlignment.MiddleRight;
            _labelBarGraphOption1.Text = "Alexa";
            _labelBarGraphOption2.Text = "NetAtmo";
            _labelBarGraphOption3.Text = "Digital Boardroom";
            _labelBarGraphOption4.Text = "55 Zoll-Tisch";
            _labelBarGraphOption5.Text = "Welcome-Screen";

            _labelForFeedbackPercentages.Visible = true;
 

            string[] answersQuestion1 = { "Alexa", "NetAtmo", "Digital Boardroom", "55 Zoll-Tisch", "Welcome-Screen" };
            string[] votes = { Convert.ToInt16(data["Alexa"] * 100) + "%", Convert.ToInt16(data["NetAtmo"] * 100) + "%", Convert.ToInt16(data["Digital Boardroom"] * 100) + "%", Convert.ToInt16(data["55 Zoll-Tisch"] * 100) + "%", Convert.ToInt16(data["Welcome-Screen"] * 100) + "%" };
            String s = ""; //String.Format("{1,-10} {0,-18}\n\n", "Optionen", "Stimmen in %");

            for (int index = 0; index < answersQuestion1.Length; index++)
            {
                if (index < answersQuestion1.Length - 1)
                    s += String.Format("{1,-10} {0,-18:N0}\n\n", answersQuestion1[index], votes[index]);
                else
                    s += String.Format("{1,-10} {0,-18:N0}", answersQuestion1[index], votes[index]);
            }

            _labelForFeedbackPercentages.Text = s;

        }

        private void ClearScreenForFeedbackSummary()
        {

            _labelFeedbackQuestion.Visible = false;
            UI_General.GetLabelGray().Visible = true;
            _labelFeedbackQuestion.BackColor = UI_General.GetLabelGrayBackColor();
            _labelFeedbackQuestion.ForeColor = Color.FromArgb(45, 45, 45);
            HideQandAElements();
            _fwdBtnFeedback.Visible = true;
            _backBtnFeedback.Visible = true;
            _fwdBtnFeedback.BringToFront();
            _backBtnFeedback.BringToFront();
            _homeBtn.BringToFront();
            _pictureBoxFeedbackFavorites.Visible = false;
            _labelBarGraphOption1.Visible = false;
            _labelBarGraphOption2.Visible = false;
            _labelBarGraphOption3.Visible = false;
            _labelBarGraphOption4.Visible = false;
            _labelBarGraphOption5.Visible = false;
            _labelForFeedbackPercentages.Visible = false;

        }


        private void HideEvaluationElements()
        {
            _labelFeedbackMoreInfo.Visible = false;
            _labelForFeedbackPercentages.Visible = false;
            _labelFeedbackFavorites.Visible = false;
            _pictureBoxFeedbackFavorites.Visible = false;
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

        private static string SetTextBoxFeedbackMoreInfoText()
        {
            return "Hier noch mehr Interessantes:\n\n35% der Wähler denken das X das Beste hier im Airstream ist.\n\n25% denken Y ist das interessanteste und\n\n21% bevorzugen Z.\n\nVielen Dank für Ihr Feedback. Es hilft uns dabei uns und unsere Arbeit hier im Airstream zu verbessern.";
        }


        public Dictionary<string, float> GetFavoritesData()
        {
            int[] pData = new int[5];
            string[] answers = new string[] { "Alexa", "NetAtmo", "Digital Boardroom", "55 Zoll-Tisch", "Welcome-Screen" };
            string defaultQuery;
            defaultQuery = @"SELECT Count(feedback.feedbackAnswer)
                             FROM feedback
                             WHERE feedback.feedbackAnswer =";
            DatabaseConnection db = new DatabaseConnection();

            int i = 0;
            float total = 0;
            foreach (string answer in answers)
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

        public Dictionary<string, float> GetFiveOptionQData(int fQuestion)
        {
            int[] pData = new int[5];
            string[] answers = new string[] { "Sehr gut", "Gut", "Nichts besonderes", "Schlecht", "Sehr schlecht" };
            string defaultQuery;
            defaultQuery = @"SELECT Count(feedback.feedbackAnswer)
                             FROM feedback
                             WHERE feedback.feedbackAnswer =";
            DatabaseConnection db = new DatabaseConnection();

            int i = 0;
            float total = 0;
            foreach (string answer in answers)
            {
                string q = answer;
                string query = defaultQuery + " '" + q + "'" + " AND feedback.feedbackQ ='" + fQuestion + "';";
                //Debug.Print(query.ToString());
                int count = db.Count(query);
                pData[i] = count;
                total += count;
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

        public Dictionary<string, float> GetTwoOptionQData(int fQuestion)
        {
            int[] pData = new int[2];
            string[] answers = new string[] { "Ja", "Nein" };
            string defaultQuery;
            defaultQuery = @"SELECT Count(feedback.feedbackAnswer)
                             FROM feedback
                             WHERE feedback.feedbackAnswer =";
            DatabaseConnection db = new DatabaseConnection();

            int i = 0;
            float total = 0;
            foreach (string answer in answers)
            {
                string q = answer;
                string query = defaultQuery + " '" + q + "'" + " AND feedback.feedbackQ ='" + fQuestion + "';";
                Debug.Print(query.ToString());
                int count = db.Count(query);
                Debug.Print(count.ToString());
                pData[i] = count;
                total += count;
                i += 1;
            }
            Debug.Print(total.ToString());
            Dictionary<string, float> dictionary = new Dictionary<string, float>();
            dictionary.Add(answers[0], (pData[0] / total));
            dictionary.Add(answers[1], (pData[1] / total));
            Debug.Print((pData[0] / total).ToString());
            Debug.Print((pData[1] / total).ToString());

            return dictionary;
        }


        public void ShowEvaluationForQWithFiveAnswers(int question)
        {
            ClearScreenForFeedbackSummary();

            // Array to store percentages
            Dictionary<string, float> data = new Dictionary<string, float>();
            data = GetFiveOptionQData(question);

            PieChart A = new PieChart();
            List<Tuple<string, float>> B = new List<Tuple<string, float>>();

            B.Add(new Tuple<string, float>("Sehr gut", data["Sehr gut"]));
            B.Add(new Tuple<string, float>("Gut", data["Gut"]));
            B.Add(new Tuple<string, float>("Nichts besonderes", data["Nichts besonderes"]));
            B.Add(new Tuple<string, float>("Schlecht", data["Schlecht"]));
            B.Add(new Tuple<string, float>("Sehr schlecht", data["Sehr schlecht"]));
            Bitmap pieChart = A.DrawPieChart(B);

            _pictureBoxFeedbackFavorites.BackgroundImage = pieChart;
            _pictureBoxFeedbackFavorites.Visible = true;

            _labelBarGraphOption1.Visible = true;
            _labelBarGraphOption2.Visible = true;
            _labelBarGraphOption3.Visible = true;
            _labelBarGraphOption4.Visible = true;
            _labelBarGraphOption5.Visible = true;
            _labelFeedbackQuestion.Text = _questions[question].question;
            _labelFeedbackQuestion.Visible = true;


            _labelBarGraphOption1.Size = _labelBarGraphOption2.Size = _labelBarGraphOption3.Size = _labelBarGraphOption4.Size = _labelBarGraphOption5.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.122), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption1.TextAlign = _labelBarGraphOption2.TextAlign = _labelBarGraphOption3.TextAlign = _labelBarGraphOption4.TextAlign = _labelBarGraphOption5.TextAlign = ContentAlignment.MiddleRight;
            _labelBarGraphOption1.Text = "Sehr gut";
            _labelBarGraphOption2.Text = "Gut";
            _labelBarGraphOption3.Text = "Nichts besonderes";
            _labelBarGraphOption4.Text = "Schlecht";
            _labelBarGraphOption5.Text = "Sehr schlecht";

            _labelForFeedbackPercentages.Visible = true;


            string[] answersQuestion1 = { "Sehr gut", "Gut", "Nichts besonderes", "Schlecht", "Sehr schlecht" };
            string[] votes = { Convert.ToInt16(data["Sehr gut"] * 100) + "%", Convert.ToInt16(data["Gut"] * 100) + "%", Convert.ToInt16(data["Nichts besonderes"] * 100) + "%", Convert.ToInt16(data["Schlecht"] * 100) + "%", Convert.ToInt16(data["Sehr schlecht"] * 100) + "%" };
            String s = ""; //String.Format("{1,-10} {0,-18}\n\n", "Optionen", "Stimmen in %");

            for (int index = 0; index < answersQuestion1.Length; index++)
            {
                if (index < answersQuestion1.Length - 1)
                    s += String.Format("{1,-10} {0,-18:N0}\n\n", answersQuestion1[index], votes[index]);
                else
                    s += String.Format("{1,-10} {0,-18:N0}", answersQuestion1[index], votes[index]);
            }

            _labelForFeedbackPercentages.Text = s;

        }


        public void ShowEvaluationForQWithTwoAnswers(int question)
        {
            ClearScreenForFeedbackSummary();

            // Array to store percentages
            Dictionary<string, float> data = new Dictionary<string, float>();
            data = GetTwoOptionQData(question);

            PieChart A = new PieChart();
            List<Tuple<string, float>> B = new List<Tuple<string, float>>();

            B.Add(new Tuple<string, float>("Ja", data["Ja"]));
            B.Add(new Tuple<string, float>("Nein", data["Nein"]));

            Bitmap pieChart = A.DrawPieChart(B);

            _pictureBoxFeedbackFavorites.BackgroundImage = pieChart;
            _pictureBoxFeedbackFavorites.Visible = true;

            _labelBarGraphOption1.Visible = true;
            _labelBarGraphOption2.Visible = true;

            _labelFeedbackQuestion.Text = _questions[question].question;
            _labelFeedbackQuestion.Visible = true;

            _labelBarGraphOption1.Size = _labelBarGraphOption2.Size = _labelBarGraphOption3.Size = _labelBarGraphOption4.Size = _labelBarGraphOption5.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.122), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));
            _labelBarGraphOption1.TextAlign = _labelBarGraphOption2.TextAlign = _labelBarGraphOption3.TextAlign = _labelBarGraphOption4.TextAlign = _labelBarGraphOption5.TextAlign = ContentAlignment.MiddleRight;
            _labelBarGraphOption1.Text = "Ja";
            _labelBarGraphOption2.Text = "Nein";

            _labelForFeedbackPercentages.Visible = true;


            string[] answersQuestion = { "Ja", "Nein"};
            string[] votes = { Convert.ToInt16(data["Ja"] * 100) + "%", Convert.ToInt16(data["Nein"] * 100) + "%"};
            String s = ""; //String.Format("{1,-10} {0,-18}\n\n", "Optionen", "Stimmen in %");

            for (int index = 0; index < answersQuestion.Length; index++)
            {
                    s += String.Format("{1,-10} {0,-18:N0}\n\n", answersQuestion[index], votes[index]);

            }

            _labelForFeedbackPercentages.Text = s;

        }


        public void UpdateFeedbackID()
        {
            DatabaseConnection db = new DatabaseConnection();
            string q = "SELECT * from feedback";
            List<string>[] result = db.SelectFromFeedback(q);
            int length = Convert.ToInt16((result[0].Count().ToString()));
            _feedbackID = length;
        }



        //-//-//-//           EVENT HANDLERS       //-//-//-//

        /// <summary>
        /// An event handler for clicking any of the options to a question
        /// </summary>
        private void Option_Click(object sender, EventArgs e)
        {
            _localOptionClick += 1;

            Button clickedButton = (Button)sender;

            clickedButton.Focus();

            Button answerGivenBtn = (Button)sender;
            Answer answer = new Answer(answerGivenBtn.Name);
            _currentVoter.AddGivenAnswer(answer);
            Debug.Print(_buttonOption0.ContainsFocus.ToString());
            List<Button> unFocusedBtns = new List<Button>();
            NextQuestion();

        }

        public void HighlightFocusedBtn(Button btnToHighlight)
        {
            btnToHighlight.FlatStyle = FlatStyle.Flat;
            btnToHighlight.FlatAppearance.BorderColor = Color.Blue;
            btnToHighlight.FlatAppearance.BorderSize = 3;
        }

        public void UnHighlightFocusedBtn(Button btnToHighlight)
        {
            btnToHighlight.FlatStyle = FlatStyle.Standard;
            this.ActiveControl = null;

        }




        private void LeaveOption(object sender, EventArgs e)
        {
            Button currentBtn = (Button)sender;
            currentBtn.FlatStyle = FlatStyle.Standard;
        }

        private void backBtn_Click(object sender, EventArgs e)
        {

            if(_feedbackSummaryCnter > 0)
            {
                _feedbackSummaryCnter -= 1;

                ClearScreenForFeedbackSummary();
                if (_feedbackSummaryCnter == 0)
                {
                    ShowEvaluationForFavourites();
                }
                else if (_feedbackSummaryCnter == 1 || _feedbackSummaryCnter == 4)
                {
                    ShowEvaluationForQWithFiveAnswers(_feedbackSummaryCnter);
                }
                else
                {
                    ShowEvaluationForQWithTwoAnswers(_feedbackSummaryCnter);
                }
            }
            Debug.Print(_feedbackSummaryCnter.ToString());

        }

        private void fwdBtn_Click(object sender, EventArgs e)
        {


            if (_feedbackSummaryCnter >= 0 && _feedbackSummaryCnter < 5)
            {
                _feedbackSummaryCnter += 1;
                ClearScreenForFeedbackSummary();
                if (_feedbackSummaryCnter == 0) {
                    ShowEvaluationForFavourites();
                }
                else if(_feedbackSummaryCnter == 1 || _feedbackSummaryCnter == 4)
                {
                    ShowEvaluationForQWithFiveAnswers(_feedbackSummaryCnter);
                }
                else
                {
                    ShowEvaluationForQWithTwoAnswers(_feedbackSummaryCnter);
                }
            }
            Debug.Print(_feedbackSummaryCnter.ToString());


        }

    }
}
