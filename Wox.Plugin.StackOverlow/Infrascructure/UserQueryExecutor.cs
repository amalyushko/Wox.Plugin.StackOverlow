using System;
using System.Collections.Generic;
using Wox.Plugin.StackOverlow.Infrascructure.Api;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public sealed class UserQueryExecutor
    {
        private readonly StackOverflowApi _stackOverflowApi;

        private readonly QuestionResultBuilder _questionResultBuilder;

        private readonly IQuestionsOrderer _questionsOrderer;

        public UserQueryExecutor(StackOverflowApi stackOverflowApi, QuestionResultBuilder questionResultBuilder, IQuestionsOrderer questionsOrderer)
        {
            if (stackOverflowApi == null) throw new ArgumentNullException(nameof(stackOverflowApi));
            if (questionResultBuilder == null) throw new ArgumentNullException(nameof(questionResultBuilder));
            if (questionsOrderer == null) throw new ArgumentNullException(nameof(questionsOrderer));

            _stackOverflowApi = stackOverflowApi;
            _questionResultBuilder = questionResultBuilder;
            _questionsOrderer = questionsOrderer;
        }

        public List<Result> Execute(Query query)
        {
            if (string.IsNullOrEmpty(query.Search))
            {
                return new List<Result>();
            }

            var questionResponse = _stackOverflowApi.GetQuestions(SearchRequestBuilder.Parse(query.Search));

            var orderedQuestions = _questionsOrderer.GetOrderedQuestions(questionResponse.Items);
            return _questionResultBuilder.Convert(orderedQuestions);
        } 
    }
}