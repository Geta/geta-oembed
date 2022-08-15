// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.OEmbed.Optimizely.Models
{
    public class ResponsiveOEmbedModel
    {
        public ResponsiveOEmbedModel(OEmbedModel baseModel)
        {
            OEmbed = baseModel;
        }

        public int? ScreenMinWidth { get; set; }
        public int? ScreenMaxWidth { get; set; }
        public OEmbedModel OEmbed { get; }
    }
}