using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wox.Plugin.StackOverlow.Infrascructure;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Tests.QuestionsOrderStrategy
{
    [TestFixture]
    public abstract class BaseQuestionsOrderStrategyTests
    {
        public abstract IQuestionsOrderStrategy GetOrderStategyInstance();

        [Test]
        public void GetOrderedQuestions_SameItemsCountAfterOrdering()
        {
            const int questionsCount = 5;

            var unorderedQuestions = new List<Question>();
                
            for (var i = 0; i < questionsCount; i++)
            {
                unorderedQuestions.Add(new Question {Score = i});
            }

            var stategy = GetOrderStategyInstance();
            var orderedQuestions = stategy.GetOrderedQuestions(unorderedQuestions);

            Assert.That(orderedQuestions.Count(), Is.EqualTo(unorderedQuestions.Count));
        }

        [Test]
        public void GetOrderedQuestions_SameItemsAfterOrdering()
        {
            var unorderedQuestions = new List<Question>
            {
                new Question {Score = 11},
                new Question {Score = 131, IsAnswered = true},
                new Question {Score = 7},
                new Question {Score = 42, IsAnswered = true},
                new Question {Score = 0},
            };

            var stategy = GetOrderStategyInstance();
            var orderedQuestions = stategy.GetOrderedQuestions(unorderedQuestions);

            CollectionAssert.AreEquivalent(orderedQuestions, unorderedQuestions);
        }

        [Test]
        public void GetOrderedQuestions_CorrectOrdering_ByScore()
        {
            var unorderedQuestions = new List<Question>
            {
                new Question {Score = 5},
                new Question {Score = 11},
                new Question {Score = 1},
                new Question {Score = 42},
                new Question {Score = 142},
            };

            var stategy = GetOrderStategyInstance();
            var orderedQuestions = stategy.GetOrderedQuestions(unorderedQuestions);

            Assert.That(orderedQuestions, Is.Ordered.By("Score").Descending);
        }

        [Test]
        public void GetOrderedQuestions_CorrectOrdering_IsAnsweredHasGreaterWeightThanScore()
        {
            var unorderedQuestions = new List<Question>
            {
                new Question {Score = 1, IsAnswered = true},
                new Question {Score = 100},
                new Question {Score = 1},
                new Question {Score = 424},
                new Question {Score = 10000},
            };

            var stategy = GetOrderStategyInstance();
            var orderedQuestions = stategy.GetOrderedQuestions(unorderedQuestions);

            Assert.That(orderedQuestions, Is.Ordered.Using(new Comparison<Question>(CompareQuestions)).Descending);
        }


        [Test]
        public void GetOrderedQuestions_EmptyCollection_ThrowsNothing()
        {
            var emptyQuestions = new List<Question>();

            var stategy = GetOrderStategyInstance();

            Assert.DoesNotThrow(() => stategy.GetOrderedQuestions(emptyQuestions));
        }

        [Test]
        public void GetOrderedQuestions_EmptyCollection_EmptyResult()
        {
            var emptyQuestions = new List<Question>();

            var stategy = GetOrderStategyInstance();
            var orderedQuestions = stategy.GetOrderedQuestions(emptyQuestions);

            CollectionAssert.IsEmpty(orderedQuestions);
        }

        [Test]
        public void GetOrderedQuestions_CorrectOrdering_ContainsSameItems()
        {
            var unorderedQuestions = new List<Question>
            {
                new Question {Score = 1, IsAnswered = true},
                new Question {Score = 100},
                new Question {Score = 1},
                new Question {Score = 42},
                new Question {Score = 1, IsAnswered = true},
                new Question {Score = 42},
                new Question {Score = 42},
                new Question {Score = 42},
                new Question {Score = 42},
                new Question {Score = 1, IsAnswered = false},
                new Question {Score = 4},
            };

            var stategy = GetOrderStategyInstance();
            var orderedQuestions = stategy.GetOrderedQuestions(unorderedQuestions);

            Assert.That(orderedQuestions, Is.Ordered.Using(new Comparison<Question>(CompareQuestions)).Descending);
        }

        private int CompareQuestions(Question q1, Question q2)
        {
            var result = q1.IsAnswered.CompareTo(q2.IsAnswered);
            return result == 0 ? q2.Score.CompareTo(q2.Score) : result;
        }
    }
}