using Wox.Plugin.StackOverlow.Infrascructure.Api;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public static class UserQueryExecutorFactory
    {
        public static UserQueryExecutor Create(PluginInitContext context)
        {
            var stackOverflowApi = new StackOverflowApi();
            var questionResultBuilder = new QuestionResultBuilder(context.API);
            var questionsOrderer = new ByAnsweredAndScoreQuestionsOrderStrategy();

            return new UserQueryExecutor(stackOverflowApi, questionResultBuilder, questionsOrderer);
        }
    }
}