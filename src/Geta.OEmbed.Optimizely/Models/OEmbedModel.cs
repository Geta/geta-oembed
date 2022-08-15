// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Models;
using Newtonsoft.Json;

namespace Geta.OEmbed.Optimizely.Models
{
    public class OEmbedModel : OEmbedResponse
    {
        [JsonProperty("requestedUrl")]
        public string RequestedUrl { get; set; } = string.Empty;
        
        [JsonProperty("autoplay")]
        public bool Autoplay { get; set; }
        
        [JsonProperty("enableControls")]
        public bool EnableControls { get; set; }
        
        [JsonProperty("loop")]
        public bool Loop { get; set; }
        
        [JsonProperty("muted")]
        public bool Muted { get; set; }
        
        [JsonProperty("maxWidth")]
        public int? MaxWidth { get; set; }
        
        [JsonProperty("maxHeight")]
        public int? MaxHeight { get; set; }
    }
}