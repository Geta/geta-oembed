// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Models;
using Geta.OEmbed.Providers;
using Geta.OEmbed.Tests.Common.Factories;
using RichardSzalay.MockHttp;

namespace Geta.OEmbed.Tests
{
    public class OEmbedServiceTests
    {
        private readonly IHttpClientFactory _providerClientFactory;
        private readonly IHttpClientFactory _endpointClientFactory;

        public OEmbedServiceTests()
        {
            _providerClientFactory = CreateHttpClientFactory(() => GetProviderMessageHandler());
            _endpointClientFactory = CreateHttpClientFactory(() => GetEndpointMessageHandler());
        }

        [Fact]
        public async Task OEmbedService_can_GetAsync()
        {
            var subject = CreateService();
            var url = "https://www.youtube.com/watch?v=HmZKgaHa3Fg";
            var options = new OEmbedOptions
            {
                Autoplay = true,
                Muted = true,
                Loop = true
            };

            void Validate(OEmbedResponse? response)
            {
                Assert.NotNull(response);

                if (response is null)
                    throw new InvalidOperationException("response cannot be null here");

                Assert.Equal("HD Video (1080p) with Relaxing Music of Native American Shamans", response.Title);
                Assert.Equal("LoungeV Films - Relaxing Music and Nature Sounds", response.AuthorName);
                Assert.Equal("https://www.youtube.com/c/LoungeVstudioRelaxingMusicNatureSounds4KultraHD", response.AuthorUrl);
                Assert.Equal("video", response.Type);
                Assert.Equal(113, response.Height);
                Assert.Equal(200, response.Width);
                Assert.Equal("1.0", response.Version);
                Assert.Equal("YouTube", response.ProviderName);
                Assert.Equal("https://www.youtube.com/", response.ProviderUrl);
                Assert.Equal(360, response.ThumbnailHeight);
                Assert.Equal(480, response.ThumbnailWidth);
                Assert.Equal("https://i.ytimg.com/vi/HmZKgaHa3Fg/hqdefault.jpg", response.ThumbnailUrl);

                Assert.False(string.IsNullOrEmpty(response.Html));
                Assert.Contains("iframe", response.Html);
            }

            var response = await subject.GetAsync(url);
            Validate(response);

            response = await subject.GetAsync(url, CancellationToken.None);
            Validate(response);

            response = await subject.GetAsync(url, options);
            Validate(response);

            response = await subject.GetAsync(url, options, CancellationToken.None);
            Validate(response);
        }

        private OEmbedService CreateService()
        {
            var manifestLoader = new HttpClientManifestLoader(_providerClientFactory);
            var repository = new OEmbedProviderRepository(manifestLoader);
            return new OEmbedService(repository, _endpointClientFactory);
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
