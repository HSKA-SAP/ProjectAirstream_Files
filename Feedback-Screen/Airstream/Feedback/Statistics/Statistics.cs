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

            listAllColors.Add(Color.FromArgb(0, 153, 204));
            listAllColors.Add(Color.FromArgb(171, 231, 255));
            listAllColors.Add(Color.FromArgb(33, 107, 172));
            listAllColors.Add(Color.FromArgb(0, 255, 204));
            listAllColors.Add(Color.FromArgb(0, 74, 139));
            listAllColors.Add(Color.FromArgb(117, 177, 201));

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
            int PieSize = (Int32)(UI_General.GetSizeScreen().Width / 6.4);
            float PrevStart = 0;

            Bitmap Result = new Bitmap(PieSize, PieSize);
            Graphics G = Graphics.FromImage(Result);

            float TotalValue = 0;
            for (int i = 0; i < data.Count; i++)
            {
                TotalValue += data[i].Item2;
            }

            for (int i = 0; i < data.Count; i++)
            {
                G.FillPie(new SolidBrush(ShowStatistics.DetermineColor()), new Rectangle(0, 0, PieSize, PieSize), PrevStart, (data[i].Item2 / TotalValue) * 360);
                PrevStart += (data[i].Item2 / TotalValue) * 360;
            }
            
            ShowStatistics.getUsedColors().Clear();

            return Result;
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
