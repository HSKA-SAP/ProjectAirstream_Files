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

namespace Airstream
{
    public partial class UI_Feedback : Form
    {
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

        public UI_Feedback()
        {
            InitializeComponent();

            UI_General.SetGeneralElements(this);

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
            _labelBarGraphOption1.BackColor = ShowStatistics.DetermineColor(0);
            _labelBarGraphOption1.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.51));
            _labelBarGraphOption1.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.16), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));

            Controls.Add(_labelBarGraphOption1);

            _labelBarGraphOption2 = new Label();
            _labelBarGraphOption2.BackColor = ShowStatistics.DetermineColor(0);
            _labelBarGraphOption2.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.56));
            _labelBarGraphOption2.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.016), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));

            Controls.Add(_labelBarGraphOption2);

            _labelBarGraphOption3 = new Label();
            _labelBarGraphOption3.BackColor = ShowStatistics.DetermineColor(0);
            _labelBarGraphOption3.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.61));
            _labelBarGraphOption3.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.076), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));

            Controls.Add(_labelBarGraphOption3);

            _labelBarGraphOption4 = new Label();
            _labelBarGraphOption4.BackColor = ShowStatistics.DetermineColor(0);
            _labelBarGraphOption4.Location = new Point(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.093), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.66));
            _labelBarGraphOption4.Size = new Size(Convert.ToInt32(UI_General.GetSizeScreen().Width * 0.122), Convert.ToInt32(UI_General.GetSizeScreen().Height * 0.032));

            Controls.Add(_labelBarGraphOption4);

            _labelBarGraphOption5 = new Label();
            _labelBarGraphOption5.BackColor = ShowStatistics.DetermineColor(0);
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
        }

        private static string SetLabelFeedbackQuestionText()
        {
            return "HIER KÖNNTE IHRE FRAGE STEHEN? ;)";
        }

        private static string SetTextBoxFeedbackMoreInfoText()
        {
            return "Hier noch mehr Interessantes:\n\n35% der Wähler denken das X das Beste hier im Airstream ist.\n\n25% denken Y ist das interessanteste und\n\n21% bevorzugen Z.\n\nVielen Dank für Ihr Feedback. Es hilft uns dabei uns und unsere Arbeit hier im Airstream zu verbessern.";
        }
    }
}
