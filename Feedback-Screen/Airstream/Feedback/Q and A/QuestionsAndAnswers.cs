using System;
using System.Collections.Generic;
using System.Linq;
using Airstream.Feedback.Voters;

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

            if (options.Count() < 2)
                throw new Exception("Not enough options to answer the question!");

            _number = listAllQuestion.Count();
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

    class Answer
    {
        private string _text;
        private int _countVotes;
        private DateTime _dateTimeVote;

        public string text { get { return _text; } }
        public int countVotes { get { return _countVotes; } }
        public DateTime dateTimeVote { get { return _dateTimeVote; } }

        public Answer(string text)
        {
            if (String.IsNullOrEmpty(text))
                throw new Exception("No text existing!");

            _text = text;
            _countVotes = 0;
        }

        public static void SetDateTimeVote(Answer answer)
        {
            answer._dateTimeVote = DateTime.Now;
        }
    }
}
