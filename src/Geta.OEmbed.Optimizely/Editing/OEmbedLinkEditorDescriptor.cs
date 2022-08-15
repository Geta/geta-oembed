// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Collections;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using Geta.OEmbed.Optimizely.Models;

namespace Geta.OEmbed.Optimizely.Editing
{
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = "HyperLink", EditorDescriptorBehavior = EditorDescriptorBehavior.PlaceLast)]
    public class OEmbedLinkEditorDescriptor : EditorDescriptor
    {
        private readonly string[] _excludedProviders = new[] { "Page", "Catalog content", "Email" };
        
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
                var name = GetProviderName(provider);

                if (_excludedProviders.Any(p => p.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    providers.RemoveAt(i);
                }
            }

            metadata.EditorConfiguration["providers"] = providers;
        }

        protected virtual string GetProviderName(object? provider)
        {
            if (provider is null)
            {
                return string.Empty;
            }               

            var providerType = provider.GetType();
            var nameProperty = providerType.GetProperty("Name");
            if (nameProperty?.GetValue(provider) is not string nameValue)
            {
                return string.Empty;
            }

            return nameValue;
        }
    }
}