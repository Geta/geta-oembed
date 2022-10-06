// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Client;
using Geta.OEmbed.Client.Providers;
using Geta.OEmbed.Tests.Common.Factories;
using RichardSzalay.MockHttp;

namespace Geta.OEmbed.Tests
{
    public class OEmbedProviderRepositoryTests
    {
        private readonly IHttpClientFactory _clientFactory;

        public OEmbedProviderRepositoryTests()
        {
            _clientFactory = CreateHttpClientFactory(() => GetMessageHandler());
        }

        [Fact]
        public async Task OEmbedProviderRepository_can_ListAsync()
        {
            var subject = CreateRepository(_clientFactory);

            void Validate(IEnumerable<IOEmbedProvider>? providers)
            {
                Assert.NotNull(providers);

                if (providers is null)
                    throw new InvalidOperationException("providers cannot be null here");

                Assert.Equal(287, providers.Count());
            }

            var providers = await subject.ListAsync();
            Validate(providers);

            providers = await subject.ListAsync(CancellationToken.None);
            Validate(providers);
        }

        [Fact]
        public async Task OEmbedProviderRepository_can_FindByUrlAsync()
        {
            var subject = CreateRepository(_clientFactory);
            var url = "https://www.youtube.com/watch?v=HmZKgaHa3Fg";
            
            void Validate(IOEmbedProvider? provider)
            {
                Assert.NotNull(provider);

                if (provider is null)
                    throw new InvalidOperationException("provider cannot be null here");

                Assert.Equal("YouTube", provider.ProviderName);
                Assert.Equal("https://www.youtube.com/", provider.ProviderUrl);
                Assert.Single(provider.Endpoints);

                var endpoint = provider.Endpoints[0];
            
                Assert.Equal(6, endpoint.Schemes.Count);
                Assert.Equal("https://www.youtube.com/oembed", endpoint.Url);
                Assert.True(endpoint.Discovery);
            }

            var provider = await subject.FindByUrlAsync(url);
            Validate(provider);

            provider = await subject.FindByUrlAsync(url, CancellationToken.None);
            Validate(provider);

            var providers = await subject.ListAsync();

            provider = subject.FindByUrl(providers, url);
            Validate(provider);
        }

        private static OEmbedProviderRepository CreateRepository(IHttpClientFactory httpClientFactory)
        {
            var config = new OEmbedConfiguration();
            var manifestLoader = new HttpClientManifestLoader(config, httpClientFactory.CreateClient());
            return new OEmbedProviderRepository(manifestLoader);
        }

        private static MockHttpClientFactory CreateHttpClientFactory(Func<MockHttpMessageHandler> handlerFactory)
        {
            return new MockHttpClientFactory(handlerFactory);
        }

        private static MockHttpMessageHandler GetMessageHandler()
        {
            return GetMessageHandler(x => x.Respond("application/json", GetProviderResponse()));
        }

        private static MockHttpMessageHandler GetMessageHandler(Action<MockedRequest> action)
        {
            var mockHttp = new MockHttpMessageHandler();
            var mockRequest = mockHttp.When("https://oembed.com/providers.json");

            action(mockRequest);

            return mockHttp;
        }       

        private static string GetProviderResponse()
        {
            using var stream = new FileStream("Resources/providers.json", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
