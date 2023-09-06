using Geta.OEmbed.Client;
using Geta.OEmbed.Client.Models;
using Geta.OEmbed.Client.Providers;

namespace Geta.OEmbed.Tests
{
    public class YouTubeVideoResponseFormatterTests
    {
        [Fact]
        public void YouTubeVideoResponseFormatter_can_FormatResponse()
        {
            var id = "OeNveeRydsA";
            var url = $"https://www.youtube.com/watch?v={id}";
            var iframe = $"<iframe src=\"https://www.youtube.com/embed/{id}?feature=oembed\"></iframe>";

            var response = new OEmbedResponse
            {
                Url = url,
                Html = iframe
            };
            var options = new OEmbedOptions
            {
                Autoplay = true,
                Loop = true,
            };

            var subject = new YouTubeVideoResponseFormatter();
            var formattedResponse = subject.FormatResponse(response, options);

            Assert.NotNull(formattedResponse);
            Assert.Contains("loop=1", formattedResponse.Html);
            Assert.Contains("autoplay=1", formattedResponse.Html);
            Assert.Contains($"playlist={id}", formattedResponse.Html);
        }
    }
}
