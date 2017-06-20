using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airstream.Feedback.Q_and_A_Logic
{
    class QuestionLogic
    {

        public static List<QuestionsAndAnswers.Question> CreateTheFeedbackQuestions()
        {
            QuestionsAndAnswers.Question q1 = new QuestionsAndAnswers.Question("Welches der folgenden Geräte im Airstream fanden Sie am besten?\n", new List<QuestionsAndAnswers.Answer>());
            q1.options.Add(new QuestionsAndAnswers.Answer("Alexa"));
            q1.options.Add(new QuestionsAndAnswers.Answer("NetAtmo"));
            q1.options.Add(new QuestionsAndAnswers.Answer("55 Zoll-Tisch"));
            q1.options.Add(new QuestionsAndAnswers.Answer("Welcome-Screen"));
            q1.options.Add(new QuestionsAndAnswers.Answer("Digital Boardroom"));

            QuestionsAndAnswers.Question q2 = new QuestionsAndAnswers.Question("Wie würden Sie den Gesamteindruck bewerten?\n", new List<QuestionsAndAnswers.Answer>());
            q2.options.Add(new QuestionsAndAnswers.Answer("Sehr gut"));
            q2.options.Add(new QuestionsAndAnswers.Answer("Gut"));
            q2.options.Add(new QuestionsAndAnswers.Answer("Nichts besonderes"));
            q2.options.Add(new QuestionsAndAnswers.Answer("Schlecht"));
            q2.options.Add(new QuestionsAndAnswers.Answer("Sehr schlecht"));
            /*
            QuestionsAndAnswers.Question q3= new QuestionsAndAnswers.Question("Random Question?\n", new List<QuestionsAndAnswers.Answer>());
            q3.options.Add(new QuestionsAndAnswers.Answer("Sehr gut"));
            q3.options.Add(new QuestionsAndAnswers.Answer("Gut"));
            q3.options.Add(new QuestionsAndAnswers.Answer("Nichts besonderes"));
            q3.options.Add(new QuestionsAndAnswers.Answer("Schlecht"));
            q3.options.Add(new QuestionsAndAnswers.Answer("Sehr schlecht"));
            */
            List<QuestionsAndAnswers.Question> questions = new List<QuestionsAndAnswers.Question>();

            questions.Add(q1);
            questions.Add(q2);
            //questions.Add(q3);
            return questions;
        }



    }


}
