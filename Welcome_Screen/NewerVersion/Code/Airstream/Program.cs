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
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Airstream
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        /// 

        private static int _uniqQID;
        private static int _uniqWhatsInsideID;
        private static string projectFolder = "..\\..\\..\\";

        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UpdateUniqueQID();
            UpdateUniqueWhatsInsideID();
            RefereshData();
            Application.Run(new UI_Default());
        }

        public static void AddQuestionDataDYK()
        {
            //Add default data to the database
            DatabaseConnection db = new DatabaseConnection();
            List<string> questions = new List<string>();
            List<string> optionAs = new List<string>();
            List<string> optionBs = new List<string>();
            List<string> optionCs = new List<string>();
            List<string> optionDs = new List<string>();
            List<string> answers = new List<string>();

            int iterator = 0;
            using (var fs = File.OpenRead(projectFolder + @"Data\did_you_know_data.csv"))
            using (var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (iterator >= 1)
                    {
                        questions.Add(values[0]);
                        optionAs.Add(values[1]);
                        optionBs.Add(values[2]);
                        optionCs.Add(values[3]);
                        optionDs.Add(values[4]);
                        answers.Add(values[5]);
                    }

                    iterator += 1;
                }
            }

            List<string>[] result = db.SelectFromDYK("SELECT * FROM didyouknowdata");
            // Note result[0][3] grabs the 3rd row and the first column (zero based indexing)

            int i = 0;
            //populate data if empty
            if(result[0].Any() == false)
            {
                _uniqQID = 0;

                foreach(var val in questions)
                {
                    string s1, s2, s3, s4, s5, s6, s7;
                    s1 = _uniqQID.ToString();
                    s2 = questions[i];
                    s3 = optionAs[i];
                    s4 = optionBs[i];
                    s5 = optionCs[i];
                    s6 = optionDs[i];
                    s7 = answers[i];
                    i += 1;
                    _uniqQID += 1;
                    string q = String.Format("INSERT INTO didyouknowdata VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}');", s1, s2, s3, s4, s5, s6, s7);
                    db.InsertUpdateDelete(q);
                }
            }
            db.CloseConnection();

        }


        public static void AddDataWhatsInside()
        {
            //Add default data to the database
            DatabaseConnection db = new DatabaseConnection();
            List<string> item = new List<string>();
            List<string> data = new List<string>();
            List<string> technicalspecs = new List<string>();
            List<string> filepaths = new List<string>();



            int iterator = 0;
            try
            {
                using (var fs = File.OpenRead(projectFolder + @"Data\whats_inside_data.csv"))
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (iterator >= 1)
                        {
                            StreamReader sr = new StreamReader(projectFolder + @"Data\" + values[1].ToString(), Encoding.UTF7);
                            string descText = sr.ReadToEnd();
                            Debug.Print(descText);
                            sr = new StreamReader(projectFolder + @"Data\" + values[2].ToString(), Encoding.UTF7);
                            string techDescText = sr.ReadToEnd();

                            item.Add(values[0]);
                            data.Add(descText);
                            technicalspecs.Add(techDescText);
                            filepaths.Add(values[3]);
                            Debug.Print(values[1].ToString());
                        }

                        iterator += 1;
                    }
                }
            }
            catch(Exception e)
            {
                Debug.Print("The data file is open in another application");
                MessageBox.Show("Close the app and then close excel before continuing");
            }
            

            List<string>[] result = db.SelectFromWhatsInside("SELECT * FROM whatsinsidedata");
            // Note result[0][3] grabs the 3rd row and the first column (zero based indexing)

            int i = 0;
            //populate data if empty
            if (result[0].Any() == false)
            {
                _uniqWhatsInsideID = 0;

                foreach (var val in data)
                {
                    string s1, s2, s3, s4, s5;
                    s1 = _uniqWhatsInsideID.ToString();
                    s2 = item[i];
                    s3 = data[i];
                    s4 = technicalspecs[i];
                    s5 = filepaths[i];
                    i += 1;
                    _uniqWhatsInsideID += 1;
                    string q = String.Format("INSERT INTO whatsinsidedata VALUES('{0}','{1}','{2}','{3}','{4}');", s1, s2, s3, s4,s5);
                    db.InsertUpdateDelete(q);
                }
            }
            db.CloseConnection();

        }

        public static void RefereshData()
        {
            DatabaseConnection db = new DatabaseConnection();
            string q1 = "DELETE FROM didyouknowdata";
            db.InsertUpdateDelete(q1);
            string q2 = "DELETE FROM whatsinsidedata";
            db.InsertUpdateDelete(q2);
            AddDataWhatsInside();
            AddQuestionDataDYK();
        }
        public static void UpdateUniqueQID()
        {
            DatabaseConnection db = new DatabaseConnection();
            List<string>[] result = db.SelectFromDYK("SELECT * FROM didyouknowdata");
            _uniqQID = result[0].ToArray().Length;
        }

        public static void UpdateUniqueWhatsInsideID()
        {
            DatabaseConnection db = new DatabaseConnection();
            List<string>[] result = db.SelectFromWhatsInside("SELECT * FROM whatsinsidedata");
            _uniqWhatsInsideID = result[0].ToArray().Length;
        }

        public static string StringToCSVCell(string str)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }


    }
}
