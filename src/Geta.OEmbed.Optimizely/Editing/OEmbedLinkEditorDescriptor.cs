// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Collections;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using Geta.OEmbed.Optimizely.Models;

namespace Geta.OEmbed.Optimizely.Editing
{
    [EditorDescriptorRegistration(
        TargetType = typeof(string),
        UIHint = UiHint,
        EditorDescriptorBehavior = EditorDescriptorBehavior.PlaceLast)]
    public class OEmbedLinkEditorDescriptor : EditorDescriptor
    {
        public const string UiHint = "HyperLink";
        private const string ExternalLinkProviderName = "ExternalLink";
        private const string MediaSearchArea = "CMS/Files";

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);

            if (metadata.ContainerType != typeof(OEmbedLinkModel))
            {
                return;
            }

            if (metadata.EditorConfiguration["providers"] is not IList providers)
            {
                return;
            }

            for (var i = providers.Count - 1; i >= 0; i--)
            {
                var provider = providers[i];
                var (name, searchArea) = GetProviderInfo(provider);

                if (!name.Equals(ExternalLinkProviderName, StringComparison.OrdinalIgnoreCase)
                    && !searchArea.Equals(MediaSearchArea, StringComparison.OrdinalIgnoreCase))
                {
                    providers.RemoveAt(i);
                }
            }

            metadata.EditorConfiguration["providers"] = providers;
        }

        protected virtual (string, string) GetProviderInfo(object? provider)
        {
            if (provider is null)
            {
                return (string.Empty, string.Empty);
            }

            var providerType = provider.GetType();
            var nameProperty = providerType.GetProperty("Name");
            var searchAreaProperty = providerType.GetProperty("SearchArea");
            var nameValue = nameProperty?.GetValue(provider) as string ?? string.Empty;
            var searchAreaValue = searchAreaProperty?.GetValue(provider) as string ?? string.Empty;

            return (nameValue, searchAreaValue);
        }
    }
}
