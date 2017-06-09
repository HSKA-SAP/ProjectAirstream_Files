using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Airstream.Feedback.QuestionsAndAnswers
{
    class Question
    {
        private static List<Question> listAllQuestion = new List<Question>();

        private int _number;
        private string _question;
        private List<Answer> _options;

        public int number { get { return _number; } }
        public string question { get { return _question; } }
        public List<Answer> options { get { return _options; } }

        public Question(string question, List<Answer> options)
        {
            if (String.IsNullOrEmpty(question))
                throw new Exception("No Question!");

            //if (options.Count() < 2)
              //  throw new Exception("Not enough options to answer the question!");

            _number = listAllQuestion.Count();
            _question = question;
            _options = options;

            listAllQuestion.Add(this);
        }

        public static List<Question> GetAllQuestion()
        {
            return listAllQuestion;
        }

        public static void CreateTheFeedbackQuestions()
        {
            Question q1 = new Question("                  Welches der folgenden Geräte im Airstream fanden Sie am besten?\n_______________________________________________________________________", new List<Answer>());
            q1._options.Add(new Answer("Alexa", new Bitmap(@"Pictures\alexalogodone.jpg")));
            q1._options.Add(new Answer("NetAtmo", new Bitmap(@"Pictures\netatmologodone.jpg")));
            q1._options.Add(new Answer("55 Zoll-Tisch", new Bitmap(@"Pictures\touchtablelogodone.jpg")));
            q1._options.Add(new Answer("Welcome-Screen", new Bitmap(@"Pictures\welcomescreenlogodone.jpg")));
            q1._options.Add(new Answer("Digital Boardroom", new Bitmap(@"Pictures\digitalboardroomlogodone.jpg")));
        }
    }

    class Answer
    {
        private static List<Answer> listAllAnswers = new List<Answer>();

        private string _text;
        private static int _countVotes;
        private static Bitmap _logo;

        public string text { get { return _text; } }
        public int countVotes { get { return _countVotes; } }
        public Bitmap logo { get { return _logo; } }

        public Answer(string text, Bitmap logo)
        {
            if (String.IsNullOrEmpty(text))
                throw new Exception("No text existing!");

            _text = text;
            _countVotes = 0;
            _logo = logo;

            listAllAnswers.Add(this);
        }
        
        public static List<Answer> GetAllAnswers()
        {
            return listAllAnswers;
        }

        public static int GetCountVotes(Answer answer)
        {
            return answer.countVotes;
        }

        public static Bitmap GetLogo(Answer answer)
        {
            return answer.logo;
        }

        public void AddCountVote()
        {
            _countVotes++;
        }
    }
}
