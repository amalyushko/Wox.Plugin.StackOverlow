using Wox.Plugin.StackOverlow.Infrascructure.Api;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public static class UserQueryExecutorFactory
    {
        public static UserQueryExecutor Create(PluginInitContext context)
        {
            var deserializer = new JsonNetDeserializer();
            var stackOverflowApi = new StackOverflowApi(deserializer);
            var orderStrategy = new ByAnsweredAndScoreQuestionsOrderStrategy();

            var questionResultBuilder = new QuestionResultBuilder(context.API, orderStrategy);

            return new UserQueryExecutor(stackOverflowApi, questionResultBuilder);
        }
    }
}