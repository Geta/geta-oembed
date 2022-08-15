// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.ContentApi.Core.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Geta.OEmbed.Optimizely.ContentDeliveryApi.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddGetaOEmbedContentDeliveryApi(this IServiceCollection services)
        {
            services.AddSingleton<IPropertyConverterProvider, OEmbedPropertyConverterProvider>();
            return services;
        }
    }
}