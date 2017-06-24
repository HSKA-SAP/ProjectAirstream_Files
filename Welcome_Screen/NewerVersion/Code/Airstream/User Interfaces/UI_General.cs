using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Airstream
{
    public partial class UI_General : Form
    {
        private static Color _screenBackColor = Color.FromArgb(101, 140, 163);
        private static Color _screenForeColor = Color.FromArgb(219, 219, 219);
        private static Font _screenDefaultFont = new Font("Arial Bold", 18.0f);
        private static Size _sizeScreen;
        private static Bitmap _pathToBackGroundImage = new Bitmap(@"Pictures\Feedback_Background.jpg");

        private static Timer _time = new Timer();

        private static Label _labelHeader;
        private static Color _labelHeaderBackColor = Color.Transparent;
        private static string _labelHeaderText = SetLabelHeaderText();

        private static PictureBox _sapLogo;
        private static Bitmap _pathToSapLogo = new Bitmap(@"Pictures\SAP-Logo.jpg");

        public static Panel _labelWhite;
        private static Color _labelWhiteBackColor = Color.FromArgb(255, 255, 255);
        private static Color _labelWhiteForeColor = Color.FromArgb(45, 45, 45);
        private static Font _labelWhiteFont = new Font("Arial Bold", 16.0f);
        private static string projectFolder = "..\\..\\..\\";

        public UI_General()
        {
            this.DoubleBuffered = true;

        }


        public static string GetProjectFolder()
        {
            return projectFolder;
        }

        public static Size GetSizeScreen()
        {
            return _sizeScreen;
        }

        public static Font GetScreenDefaultFont()
        {
            return _screenDefaultFont;
        }

        public static Panel GetLabelGray()
        {
            return _labelWhite;
        }

        public static Color GetLabelGrayBackColor()
        {
            return _labelWhiteBackColor;
        }

        public static Color GetLabelGrayForeColor()
        {
            return _labelWhiteForeColor;
        }

        public static Font GetLabelGrayFont()
        {
            return _labelWhiteFont;
        }

        /// <summary>
        /// Initialisieren der Uhr (jede Sekunde aktualisiert)
        /// </summary>
        public static void InitializeTimer()
        {
            _time.Interval = 1000;
            _time.Tick += new EventHandler(TimerTick);
        }

        /// <summary>
        /// Aktualisierung der Uhrzeit und des Datums
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimerTick(object sender, EventArgs e)
        {
            _labelHeader.Text = SetLabelHeaderText();
        }

        private static string SetLabelHeaderText()
        {
            return string.Format("{0:00}.{1:00}.{2:0000}\n{3:00}:{4:00}:{5:00}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public static string GetLabelHeaderText()
        {
            return _labelHeaderText;
        }

        public static void Exit_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        /// <summary>
        /// Elements that will be used for both Welcome and Feedback-Screen
        /// </summary>
        /// <param name="form"></param>
        public static void SetGeneralElements(Form form)
        {
            InitializeTimer();
            _time.Start();
            form.TopMost = true;
            form.FormBorderStyle = FormBorderStyle.None;

            form.BackColor = _screenBackColor;
            form.Size = form.MaximumSize = form.MinimumSize = _sizeScreen = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            form.BackgroundImage = _pathToBackGroundImage;
            form.BackgroundImageLayout = ImageLayout.Stretch;
            // BEGINN ELEMENTE
            _labelHeader = new Label();
            _labelHeader.BackColor = Color.Transparent;
            _labelHeader.Font = _screenDefaultFont;
            _labelHeader.ForeColor = _screenForeColor;


            _labelHeader.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.06), Convert.ToInt32(_sizeScreen.Height * 0.05));
            _labelHeader.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.3), Convert.ToInt32(_sizeScreen.Height * 0.2));
            _labelHeader.Text = _labelHeaderText;

            form.Controls.Add(_labelHeader);

            _sapLogo = new PictureBox();
            _sapLogo.BackgroundImage = _pathToSapLogo;
            _sapLogo.BackgroundImageLayout = ImageLayout.Stretch;
            _sapLogo.Click += new EventHandler(Exit_Click);
            _sapLogo.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.815), Convert.ToInt32(_sizeScreen.Height * 0.05));
            _sapLogo.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.11), Convert.ToInt32(_sizeScreen.Height * 0.1));

            form.Controls.Add(_sapLogo);

            _labelWhite = new Panel();
            _labelWhite.BackColor = _labelWhiteBackColor;
            _labelWhite.BorderStyle = BorderStyle.Fixed3D;
            _labelWhite.Location = new Point(Convert.ToInt32(_sizeScreen.Width * 0.06), Convert.ToInt32(_sizeScreen.Height * 0.25));
            _labelWhite.Size = new Size(Convert.ToInt32(_sizeScreen.Width * 0.867), Convert.ToInt32(_sizeScreen.Height * 0.5));

            form.Controls.Add(_labelWhite);


        }
    }
}