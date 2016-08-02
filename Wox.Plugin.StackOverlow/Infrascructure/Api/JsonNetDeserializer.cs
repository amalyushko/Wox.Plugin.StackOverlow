using Newtonsoft.Json;

namespace Wox.Plugin.StackOverlow.Infrascructure.Api
{
    public class JsonNetDeserializer: IDeserializer
    {
        public T Deserialize<T>(string rawString)
        {
            return JsonConvert.DeserializeObject<T>(rawString);
        }
    }
}