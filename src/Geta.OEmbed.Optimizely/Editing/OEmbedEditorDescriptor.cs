// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Cms.Shell.UI.UIDescriptors;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using Geta.OEmbed.Optimizely.Models;

namespace Geta.OEmbed.Optimizely.Editing
{
    [EditorDescriptorRegistration(TargetType = typeof(OEmbedModel))]
    public class OEmbedEditorDescriptor : UrlEditorDescriptor
    {
        public OEmbedEditorDescriptor()
        {
            AllowedTypes = new[] { typeof(IContentImage), typeof(IContentVideo) };
            ClientEditingClass = "geta-oembed/OEmbedEditor";
        }

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);

            metadata.EditorConfiguration["endpointRoute"] = "/oembed";
            metadata.EditorConfiguration["repositoryKey"] = MediaRepositoryDescriptor.RepositoryKey;

            var typeName = typeof(OEmbedLinkModel).FullName;
            if (typeName is null)
            {
                return;
            }

            metadata.AdditionalValues["modelType"] = typeName;
        }
    }
}