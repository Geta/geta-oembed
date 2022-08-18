namespace Geta.OEmbed.Extensions
{
    public static class StringExtensions
    {
        public static string AddParameters(this string url, IDictionary<string, string> parameters)
        {
            var query = string.Join("&amp;", parameters.Select(o => $"{o.Key}={o.Value}"));

            return url.IndexOf("?", StringComparison.Ordinal) > -1
                ? $"{url}&{query}"
                : $"{url}?{query}";
        }
    }
}
