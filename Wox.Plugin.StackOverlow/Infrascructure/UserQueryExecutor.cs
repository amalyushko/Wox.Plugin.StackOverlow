using System;
using System.Collections.Generic;
using Wox.Plugin.StackOverlow.Infrascructure.Api;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public sealed class UserQueryExecutor
    {
        private readonly IStackOverflowApi _stackOverflowApi;

        private readonly QuestionResultBuilder _questionResultBuilder;

        public UserQueryExecutor(IStackOverflowApi stackOverflowApi, QuestionResultBuilder questionResultBuilder)
        {
            if (stackOverflowApi == null) throw new ArgumentNullException(nameof(stackOverflowApi));
            if (questionResultBuilder == null) throw new ArgumentNullException(nameof(questionResultBuilder));

            _stackOverflowApi = stackOverflowApi;
            _questionResultBuilder = questionResultBuilder;
        }

        public List<Result> Execute(SearchRequest searchRequest)
        {
            if (string.IsNullOrEmpty(searchRequest?.Query))
            {
                return new List<Result>();
            }

            var response = _stackOverflowApi.GetQuestions(searchRequest);

            return _questionResultBuilder.ProcessServerResponse(response);
        }
    }
}