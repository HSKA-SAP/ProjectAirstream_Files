using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Airstream.Feedback.QuestionsAndAnswers;
using Airstream.Feedback.Q_and_A_Logic;
using Airstream.Feedback.Voters;
using Airstream.User_Interfaces;
using Airstream.Feedback.Database;

namespace Airstream
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new UI_Feedback(questions,TestVoter));
            Application.Run(new UI_Default());
        }
    }
}
