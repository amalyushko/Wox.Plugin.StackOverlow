using Moq;
using NUnit.Framework;
using Wox.Plugin.StackOverlow.Infrascructure;
using Wox.Plugin.StackOverlow.Infrascructure.Api;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Tests
{
    [TestFixture]
    public class UserQueryExecutorTests
    {
        private Infrascructure.UserQueryExecutor GetUserQueryExecutor(IStackOverflowApi api)
        {
            var sortingStrategy = new ByAnsweredAndScoreQuestionsOrderStrategy();
            var woxPublicApiMock = new Mock<IPublicAPI>();
            woxPublicApiMock.Setup(p => p.GetTranslation(It.IsAny<string>())).Returns("mock");

            var questionResultBuilder = new QuestionResultBuilder(woxPublicApiMock.Object, sortingStrategy);
            return new Infrascructure.UserQueryExecutor(api, questionResultBuilder);
        }

        private SearchRequest GetSearchRequest(string query)
        {
            return new SearchRequest
            {
                Query = query
            };
        }

        private Mock<IStackOverflowApi> GetApiMock(Response response = null)
        {
            var api = new Mock<IStackOverflowApi>();
            api.Setup(a => a.GetQuestions(It.IsAny<SearchRequest>())).Returns(response ?? new NoConnectionResponse());

            return api;
        }

        [Test]
        public void Execute_EmptySearchQuery_NoRequestToServer()
        {
            var apiMock = GetApiMock();
            var executor = GetUserQueryExecutor(apiMock.Object);

            executor.Execute(GetSearchRequest(string.Empty));

            apiMock.Verify(a => a.GetQuestions(It.IsAny<SearchRequest>()), Times.Never);
        }
    }
}   