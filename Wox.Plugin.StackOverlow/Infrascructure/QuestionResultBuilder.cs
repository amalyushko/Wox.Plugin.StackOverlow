using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public class QuestionResultBuilder
    {
        private const string IMAGES_PATH = "Images";

        private readonly IPublicAPI _woxPluginApi;

        private readonly IQuestionsOrderStrategy _questionsOrderStrategy;

        public QuestionResultBuilder(IPublicAPI woxPluginApi, IQuestionsOrderStrategy questionsOrderStrategy)
        {
            if (woxPluginApi == null) throw new ArgumentNullException(nameof(woxPluginApi));
            if (questionsOrderStrategy == null) throw new ArgumentNullException(nameof(questionsOrderStrategy));

            _woxPluginApi = woxPluginApi;
            _questionsOrderStrategy = questionsOrderStrategy;
        }

        public List<Result> ProcessServerResponse(Response response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            var questionsResponse = response as QuestionResponse;
            if (questionsResponse != null)
            {
                var orderedQuestions = _questionsOrderStrategy.GetOrderedQuestions(questionsResponse.Items);
                return ConvertQuestions(orderedQuestions);
            }

            var noConnectionResponse = response as NoConnectionResponse;
            if (noConnectionResponse != null)
            {
                return new List<Result> { ConvertToNoConnectionResult() };
            }

            var requestErrorResponse = response as ErrorResponse;
            if (requestErrorResponse != null)
            {
                return new List<Result> { ConvertToRequestErrorResult(requestErrorResponse) };
            }

            // for now just return empty list
            return new List<Result>();
        }

        private List<Result> ConvertQuestions(IEnumerable<Question> questions)
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
                        _woxPluginApi.ShowMsg(_woxPluginApi.GetTranslation("wox_plugin_so_results_error_open_link"));
                        return false;
                    }
                }
            };
        }

        private Result ConvertToNoConnectionResult()
        {
            return new Result
            {
                Title = _woxPluginApi.GetTranslation("wox_plugin_so_results_no_network_connection"),
                IcoPath = GetIcon(IconPath.NOT_NETWORK_CONNECTION_ICON_FILENAME)
            };
        }

        private string GetIcon(Question question)
        {
            var icon = question.IsAnswered ? IconPath.ACCEPTED_ANSWER_ICON_FILENAME : IconPath.NOT_ACCEPTED_ICON_FILENAME;
            return GetIcon(icon);
        }

        private static string GetIcon(string iconName)
        {
            return $"{IMAGES_PATH}/{iconName}";
        }

        private string GetSubTitle(Question question)
        {
            var sb = new StringBuilder();
            var answersCountLiteral = string.Format(_woxPluginApi.GetTranslation("wox_plugin_so_results_answers_count"), question.AnswerCount);
            sb.AppendFormat("{0}.", answersCountLiteral);

            if (question.Tags != null && question.Tags.Any())
            {
                var tagsLiteral = string.Format(_woxPluginApi.GetTranslation("wox_plugin_so_results_tags"), string.Join(" ,", question.Tags));
                sb.AppendFormat(" {0}.", tagsLiteral);
            }

            return sb.ToString();
        }

        private Result ConvertToRequestErrorResult(ErrorResponse errorResponse)
        {
            var title = string.IsNullOrEmpty(errorResponse.Message)
                ? _woxPluginApi.GetTranslation("wox_plugin_so_results_error_response")
                : errorResponse.Message;

            return new Result
            {
                Title = title
            };
        }
    }
}