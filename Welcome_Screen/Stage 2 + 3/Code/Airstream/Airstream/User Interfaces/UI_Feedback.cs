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
        private Color _screenBackColor = Color.FromArgb(0, 74, 139);
        private Color _screenForeColor = Color.FromArgb(117, 177, 201);
        private Font _screenDefaultFont = new Font("Arial Bold", 22.0f);
        private Size _sizeScreen;
        private string _screenText = "Feedback-Screen";

        private Timer _time = new Timer();

        private Label _labelHeader;
        private Color _labelHeaderBackColor = Color.Transparent;
        private string _labelHeaderText = "      Welcome to the Airstream! \n__________________________\n        " + string.Format("{0:00}.{1:00}.{2:0000}     {3:00}:{4:00}:{5:00}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        private PictureBox _sapLogo;

        private Label _labelGray;
        private Color _labelGrayBackColor = Color.FromArgb(198, 194, 191);
        private Color _labelGrayForeColor = Color.FromArgb(45, 45, 45);
        private Font _labelGrayFont = new Font("Arial Bold", 16.0f);

        private Label _labelFeedbackQuestion;
        private string _labelFeedbackQuestionText = "THIS COULD BE YOUR QUESTION? ;)";

        private Label _labelFeedbackWhatOthersHadToSay;
        private string _labelFeedbackWhatOthersHadToSayText = "What others had to say";

        private Label _labelBarGraphOption1;
        private Label _labelBarGraphOption2;
        private Label _labelBarGraphOption3;
        private Label _labelBarGraphOption4;
        private Label _labelBarGraphOption5;

        private Label _labelFeedbackFavorites;
        private string _labelFeedbackFavoritesText = "Favorites";

        private PictureBox _pictureBoxFeedbackFavorites;

        private Label _labelFeedbackMoreInfo;
        private string _labelFeedbackMoreInfoText = "More info";

        private RichTextBox _textBoxFeedbackMoreInfo;
        private Font _textBoxFeedbackMoreInfoFont = new Font("Arial Bold", 13.5f);

        public UI_Feedback()
        {
            InitializeComponent();
            InitializeTimer();
            _time.Start();
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;

            BackColor = _screenBackColor;
            Size = MaximumSize = MinimumSize = _sizeScreen = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Text = _screenText;

            // BEGINN ELEMENTE
            _labelHeader = new Label();
            _labelHeader.BackColor = _labelHeaderBackColor;
            _labelHeader.Font = _screenDefaultFont;
            _labelHeader.ForeColor = _screenForeColor;
            _labelHeader.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.377), Convert.ToInt32(_sizeScreen.Height * 0.05));
            _labelHeader.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.3), Convert.ToInt32(_sizeScreen.Height * 0.2));
            _labelHeader.Text = _labelHeaderText;

            Controls.Add(_labelHeader);

            _sapLogo = new PictureBox();
            _sapLogo.BackgroundImage = new Bitmap(@"Pictures\SAP-Logo.jpg");
            _sapLogo.BackgroundImageLayout = ImageLayout.Stretch;
            _sapLogo.Click += new EventHandler(Exit_Click);
            _sapLogo.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.815), Convert.ToInt32(_sizeScreen.Height * 0.05));
            _sapLogo.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.11), Convert.ToInt32(_sizeScreen.Height * 0.1));

            Controls.Add(_sapLogo);

            // START---FEEDBACK

            // AUSWERTUNGS-ELEMENTE
            _labelFeedbackMoreInfo = new Label();
            _labelFeedbackMoreInfo.BackColor = _labelGrayBackColor;
            _labelFeedbackMoreInfo.ForeColor = _labelGrayForeColor;
            _labelFeedbackMoreInfo.Font = _labelGrayFont;
            _labelFeedbackMoreInfo.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.698), Convert.ToInt32(_sizeScreen.Height * 0.40));
            _labelFeedbackMoreInfo.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.16), Convert.ToInt32(_sizeScreen.Height * 0.04));
            _labelFeedbackMoreInfo.Text = _labelFeedbackMoreInfoText;

            Controls.Add(_labelFeedbackMoreInfo);

            _textBoxFeedbackMoreInfo = new RichTextBox();
            _textBoxFeedbackMoreInfo.BackColor = _labelGrayBackColor;
            _textBoxFeedbackMoreInfo.Cursor = Cursors.No;
            _textBoxFeedbackMoreInfo.ForeColor = _labelGrayForeColor;
            _textBoxFeedbackMoreInfo.Font = _textBoxFeedbackMoreInfoFont;
            _textBoxFeedbackMoreInfo.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.7), Convert.ToInt32(_sizeScreen.Height * 0.50));
            _textBoxFeedbackMoreInfo.ReadOnly = true;
            _textBoxFeedbackMoreInfo.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.2), Convert.ToInt32(_sizeScreen.Height * 0.278));
            _textBoxFeedbackMoreInfo.Text = "Here is some more intersting stuff:\n\n35% of the voters think that X is the best thing here in the Airstream.\n\n25% think Y is the most interesting thing and\n\n21% prefer Z as there favorite gadget.\n\nThank you for giving us a feedback. This will help us to improve ourselves and our work here in the Airstream.";

            Controls.Add(_textBoxFeedbackMoreInfo);

            _labelFeedbackFavorites = new Label();
            _labelFeedbackFavorites.BackColor = _labelGrayBackColor;
            _labelFeedbackFavorites.ForeColor = _labelGrayForeColor;
            _labelFeedbackFavorites.Font = _labelGrayFont;
            _labelFeedbackFavorites.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.45), Convert.ToInt32(_sizeScreen.Height * 0.40));
            _labelFeedbackFavorites.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.2), Convert.ToInt32(_sizeScreen.Height * 0.04));
            _labelFeedbackFavorites.Text = _labelFeedbackFavoritesText;

            Controls.Add(_labelFeedbackFavorites);

            _pictureBoxFeedbackFavorites = new PictureBox();
            _pictureBoxFeedbackFavorites.BackColor = _labelGrayBackColor;
            _pictureBoxFeedbackFavorites.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.4), Convert.ToInt32(_sizeScreen.Height * 0.50));
            _pictureBoxFeedbackFavorites.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.156), Convert.ToInt32(_sizeScreen.Height * 0.278));

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
            _labelFeedbackWhatOthersHadToSay.BackColor = _labelGrayBackColor;
            _labelFeedbackWhatOthersHadToSay.ForeColor = _labelGrayForeColor;
            _labelFeedbackWhatOthersHadToSay.Font = _labelGrayFont;
            _labelFeedbackWhatOthersHadToSay.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.093), Convert.ToInt32(_sizeScreen.Height * 0.40));
            _labelFeedbackWhatOthersHadToSay.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.8), Convert.ToInt32(_sizeScreen.Height * 0.04));
            _labelFeedbackWhatOthersHadToSay.Text = _labelFeedbackWhatOthersHadToSayText;

            Controls.Add(_labelFeedbackWhatOthersHadToSay);

            _labelBarGraphOption1 = new Label();
            _labelBarGraphOption1.BackColor = ShowStatistics.DetermineColor(0);
            _labelBarGraphOption1.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.093), Convert.ToInt32(_sizeScreen.Height * 0.51));
            _labelBarGraphOption1.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.16), Convert.ToInt32(_sizeScreen.Height * 0.032));

            Controls.Add(_labelBarGraphOption1);

            _labelBarGraphOption2 = new Label();
            _labelBarGraphOption2.BackColor = ShowStatistics.DetermineColor(0);
            _labelBarGraphOption2.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.093), Convert.ToInt32(_sizeScreen.Height * 0.56));
            _labelBarGraphOption2.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.016), Convert.ToInt32(_sizeScreen.Height * 0.032));

            Controls.Add(_labelBarGraphOption2);

            _labelBarGraphOption3 = new Label();
            _labelBarGraphOption3.BackColor = ShowStatistics.DetermineColor(0);
            _labelBarGraphOption3.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.093), Convert.ToInt32(_sizeScreen.Height * 0.61));
            _labelBarGraphOption3.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.076), Convert.ToInt32(_sizeScreen.Height * 0.032));

            Controls.Add(_labelBarGraphOption3);

            _labelBarGraphOption4 = new Label();
            _labelBarGraphOption4.BackColor = ShowStatistics.DetermineColor(0);
            _labelBarGraphOption4.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.093), Convert.ToInt32(_sizeScreen.Height * 0.66));
            _labelBarGraphOption4.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.122), Convert.ToInt32(_sizeScreen.Height * 0.032));

            Controls.Add(_labelBarGraphOption4);

            _labelBarGraphOption5 = new Label();
            _labelBarGraphOption5.BackColor = ShowStatistics.DetermineColor(0);
            _labelBarGraphOption5.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.093), Convert.ToInt32(_sizeScreen.Height * 0.71));
            _labelBarGraphOption5.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.092), Convert.ToInt32(_sizeScreen.Height * 0.032));

            Controls.Add(_labelBarGraphOption5);

            _labelFeedbackQuestion = new Label();
            _labelFeedbackQuestion.BackColor = _labelGrayBackColor;
            _labelFeedbackQuestion.ForeColor = _labelGrayForeColor;
            _labelFeedbackQuestion.Font = _screenDefaultFont;
            _labelFeedbackQuestion.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.09), Convert.ToInt32(_sizeScreen.Height * 0.28));
            _labelFeedbackQuestion.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.8), Convert.ToInt32(_sizeScreen.Height * 0.14));
            _labelFeedbackQuestion.Text = _labelFeedbackQuestionText;

            Controls.Add(_labelFeedbackQuestion);

            _labelGray = new Label();
            _labelGray.BackColor = _labelGrayBackColor;
            _labelGray.BorderStyle = BorderStyle.Fixed3D;
            _labelGray.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.06), Convert.ToInt32(_sizeScreen.Height * 0.25));
            _labelGray.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.867), Convert.ToInt32(_sizeScreen.Height * 0.64));

            Controls.Add(_labelGray);

            ShowStatistics.getUsedColors().Clear();

            // END---FEEDBACK
        }

        /// <summary>
        /// Initialisieren der Uhr (jede Sekunde aktualisiert)
        /// </summary>
        public void InitializeTimer()
        {
            _time.Interval = 1000;
            _time.Tick += new EventHandler(TimerTick);
        }

        /// <summary>
        /// Aktualisierung der Uhrzeit und des Datums
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerTick(object sender, EventArgs e)
        {
            _labelHeader.Text = "      Welcome to the Airstream! \n__________________________\n        " + string.Format("{0:00}.{1:00}.{2:0000}     {3:00}:{4:00}:{5:00}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second); ;
        }

        void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
