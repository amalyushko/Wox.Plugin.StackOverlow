using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Infrascructure.Api
{
    public interface IStackOverflowApi
    {
        Response GetQuestions(SearchRequest request);
    }
}