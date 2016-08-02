namespace Wox.Plugin.StackOverlow.Infrascructure.Api
{
    public interface IDeserializer
    {
        T Deserialize<T>(string rawString);
    }
}