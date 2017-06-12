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
            QuestionsAndAnswers.Question q1 = new QuestionsAndAnswers.Question("What was your favourite feature inside the Airstream?\n", new List<QuestionsAndAnswers.Answer>());
            q1.options.Add(new QuestionsAndAnswers.Answer("Alexa"));
            q1.options.Add(new QuestionsAndAnswers.Answer("NetAtmo"));
            q1.options.Add(new QuestionsAndAnswers.Answer("55 Zoll-Tisch"));
            q1.options.Add(new QuestionsAndAnswers.Answer("Welcome-Screen"));
            q1.options.Add(new QuestionsAndAnswers.Answer("Digital Boardroom"));

            QuestionsAndAnswers.Question q2 = new QuestionsAndAnswers.Question("How would you rate your overall experience?\n", new List<QuestionsAndAnswers.Answer>());
            q2.options.Add(new QuestionsAndAnswers.Answer("Sehr gut"));
            q2.options.Add(new QuestionsAndAnswers.Answer("Gut"));
            q2.options.Add(new QuestionsAndAnswers.Answer("Nichts besonderes"));
            q2.options.Add(new QuestionsAndAnswers.Answer("Schlecht"));
            q2.options.Add(new QuestionsAndAnswers.Answer("Sehr schlecht"));

            QuestionsAndAnswers.Question q3= new QuestionsAndAnswers.Question("Did the coffee machine make coffee when asked with Alexa?\n", new List<QuestionsAndAnswers.Answer>());
            q3.options.Add(new QuestionsAndAnswers.Answer("Ja"));
            q3.options.Add(new QuestionsAndAnswers.Answer("Nein"));



            QuestionsAndAnswers.Question q4 = new QuestionsAndAnswers.Question("Did the seat sensors detect you were seated??\n", new List<QuestionsAndAnswers.Answer>());
            q4.options.Add(new QuestionsAndAnswers.Answer("Ja"));
            q4.options.Add(new QuestionsAndAnswers.Answer("Nein"));


            QuestionsAndAnswers.Question q5 = new QuestionsAndAnswers.Question("How well did the touchscreen work?\n", new List<QuestionsAndAnswers.Answer>());
            q5.options.Add(new QuestionsAndAnswers.Answer("Sehr gut"));
            q5.options.Add(new QuestionsAndAnswers.Answer("Gut"));
            q5.options.Add(new QuestionsAndAnswers.Answer("Nichts besonderes"));
            q5.options.Add(new QuestionsAndAnswers.Answer("Schlecht"));
            q5.options.Add(new QuestionsAndAnswers.Answer("Sehr schlecht"));


            QuestionsAndAnswers.Question q6 = new QuestionsAndAnswers.Question("Were you identified accurately by the Welcome Screen?\n", new List<QuestionsAndAnswers.Answer>());
            q6.options.Add(new QuestionsAndAnswers.Answer("Ja"));
            q6.options.Add(new QuestionsAndAnswers.Answer("Nein"));

            List<QuestionsAndAnswers.Question> questions = new List<QuestionsAndAnswers.Question>();

            questions.Add(q1);
            questions.Add(q2);
            questions.Add(q3);
            questions.Add(q4);
            questions.Add(q5);
            questions.Add(q6);
            return questions;
        }



    }


}
