// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Client.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Geta.OEmbed.Client.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGetaOEmbed(this IServiceCollection services, Action<OEmbedConfiguration>? configure = null)
        {
            var configuration = new OEmbedConfiguration();

            if (configure is not null)
                configure(configuration);

            services.TryAddSingleton(configuration);
            services.TryAddEnumerable(new ServiceDescriptor(typeof(IProviderUrlBuilder), typeof(DefaultProviderUrlBuilder), ServiceLifetime.Singleton));
            services.TryAddEnumerable(new ServiceDescriptor(typeof(IProviderResponseFormatter), typeof(YouTubeVideoResponseFormatter), ServiceLifetime.Singleton));

            services.AddHttpClient<HttpClientManifestLoader>();
            services.AddHttpClient<OEmbedService>();
            services.AddHttpClient<IProviderManifestLoader, HttpClientManifestLoader>();
            services.AddHttpClient<IOEmbedService, OEmbedService>();

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