namespace FinTech
{
    using Newtonsoft.Json.Linq;

    public static class JsonExtensions
    {
        public static object ValueByPath(this JObject source, string path)
        {
            var token = source.SelectToken(path);
            var value = (JValue) token;
            return value.Value;
        }
    }
}