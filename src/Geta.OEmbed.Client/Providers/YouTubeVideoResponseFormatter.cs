// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Client.Extensions;
using Geta.OEmbed.Client.Models;
using System.Text.RegularExpressions;

namespace Geta.OEmbed.Client.Providers
{
    public class YouTubeVideoResponseFormatter : IProviderResponseFormatter
    {
        private static readonly Regex SrcFilter = new("src=\"([a-z-0-9-_?=/:.]{1,})\"", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        private static readonly Regex IdFilter = new("/embed/([a-z0-9-_]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        public virtual bool CanFormat(IOEmbedProvider oEmbedProvider, OEmbedResponse oEmbedResponse)
        {
            if (oEmbedProvider.ProviderName != "YouTube")
            {
                return false;
            }

            if (oEmbedResponse.Type != "video")
            {
                return false;
            }

            return true;
        }

        public virtual OEmbedResponse FormatResponse(OEmbedResponse oEmbedResponse, OEmbedOptions options)
        {
            ParseVideoUrl(oEmbedResponse, options);

            return oEmbedResponse;
        }

        protected virtual void ParseVideoUrl(OEmbedResponse oEmbedResponse, OEmbedOptions options)
        {
            var urlMatch = SrcFilter.Match(oEmbedResponse.Html);
            if (!urlMatch.Success)
            {
                return;
            }

            var originalUrl = urlMatch.Groups[1].Value;
            var idMatch = IdFilter.Match(originalUrl);
            if (!idMatch.Success)
            {
                return;
            }

            var id = idMatch.Groups[1].Value;
            var parameters = CreateReplacementParameters(options, id);
            var urlWithOptions = originalUrl.AddParameters(parameters);

            oEmbedResponse.Html = oEmbedResponse.Html.Replace(originalUrl, urlWithOptions);
        }

        protected virtual IDictionary<string, string> CreateReplacementParameters(OEmbedOptions options, string id)
        {
            var parameters = new Dictionary<string, string>
            {
                { nameof(OEmbedOptions.Autoplay).ToLower(), options.Autoplay ? "1" : "0" },
                { nameof(OEmbedOptions.Controls).ToLower(), options.Controls ? "1" : "0" },
                { nameof(OEmbedOptions.Loop).ToLower(), options.Loop ? "1" : "0" },
                { nameof(OEmbedOptions.Muted).ToLower(), options.Muted ? "1" : "0" },
                { "mute", options.Muted ? "1" : "0" },
            };

            if (options.Loop)
            {
                parameters.Add("playlist", id);
            }

            return parameters;
        }
    }
}
