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

namespace Airstream.Feedback.Statistics
{
    public class ShowStatistics
    {
        Random random = new Random();
        static List<Color> listAllColors = new List<Color>();
        static List<Color> listUsedColors = new List<Color>();

        public static Color DetermineColor()
        {
            // result = null is not accepted, so I randomly took Color.White
            Color result = Color.White;

            listAllColors.Add(Color.FromArgb(92, 186, 230));
            listAllColors.Add(Color.FromArgb(182, 217, 87));
            listAllColors.Add(Color.FromArgb(250, 196, 100));
            listAllColors.Add(Color.FromArgb(140, 211, 255));
            listAllColors.Add(Color.FromArgb(217, 152, 203));

            for (int i = 0; i < (listAllColors.Count - 1); i++)
            {
                result = listAllColors[i];

                foreach (Color c in listUsedColors)
                {
                    if (result == c)
                    {
                        result = Color.White;
                    }
                }

                if (result != Color.White)
                {
                    listUsedColors.Add(result);
                    return result;
                }
            }

            return Color.Pink;
        }

        public static List<Color> getUsedColors()
        {
            return listUsedColors;
        }
    }

    public class PieChart
    {
        public Bitmap DrawPieChart(List<Tuple<string, float>> data)
        {
            int pieSize = (Int32)(UI_General.GetSizeScreen().Width / 6.4);
            float prevStart = 0;

            Bitmap result = new Bitmap(pieSize, pieSize);
            Graphics g = Graphics.FromImage(result);

            float totalValue = 0;
            for (int i = 0; i < data.Count; i++)
            {
                totalValue += data[i].Item2;
            }

            for (int i = 0; i < data.Count; i++)
            {
                g.FillPie(new SolidBrush(ShowStatistics.DetermineColor()), new Rectangle(0, 0, pieSize, pieSize), prevStart, (data[i].Item2 / totalValue) * 360);
                prevStart += (data[i].Item2 / totalValue) * 360;
            }
            
            ShowStatistics.getUsedColors().Clear();

            return result;
        }
    }

    class CalculateStatistics
    {
        public static List<double> AnalyseQuestion(Question question)
        {
            List<double> result = new List<double>();
            int totalVotesForQuestion = 0;
            double percentOfTheVotes;

            for(int i = 0; i < question.options.Count(); i++)
            {
                totalVotesForQuestion += question.options[i].countVotes;
            }

            foreach(Answer a in question.options)
            {
                percentOfTheVotes = a.countVotes / totalVotesForQuestion;
                result.Add(Math.Round(percentOfTheVotes, 2)); // ==> Round to 2 decimal places
            }

            return result;
        }
    }
}