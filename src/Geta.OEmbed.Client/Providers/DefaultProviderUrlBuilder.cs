// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Web;

namespace Geta.OEmbed.Client.Providers
{
    public class DefaultProviderUrlBuilder : IProviderUrlBuilder
    {
        public virtual bool CanBuild(IOEmbedProvider oEmbedProvider)
        {
            return true;
        }

        public virtual string Build(string url, IOEmbedProvider oEmbedProvider, OEmbedOptions embedOptions)
        {
            var providerOEmbedUrl = GetOEmbedUrl(oEmbedProvider);
            var parameters = CreateParameters(embedOptions);

            return FormatOEmbedUrl(url, providerOEmbedUrl, parameters);
        }

        protected virtual string FormatOEmbedUrl(string url, string providerEmbedUrl, IDictionary<string, string> parameters)
        {
            var urlParameters = string.Join("&", parameters.Select(o => $"{o.Key}={o.Value}"));
            return $"{providerEmbedUrl}?url={HttpUtility.UrlEncode(url)}&{urlParameters}";
        }

        protected virtual IDictionary<string, string> CreateParameters(OEmbedOptions options)
        {
            var parameters = new Dictionary<string, string>
            {
                { nameof(OEmbedOptions.Autoplay).ToLower(), options.Autoplay.ToString().ToLower() },
                { nameof(OEmbedOptions.Controls).ToLower(), options.Controls.ToString().ToLower() },
                { nameof(OEmbedOptions.Loop).ToLower(), options.Loop.ToString().ToLower() },
                { nameof(OEmbedOptions.Muted).ToLower(), options.Muted.ToString().ToLower() },
            };

            if (options.Width.HasValue)
            {
                parameters.Add(nameof(OEmbedOptions.Width).ToLower(), options.Width.Value.ToString());
            }

            if (options.Height.HasValue)
            {
                parameters.Add(nameof(OEmbedOptions.Height).ToLower(), options.Height.Value.ToString());
            }

            if (options.MaxWidth.HasValue)
            {
                parameters.Add(nameof(OEmbedOptions.MaxWidth).ToLower(), options.MaxWidth.Value.ToString());
            }

            if (options.MaxHeight.HasValue)
            {
                parameters.Add(nameof(OEmbedOptions.MaxHeight).ToLower(), options.MaxHeight.Value.ToString());
            }

            return parameters;
        }

        protected virtual string GetOEmbedUrl(IOEmbedProvider oEmbedProvider)
        {
            return FormatEndpointUrl(oEmbedProvider.Endpoints.First().Url);
        }

        protected virtual string FormatEndpointUrl(string url)
        {
            return url.Replace("{format}", "json");
        }
    }
}
