using Wox.Plugin.StackOverlow.Infrascructure.Api;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public static class UserQueryExecutorFactory
    {
        public static UserQueryExecutor Create(PluginInitContext context)
        {
            var deserializer = new JsonNetDeserializer();
            var stackOverflowApi = new StackOverflowApi(deserializer);
            var questionResultBuilder = new QuestionResultBuilder(context.API);
            var questionsOrderer = new ByAnsweredAndScoreQuestionsOrderStrategy();

            return new UserQueryExecutor(stackOverflowApi, questionResultBuilder, questionsOrderer);
        }
    }
}