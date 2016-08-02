using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wox.Plugin.StackOverlow.Infrascructure;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Tests
{
    [TestFixture]
    public class QuestionResultBuilderTests
    {
        private QuestionResultBuilder GetResultBuilder()
        {
            var sortingStrategy = new ByAnsweredAndScoreQuestionsOrderStrategy();
            var woxPublicApiMock = new Mock<IPublicAPI>();
            woxPublicApiMock.Setup(p => p.GetTranslation(It.IsAny<string>())).Returns("mock");

            return new QuestionResultBuilder(woxPublicApiMock.Object, sortingStrategy);
        }

        [Test]
        public void ProcessServerResponse_NoConnectionResponse_ReturnErrorResult()
        {
            var resultBuilder = GetResultBuilder();

            var result = resultBuilder.ProcessServerResponse(new NoConnectionResponse());

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().IcoPath, Does.EndWith(IconPath.NOT_NETWORK_CONNECTION_ICON_FILENAME));
        }

        [Test]
        public void ProcessServerResponse_OneQuestion_OneQuestionResult()
        {
            var resultBuilder = GetResultBuilder();

            var answer = new Question
            {
                Title = "some title",
                Tags = new[] { "js", "git" }
            };
            var result = resultBuilder.ProcessServerResponse(new QuestionResponse
            {
                Items = new List<Question> { answer }
            });

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void ProcessServerResponse_QuestionWithoutTags_NoThrows()
        {
            var resultBuilder = GetResultBuilder();

            var answer = new Question
            {
                Title = "some title",
            };
            var response = new QuestionResponse
            {
                Items = new List<Question> { answer }
            };

            Assert.DoesNotThrow(() => resultBuilder.ProcessServerResponse(response));
        }

        [Test]
        public void ProcessServerResponse_CorrectTitle()
        {
            var resultBuilder = GetResultBuilder();
            const string questionTitle = "How to write unit tests?";
            var answer = new Question
            {
                Title = questionTitle,
            };
            var response = new QuestionResponse
            {
                Items = new List<Question> { answer }
            };

            var result = resultBuilder.ProcessServerResponse(response);

            Assert.That(result.First().Title, Is.EqualTo(questionTitle));
        }

        [Test]
        public void ProcessServerResponse_QuestionHasAcceptedAnswer_CorrectAcceptedAnswerIcon()
        {
            var resultBuilder = GetResultBuilder();
            var answer = new Question
            {
                Title = "some title",
                IsAnswered = true
            };
            var response = new QuestionResponse
            {
                Items = new List<Question> { answer }
            };

            var result = resultBuilder.ProcessServerResponse(response);

            Assert.That(result.First().IcoPath, Does.EndWith(IconPath.ACCEPTED_ANSWER_ICON_FILENAME));
        }

        [Test]
        public void ProcessServerResponse_QuestionHasNotAcceptedAnswer_CorrectNoAcceptedAnswerIcon()
        {
            var resultBuilder = GetResultBuilder();
            var answer = new Question
            {
                Title = "some title",
                IsAnswered = false
            };
            var response = new QuestionResponse
            {
                Items = new List<Question> { answer }
            };

            var result = resultBuilder.ProcessServerResponse(response);

            Assert.That(result.First().IcoPath, Does.EndWith(IconPath.NOT_ACCEPTED_ICON_FILENAME));
        }

        [Test]
        public void ProcessServerResponse_ErrorResponseWithMessage_ReturnsErrorResultWithMessage()
        {
            var resultBuilder = GetResultBuilder();
            const string customErrorMessage = "Some error";
            var response = new ErrorResponse(ResponseErrorType.BadRequest, customErrorMessage);

            var result = resultBuilder.ProcessServerResponse(response);

            Assert.That(result.First().Title, Is.EqualTo(customErrorMessage));
        }

        [Test]
        public void ProcessServerResponse_ErrorResponseWithouMessage_ReturnsErrorResultWithMessageFromResources()
        {
            var resultBuilder = GetResultBuilder();
            var response = new ErrorResponse(ResponseErrorType.BadRequest);

            var result = resultBuilder.ProcessServerResponse(response);

            Assert.That(result.First().Title, Does.Not.Empty);
        }
    }
}