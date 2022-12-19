// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Core;
using EPiServer.Framework.Blobs;
using Geta.OEmbed.Optimizely.Models;

namespace Geta.OEmbed.Optimizely.Tests.Memory
{
    public class TestOEmbedMedia : IOEmbedMedia
    {
        public string Title => Name;
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;

        public ContentReference? ContentLink { get; set; }
        public ContentReference? ParentLink { get; set; }
        public Guid ContentGuid { get; set; }
        public int ContentTypeID { get; set; }
        public bool IsDeleted { get; set; }

        public PropertyDataCollection Property { get; } = new();
        public Blob? BinaryData { get; set; }
        public Blob? Thumbnail { get; set; }
        public Uri? BinaryDataContainer { get; set; }
    }
}
