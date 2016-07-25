using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public class QuestionResultBuilder
    {
        private const string IMAGES_PATH = "Images";

        private const string ACCEPTED_ANSWER_ICON_FILENAME = "accepted_answer.png";

        private const string NOT_ACCEPTED_ICON_FILENAME = "so.png";

        private readonly PluginInitContext _context;

        public QuestionResultBuilder(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Convert(IEnumerable<Question> questions)
        {
            return questions.Select(ConvertQuestionToResult).ToList();
        }

        private Result ConvertQuestionToResult(Question question)
        {
            return new Result
            {
                Title = question.Title,
                SubTitle = GetSubTitle(question),
                IcoPath = GetIcon(question),
                Action = context =>
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
                        _context.API.ShowMsg($"Cannot open link {link}");
                        return false;
                    }
                }
            };
        }

        private static string GetIcon(Question question)
        {
            var icon = question.IsAnswered ? ACCEPTED_ANSWER_ICON_FILENAME : NOT_ACCEPTED_ICON_FILENAME;
            return $"{IMAGES_PATH}/{icon}";
        }

        private static string GetSubTitle(Question question)
        {
            return $"Answers count: {question.AnswerCount}. Tags: {string.Join(" ,", question.Tags)}";
        }
    }
}