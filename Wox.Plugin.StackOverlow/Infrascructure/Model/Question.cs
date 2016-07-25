using Newtonsoft.Json;

namespace Wox.Plugin.StackOverlow.Infrascructure.Model
{
    /// <summary>
    /// http://api.stackexchange.com/docs/types/question
    /// </summary>
    [JsonObject]
    public class Question
    {
        [JsonProperty("answer_count")]
        public int AnswerCount { get; internal set; }

        [JsonProperty("is_answered")]
        public bool IsAnswered { get; internal set; }

        [JsonProperty("link")]
        public string Link { get; internal set; }

        [JsonProperty("score")]
        public int Score { get; internal set; }

        [JsonProperty("tags")]
        public string[] Tags { get; internal set; }

        [JsonProperty("title")]
        public string Title { get; internal set; }
    }
}