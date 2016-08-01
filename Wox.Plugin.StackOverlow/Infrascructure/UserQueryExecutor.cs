using System;
using System.Collections.Generic;
using System.Diagnostics;
using Wox.Plugin.StackOverlow.Infrascructure.Api;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public sealed class UserQueryExecutor
    {
        private readonly StackOverflowApi _stackOverflowApi;

        private readonly QuestionResultBuilder _questionResultBuilder;

        private readonly IQuestionsOrderStrategy _questionsOrderStrategy;

        public UserQueryExecutor(StackOverflowApi stackOverflowApi, QuestionResultBuilder questionResultBuilder, IQuestionsOrderStrategy questionsOrderStrategy)
        {
            if (stackOverflowApi == null) throw new ArgumentNullException(nameof(stackOverflowApi));
            if (questionResultBuilder == null) throw new ArgumentNullException(nameof(questionResultBuilder));
            if (questionsOrderStrategy == null) throw new ArgumentNullException(nameof(questionsOrderStrategy));

            _stackOverflowApi = stackOverflowApi;
            _questionResultBuilder = questionResultBuilder;
            _questionsOrderStrategy = questionsOrderStrategy;
        }

        public List<Result> Execute(Query query)
        {
            if (string.IsNullOrEmpty(query.Search))
            {
                return new List<Result>();
            }

            var response = _stackOverflowApi.GetQuestions(SearchRequestBuilder.Parse(query.Search));

            return ProcessServerResponse(response);
        }

        private List<Result> ProcessServerResponse(Response response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            var questionsResponse = response as QuestionResponse;
            if (questionsResponse != null)
            {
                var orderedQuestions = _questionsOrderStrategy.GetOrderedQuestions(questionsResponse.Items);
                return _questionResultBuilder.Convert(orderedQuestions);
            }

            var noConnectionResponse = response as NoConnectionResponse;
            if (noConnectionResponse != null)
            {
                return new List<Result> {_questionResultBuilder.ConvertToNoConnectionResult()};
            }

            var requestErrorResponse = response as ErrorResponse;
            if (requestErrorResponse != null)
            {
                return new List<Result> { _questionResultBuilder.ConvertToRequestErrorResult(requestErrorResponse) };
            }

            // for now just return empty list
            return new List<Result>();
        }
    }
}