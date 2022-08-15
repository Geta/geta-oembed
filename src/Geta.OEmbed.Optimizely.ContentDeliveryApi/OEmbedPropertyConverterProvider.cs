// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.ContentApi.Core.Serialization;
using EPiServer.Core;
using Geta.OEmbed.Optimizely.Editing;

namespace Geta.OEmbed.Optimizely.ContentDeliveryApi
{
    public class OEmbedPropertyConverterProvider : IPropertyConverterProvider
    {
        public IPropertyConverter? Resolve(PropertyData propertyData)
        {
            return propertyData is PropertyOEmbed ? new OEmbedPropertyConverter() : null;
        }

        public int SortOrder => 100;
    }
}