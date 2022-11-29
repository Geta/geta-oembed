using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Geta.OEmbed.Optimizely.Models;

namespace Geta.OEmbed.Optimizely.Tests.Fakes
{
    public class MemoryUrlResolver : IUrlResolver
    {
        public Dictionary<string, IOEmbedMedia> _content;

        public MemoryUrlResolver()
        {
            _content = new Dictionary<string, IOEmbedMedia>(StringComparer.OrdinalIgnoreCase);
        }

        public void Add(string url, IOEmbedMedia media)
        {
            _content.Add(url, media);
        }

        public void Clear()
        {
            _content.Clear();
        }


        public string GetUrl(ContentReference contentLink, string language, UrlResolverArguments urlResolverArguments)
        {
            throw new NotImplementedException();
        }

        public string GetUrl(UrlBuilder urlBuilderWithInternalUrl, UrlResolverArguments arguments)
        {
            return urlBuilderWithInternalUrl.ToString();
        }

        public ContentRouteData? Route(UrlBuilder urlBuilder, RouteArguments routeArguments)
        {
            var url = urlBuilder.ToString();
            if (_content.TryGetValue(url, out var content))
                return new ContentRouteData(content, null, null, null, null);

            return null;
        }

        public bool TryToPermanent(string url, out string permanentUrl)
        {
            throw new NotImplementedException();
        }
    }
}
