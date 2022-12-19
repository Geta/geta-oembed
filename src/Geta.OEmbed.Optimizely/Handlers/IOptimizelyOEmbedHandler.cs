using Geta.OEmbed.Client.Models;

namespace Geta.OEmbed.Optimizely.Handlers;

public interface IOptimizelyOEmbedHandler
{
    Task<OEmbedResponse?> HandleAsync(OEmbedRequest request, CancellationToken cancellationToken);
}
