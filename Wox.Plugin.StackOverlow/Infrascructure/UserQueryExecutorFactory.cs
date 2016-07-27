using Wox.Plugin.StackOverlow.Infrascructure.Api;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public static class UserQueryExecutorFactory
    {
        public static UserQueryExecutor Create(PluginInitContext context)
        {
            var stackOverflowApi = new StackOverflowApi();
            var questionResultBuilder = new QuestionResultBuilder(context);
            var questionsOrderer = new QuestionsOrderer();

            return new UserQueryExecutor(stackOverflowApi, questionResultBuilder, questionsOrderer);
        }
    }
}