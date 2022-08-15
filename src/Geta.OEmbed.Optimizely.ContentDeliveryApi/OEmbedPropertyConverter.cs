// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;
using Geta.OEmbed.Optimizely.Editing;
using Geta.OEmbed.Optimizely.Models;

namespace Geta.OEmbed.Optimizely.ContentDeliveryApi
{
    public class OEmbedPropertyConverter : IPropertyConverter
    {
        public IPropertyModel? Convert(PropertyData propertyData, ConverterContext contentMappingContext)
        {
            if (propertyData is PropertyOEmbed oEmbedPropertyData)
            {
                return new OEmbedPropertyModel(oEmbedPropertyData);
            }

            return null;
        }
    }

    public class OEmbedPropertyModel : PropertyModel<OEmbedModel?, PropertyOEmbed>
    {
        public OEmbedPropertyModel(PropertyOEmbed type) : base(type)
        {
            Value = type?.DeserializedValue;
        }
    }
}