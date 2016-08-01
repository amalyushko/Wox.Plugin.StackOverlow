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
        public int AnswerCount { get; set; }

        [JsonProperty("is_answered")]
        public bool IsAnswered { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}