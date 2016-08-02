using NUnit.Framework;
using Wox.Plugin.StackOverlow.Infrascructure;

namespace Wox.Plugin.StackOverlow.Tests.QuestionsOrderStrategy
{
    [TestFixture]
    public class ByAnsweredAndScoreBaseQuestionsOrderStrategyTests : BaseQuestionsOrderStrategyTests
    {
        public override IQuestionsOrderStrategy GetOrderStategyInstance()
        {
            return new ByAnsweredAndScoreQuestionsOrderStrategy();
        }
    }
}