// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Framework.Cache;
using EPiServer.Web;
using Geta.OEmbed.Optimizely.Caching;
using Geta.OEmbed.Optimizely.Tests.Common.Repositories;
using Geta.OEmbed.Providers;
using Geta.OEmbed.Tests.Common.Caching;
using Geta.OEmbed.Tests.Common.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace Geta.OEmbed.Optimizely.Tests
{
    public class CachingTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly MemoryInstanceCache _instanceCache;

        public CachingTests()
        {
            var serviceCollection = new ServiceCollection();

            var providerClientFactory = CreateHttpClientFactory(() => GetProviderMessageHandler());
            var endpointClientFactory = CreateHttpClientFactory(() => GetEndpointMessageHandler());

            var instanceCache = _instanceCache = new MemoryInstanceCache();
            var syncronizedCache = new FakeSynchronizedObjectInstanceCache(instanceCache);

            var manifestLoader = new HttpClientManifestLoader(providerClientFactory);

            var siteDefinitionRepository = new MemorySiteDefinitionRepository(new [] { SiteDefinition.Empty });
            var oembedProvider = new OptimizelyOEmbedProvider(siteDefinitionRepository, Options.Create(new UIOptions()));

            var baseRepository = new OEmbedProviderRepository(manifestLoader);
            var cachedRepository = new CachedOEmbedProviderRepository(baseRepository, syncronizedCache, oembedProvider);

            var baseEmbedService = new OEmbedService(cachedRepository, endpointClientFactory);
            var cachedEmbedService = new CachedOEmbedService(baseEmbedService, syncronizedCache);

            serviceCollection.AddSingleton<IObjectInstanceCache>(instanceCache);
            serviceCollection.AddSingleton<IOEmbedService>(cachedEmbedService);
            serviceCollection.AddSingleton<IOEmbedProviderRepository>(cachedRepository);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }


        [Fact]
        public async Task OEmbedProviderRepository_can_cache()
        {
            _instanceCache.Clear();

            var subject = _serviceProvider.GetRequiredService<IOEmbedProviderRepository>();
            
            await subject.ListAsync();

            Assert.Single(_instanceCache.Cache);

            var (cacheItem, policy) = _instanceCache.Cache.Values.FirstOrDefault();

            Assert.NotNull(cacheItem);
            Assert.NotNull(policy);

            Assert.True(policy.Expiration > TimeSpan.Zero);
        }

        [Fact]
        public async Task OEmbedService_can_cache()
        {
            _instanceCache.Clear();

            var subject = _serviceProvider.GetRequiredService<IOEmbedService>();
            var url = "https://www.youtube.com/watch?v=HmZKgaHa3Fg";

            await subject.GetAsync(url);

            Assert.Equal(2, _instanceCache.Cache.Count);

            var (cacheItem, policy) = _instanceCache.Cache.Values.LastOrDefault();

            Assert.NotNull(cacheItem);
            Assert.NotNull(policy);

            Assert.True(policy.Expiration > TimeSpan.Zero);
        }


        private static MockHttpClientFactory CreateHttpClientFactory(Func<MockHttpMessageHandler> handlerFactory)
        {
            return new MockHttpClientFactory(handlerFactory);
        }

        private static MockHttpMessageHandler GetProviderMessageHandler()
        {
            return GetHttpMessageHandler("https://oembed.com/providers.json", x => x.Respond("application/json", GetFileResponse("Resources/providers.json")));
        }

        private static MockHttpMessageHandler GetEndpointMessageHandler()
        {
            return GetHttpMessageHandler("https://www.youtube.com/oembed*", x => x.Respond("application/json", GetFileResponse("Resources/oembed-youtube.json")));
        }

        private static MockHttpMessageHandler GetHttpMessageHandler(string urlGlob, Action<MockedRequest> action)
        {
            var mockHttp = new MockHttpMessageHandler();
            var mockRequest = mockHttp.When(urlGlob);

            action(mockRequest);

            return mockHttp;
        }

        private static string GetFileResponse(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
