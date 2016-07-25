using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wox.Plugin.StackOverlow.Infrascructure.Model
{
    public class QuestionResponse
    {
        [JsonProperty("items")]
        public List<Question> Items { get; set; }

        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        [JsonProperty("quota_max")]
        public int QuotaMax { get; set; }

        [JsonProperty("quota_remaining")]
        public int QuotaRemaining { get; set; }
    }
}