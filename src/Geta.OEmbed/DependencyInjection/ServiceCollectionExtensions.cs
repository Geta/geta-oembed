// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Geta.OEmbed.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGetaOEmbed(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.TryAddSingleton<HttpClientManifestLoader>();
            services.TryAddSingleton<OEmbedProviderRepository>();
            services.TryAddSingleton<OEmbedService>();
            services.TryAddSingleton<IProviderManifestLoader, HttpClientManifestLoader>();
            services.TryAddSingleton<IOEmbedProviderRepository, OEmbedProviderRepository>();
            services.TryAddSingleton<IOEmbedService, OEmbedService>();

            return services;
        }
    }
}