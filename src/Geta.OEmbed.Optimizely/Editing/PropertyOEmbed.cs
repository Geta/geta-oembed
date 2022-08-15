// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.PlugIn;
using Geta.OEmbed.Optimizely.Cms;
using Geta.OEmbed.Optimizely.Models;

namespace Geta.OEmbed.Optimizely.Editing
{
    [PropertyDefinitionTypePlugIn(GUID = "030f6dff-9a43-4ad3-aa21-ed219968be34")]
    public class PropertyOEmbed : PropertyJsonBase<OEmbedModel>
    {
    }
}