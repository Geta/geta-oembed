// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Framework.Cache;
using EPiServer.Web;
using Geta.OEmbed.DependencyInjection;
using Geta.OEmbed.Optimizely.DependencyInjection;
using Geta.OEmbed.Tests.Common.Caching;
using Microsoft.AspNetCore.Builder;
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

            serviceCollection.AddSingleton<ISynchronizedObjectInstanceCache>(syncronizedCache);

            serviceCollection.AddGetaOEmbed();
            serviceCollection.AddGetaOEmbedOptimizely();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var embedRepository = serviceProvider.GetService<IOEmbedProviderRepository>();
            var embedService = serviceProvider.GetService<IOEmbedService>();

            Assert.NotNull(embedRepository);
            Assert.NotNull(embedService);
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

                Assert.Equal(2, dataSource.Endpoints.Count);
            });
        }
    }
}
