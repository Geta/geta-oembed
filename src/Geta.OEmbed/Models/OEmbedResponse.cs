// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Geta.OEmbed.Models
{
    public class OEmbedResponse
    {
        [JsonPropertyName("version")]
        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("width")]
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonPropertyName("title")]
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("author_name")]
        [JsonProperty("author_name")]
        public string AuthorName { get; set; } = string.Empty;

        [JsonPropertyName("author_url")]
        [JsonProperty("author_url")]
        public string AuthorUrl { get; set; } = string.Empty;

        [JsonPropertyName("provider_name")]
        [JsonProperty("provider_name")]
        public string ProviderName { get; set; } = string.Empty;

        [JsonPropertyName("provider_url")]
        [JsonProperty("provider_url")]
        public string ProviderUrl { get; set; } = string.Empty;

        [JsonPropertyName("html")]
        [JsonProperty("html")]
        public string Html { get; set; } = string.Empty;

        [JsonPropertyName("thumbnail_width")]
        [JsonProperty("thumbnail_width")]
        public int ThumbnailWidth { get; set; }
        
        [JsonPropertyName("thumbnail_height")]
        [JsonProperty("thumbnail_height")]
        public int ThumbnailHeight { get; set; }
        
        [JsonPropertyName("thumbnail_url")]
        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; } = string.Empty;
    }
}