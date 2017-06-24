﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airstream.Feedback.QuestionsAndAnswers;

namespace Airstream.Feedback.Voters
{
    public class Voter
    {
        private static List<Voter> listAllVoters = new List<Voter>();

        private long _id;
        private List<Answer> _givenAnswers;
        private DateTime _dateTimeVote;

        public long id { get { return _id; } }
        public List<Answer> givenAnswers { get { return _givenAnswers; } }
        public DateTime dateTimeVote { get { return _dateTimeVote; } }

        public Voter(long id)
        {
            _id = id;
            _givenAnswers = new List<Answer>();
        }


        public void SetDateTimeVote()
        {
            _dateTimeVote = DateTime.Now;
        }

        public void AddGivenAnswer(Answer answer)
        {
            SetDateTimeVote();
            _givenAnswers.Add(answer);
        }
    }
}