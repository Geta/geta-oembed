// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Text.Json.Serialization;

namespace Geta.OEmbed.Models
{
    public class OEmbedProvider : IOEmbedProvider
    {
        [JsonPropertyName("provider_name")]
        public virtual string ProviderName { get; set; } = string.Empty;

        [JsonPropertyName("provider_url")]
        public virtual string ProviderUrl { get; set; } = string.Empty;

        [JsonPropertyName("endpoints")]
        public virtual List<OEmbedEndpoint> Endpoints { get; set; } = new List<OEmbedEndpoint>();
       
    }
}