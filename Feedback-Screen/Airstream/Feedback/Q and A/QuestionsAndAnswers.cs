using System;
using System.Collections.Generic;
using System.Linq;

namespace Airstream.Feedback.QuestionsAndAnswers
{
    class Question
    {
        private static List<Question> listAllQuestion = new List<Question>();

        private string _question;
        private List<string> _options;

        public string question { get { return _question; } }
        public List<string> options { get { return _options; } }

        public Question(string question, List<string> options)
        {
            if (String.IsNullOrEmpty(question))
                throw new Exception("No Question!");

            if (options.Count() < 2)
                throw new Exception("Not enough options to answer the question!");

            _question = question;
            _options = options;

            listAllQuestion.Add(this);
        }

        public static List<Question> getAllQuestion()
        {
            return listAllQuestion;
        }

        public static void CreateTheFeedbackQuestions()
        {

        }
    }
}