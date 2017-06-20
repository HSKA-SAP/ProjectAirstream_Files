using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Airstream.Feedback.QuestionsAndAnswers
{
    public class Question
    {
        private int _number;
        private string _question;
        private List<Answer> _options;

        public int number { get { return _number; } }
        public string question { get { return _question; } }
        public List<Answer> options { get { return _options; } }

        public Question(string question, List<Answer> options)
        {
            if (String.IsNullOrEmpty(question))
            {
                throw new Exception("No Question!");
            }

            _question = question;
            _options = options;
        }

    }

    public class Answer
    {
        private static List<Answer> listAllAnswers = new List<Answer>();

        private string _text;
        private static int _countVotes;

        public string text { get { return _text; } }
        public int countVotes { get { return _countVotes; } }

        public Answer(string text)
        {
            if (String.IsNullOrEmpty(text))
                throw new Exception("No text existing!");

            _text = text;
            _countVotes = 0;

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

        public void AddCountVote()
        {
            _countVotes++;
        }
    }
}