using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Wox.Plugin.StackOverlow.Infrascructure;
using Wox.Plugin.StackOverlow.Infrascructure.Api;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow
{
	public class Main : IPlugin
	{
	    private StackOverflowApi _api;

	    private PluginInitContext _context;

	    public void Init(PluginInitContext context)
	    {
	        _context = context;
            _api = new StackOverflowApi();
		}

		public List<Result> Query(Query query)
		{
		    var questionResponse = _api.GetQuestions(SearchRequestBuilder.Parse(query.Search));

		    return questionResponse.Items.Select(CreateResult).ToList();
		}

	    private Result CreateResult(Question question)
	    {
	        return new Result
	        {
	            Title = question.Title,
	            SubTitle = GetSubTitle(question),
	            IcoPath = question.IsAnswered ? "Images/accepted_answer.png" : "Images/so.png",
	            Action = _ =>
	            {
	                var link = question.Link;
	                if (!question.Link.ToLower().StartsWith("http"))
	                {
	                    link = "http://" + link;
	                }
	                try
	                {
	                    Process.Start(link);
	                    return true;    
	                }
	                catch (Exception exc)
	                {
	                    _context.API.ShowMsg("Cannot");
	                    return false;
	                }
	            }
	        };
	    }

	    private static string GetSubTitle(Question question)
	    {
	        return $"Answers count: {question.AnswerCount}. Tags: {string.Join(",", question.Tags)}";
	    }
	}
}