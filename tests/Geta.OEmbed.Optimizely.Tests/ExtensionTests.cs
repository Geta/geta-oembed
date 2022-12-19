// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Framework.Cache;
using EPiServer.Web;
using Geta.OEmbed.Client;
using Geta.OEmbed.Client.DependencyInjection;
using Geta.OEmbed.Client.Providers;
using Geta.OEmbed.Optimizely.DependencyInjection;
using Geta.OEmbed.Optimizely.Tests.Memory;
using Geta.OEmbed.Tests.Common.Caching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Geta.OEmbed.Optimizely.Tests
{
    public class ExtensionTests
    {
        [Fact]
        public void DependencyInjectionExtensions_can_AddGetaOEmbedOptimizely()
        {
            var serviceCollection = new ServiceCollection();

            var instanceCache = new MemoryInstanceCache();
            var syncronizedCache = new FakeSynchronizedObjectInstanceCache(instanceCache);
            var siteDefinitionRepository = new MemorySiteDefinitionRepository(Enumerable.Empty<SiteDefinition>());

            serviceCollection.AddSingleton<ISynchronizedObjectInstanceCache>(syncronizedCache);
            serviceCollection.AddSingleton<ISiteDefinitionRepository>(siteDefinitionRepository);

            serviceCollection.AddGetaOEmbed();
            serviceCollection.AddGetaOEmbedOptimizely();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var embedRepository = serviceProvider.GetService<IOEmbedProviderRepository>();
            var embedService = serviceProvider.GetService<IOEmbedService>();
            var responseFormatters = serviceProvider.GetService<IEnumerable<IProviderResponseFormatter>>();
            var providerUrlBuilders = serviceProvider.GetService<IEnumerable<IProviderUrlBuilder>>();

            Assert.NotNull(embedRepository);
            Assert.NotNull(embedService);
            Assert.NotNull(responseFormatters);
            Assert.NotNull(providerUrlBuilders);
            Assert.True(responseFormatters?.Any());
            Assert.True(providerUrlBuilders?.Any());
        }

        [Fact]
        public void DependencyInjectionExtensions_can_MapOEmbed()
        {
            var options = Options.Create(new UIOptions());
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddControllers();
            builder.Services.AddRouting();

            var app = builder.Build();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapOEmbed(options);

                Assert.Single(endpoints.DataSources);

                var dataSource = endpoints.DataSources.FirstOrDefault();
                
                Assert.NotNull(dataSource);

                if (dataSource is null)
                    throw new InvalidOperationException("dataSource cannot be null here");

                foreach (var endpoint in dataSource.Endpoints)
                {
                    if (endpoint is not RouteEndpoint route)
                        continue;

                    Assert.EndsWith("oembed", route.RoutePattern.RawText);
                }
            });
        }
    }
}
