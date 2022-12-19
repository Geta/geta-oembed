// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Framework.Cache;
using EPiServer.Shell.Modules;
using EPiServer.Web;
using Geta.OEmbed.Client;
using Geta.OEmbed.Optimizely.Caching;
using Geta.OEmbed.Optimizely.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Geta.OEmbed.Optimizely.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddGetaOEmbedOptimizely(this IServiceCollection services)
        {
            services.TryAddScoped<OptimizelyOEmbedProvider>();
            services.TryAddScoped<IOptimizelyOEmbedHandler, OptimizelyOEmbedHandler>();
            services.AddSingleton<IOEmbedProviderRepository>(provider => new CachedOEmbedProviderRepository(
                provider.GetRequiredService<OEmbedProviderRepository>(),
                provider.GetRequiredService<ISynchronizedObjectInstanceCache>(),
                provider.GetRequiredService<OptimizelyOEmbedProvider>()));

            services.AddSingleton<OEmbedService>();
            services.AddSingleton<IOEmbedService>(provider =>
                new CachedOEmbedService(provider.GetRequiredService<OEmbedService>(),
                    provider.GetRequiredService<ISynchronizedObjectInstanceCache>()));
            
            services.Configure<ProtectedModuleOptions>(options =>
            {
                options.Items.Add(new ModuleDetails { Name = "Geta.OEmbed.Optimizely" });
            });

            return services;
        }

        public static ControllerActionEndpointConventionBuilder MapOEmbed(this IEndpointRouteBuilder endpoints, IOptions<UIOptions>? uiOptions = null)
        {
            uiOptions ??= endpoints.ServiceProvider.GetRequiredService<IOptions<UIOptions>>();

            var editUrl = uiOptions.Value.EditUrl.ToString();
            var pattern = $"{editUrl.TrimStart('~').TrimStart('/')}oembed";

            endpoints.MapControllerRoute("oembed", "oembed", new { controller = "OEmbed", action = "Get" });
            
            return endpoints
                .MapControllerRoute("oembedcms", pattern, new { controller = "OEmbedCms", action = "Index" });
        }
    }
}
