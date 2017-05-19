﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airstream.Feedback.QuestionsAndAnswers;

namespace Airstream.Feedback.Voters
{
    class Voter
    {
        private List<Voter> listAllVoters = new List<Voter>();

        private long _id;
        private List<Answer> _givenAnswers;

        public long id { get { return _id; } }
        public List<Answer> givenAnswers { get { return _givenAnswers; } }

        public Voter(long id)
        {
            _id = id;
            _givenAnswers = new List<Answer>();

            listAllVoters.Add(this);
        }

        public List<Voter> GetAllVoters()
        {
            return listAllVoters;
        }

        public void AddGivenAnswer(Voter voter, Answer answer)
        {
            Answer.SetDateTimeVote(answer);
            voter._givenAnswers.Add(answer);
        }
    }
}