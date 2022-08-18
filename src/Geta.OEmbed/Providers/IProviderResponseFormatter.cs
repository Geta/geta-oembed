using Geta.OEmbed.Models;

namespace Geta.OEmbed.Providers
{
    public interface IProviderResponseFormatter
    {
        bool CanFormat(IOEmbedProvider oEmbedProvider, OEmbedResponse oEmbedResponse);

        OEmbedResponse FormatResponse(OEmbedResponse oEmbedResponse, OEmbedOptions options);
    }
}
