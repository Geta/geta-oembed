// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Extensions;
using Geta.OEmbed.Models;
using System.Text.RegularExpressions;

namespace Geta.OEmbed.Providers
{
    public class YouTubeVideoResponseFormatter : IProviderResponseFormatter
    {
        private static readonly Regex _srcFilter = new("src=\"([a-z-0-9-_?=/:.]{1,})\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
            var match = _srcFilter.Match(oEmbedResponse.Html);
            if (!match.Success)
            {
                return;
            }

            var originalUrl = match.Groups[1].Value;
            var parameters = CreateReplacementParameters(options);
            var urlWithOptions = originalUrl.AddParameters(parameters);

            oEmbedResponse.Html = oEmbedResponse.Html.Replace(originalUrl, urlWithOptions);
        }

        protected virtual IDictionary<string, string> CreateReplacementParameters(OEmbedOptions options)
        {
            return new Dictionary<string, string>
            {
                { nameof(OEmbedOptions.Autoplay).ToLower(), options.Autoplay ? "1" : "0" },
                { nameof(OEmbedOptions.Controls).ToLower(), options.Controls ? "1" : "0" },
                { nameof(OEmbedOptions.Loop).ToLower(), options.Loop ? "1" : "0" },
                { nameof(OEmbedOptions.Muted).ToLower(), options.Muted ? "1" : "0" },
                { "mute", options.Muted ? "1" : "0" },
            };
        }
    }
}
