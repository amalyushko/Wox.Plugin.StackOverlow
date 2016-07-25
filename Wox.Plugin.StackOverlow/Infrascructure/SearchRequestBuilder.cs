using Wox.Plugin.StackOverlow.Infrascructure.Model;

namespace Wox.Plugin.StackOverlow.Infrascructure
{
    public static class SearchRequestBuilder
    {
        public static SearchRequest Parse(string rawQuery)
        {
            return new SearchRequest
            {
                Query = rawQuery,
            };
        }
    }
}