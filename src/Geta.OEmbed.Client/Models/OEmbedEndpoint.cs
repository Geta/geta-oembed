// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Text.Json.Serialization;

namespace Geta.OEmbed.Client.Models
{
    public class OEmbedEndpoint
    {
        [JsonPropertyName("schemes")]
        public List<string> Schemes { get; set; } = new List<string>();

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("discovery")]
        public bool Discovery { get; set; }
    }
}