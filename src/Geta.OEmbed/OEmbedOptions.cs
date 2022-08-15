// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.OEmbed
{
    public class OEmbedOptions
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? MaxWidth { get; set; }
        public int? MaxHeight { get; set; }
        public bool Autoplay { get; set; }
        public bool Controls { get; set; } = true;
        public bool Loop { get; set; }
        public bool Muted { get; set; } = true;

        public override string ToString()
        {
            var parameters = new Dictionary<string, string>
            {
                { nameof(Autoplay).ToLower(), Autoplay.ToString().ToLower() },
                { nameof(Controls).ToLower(), Controls.ToString().ToLower() },
                { nameof(Loop).ToLower(), Loop.ToString().ToLower() },
                { nameof(Muted).ToLower(), Muted.ToString().ToLower()},
                { "mute", Muted.ToString().ToLower()},
            };

            if (Width.HasValue)
            {
                parameters.Add(nameof(Width).ToLower(), Width.Value.ToString());
            }

            if (Height.HasValue)
            {
                parameters.Add(nameof(Height).ToLower(), Height.Value.ToString());
            }

            if (MaxWidth.HasValue)
            {
                parameters.Add(nameof(MaxWidth).ToLower(), MaxWidth.Value.ToString());
            }

            if (MaxHeight.HasValue)
            {
                parameters.Add(nameof(MaxHeight).ToLower(), MaxHeight.Value.ToString());
            }

            return string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
        }
    }
}