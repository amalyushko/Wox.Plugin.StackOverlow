using System.Collections.Generic;
using System.Linq;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public interface IQuestionsOrderStrategy
    {
        IOrderedEnumerable<Question> GetOrderedQuestions(List<Question> questions);
    }
}