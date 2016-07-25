using System;
using System.Collections.Generic;
using System.Linq;
using Wox.Plugin.StackOverlow.Infrascructure;
using Wox.Plugin.StackOverlow.Infrascructure.Api;

namespace Wox.Plugin.StackOverlow
{
	public class Main : IPlugin
	{
	    private StackOverflowApi _api;

	    private QuestionResultBuilder _questionResultBuilder;

        private QuestionsOrderer _questionsOrderer;

        public void Init(PluginInitContext context)
	    {
            _api = new StackOverflowApi();
            _questionResultBuilder = new QuestionResultBuilder(context);
            _questionsOrderer = new QuestionsOrderer();
		}

		public List<Result> Query(Query query)
		{
		    if (string.IsNullOrEmpty(query.Search))
		    {
		        return new List<Result>();
		    }

		    var questionResponse = _api.GetQuestions(SearchRequestBuilder.Parse(query.Search));

		    var orderedQuestions = _questionsOrderer.GetOrderedQuestions(questionResponse.Items);
		    return _questionResultBuilder.Convert(orderedQuestions);
		}
	}
}