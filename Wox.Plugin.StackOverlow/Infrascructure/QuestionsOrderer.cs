using System.Collections.Generic;
using System.Linq;
using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public class QuestionsOrderer : IQuestionsOrderer
    {
        public IOrderedEnumerable<Question> GetOrderedQuestions(List<Question> questions)
        {
            return questions
                .OrderByDescending(i => i.IsAnswered)
                .ThenByDescending(i => i.Score);
        }
    }
}