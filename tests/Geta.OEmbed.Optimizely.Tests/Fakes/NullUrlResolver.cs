using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;

namespace Geta.OEmbed.Optimizely.Tests.Fakes
{
    public class NullUrlResolver : IUrlResolver
    {
        public string? GetUrl(ContentReference contentLink, string language, UrlResolverArguments urlResolverArguments)
        {
            return null;
        }

        public string? GetUrl(UrlBuilder urlBuilderWithInternalUrl, UrlResolverArguments arguments)
        {
            return null;
        }

        public ContentRouteData? Route(UrlBuilder urlBuilder, RouteArguments routeArguments)
        {
            return null;
        }

        public bool TryToPermanent(string url, out string? permanentUrl)
        {
            permanentUrl = null;
            return false;
        }
    }
}
