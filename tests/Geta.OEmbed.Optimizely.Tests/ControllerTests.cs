// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Web;
using EPiServer.Web.Routing;
using Geta.OEmbed.AspNetCore.Mvc;
using Geta.OEmbed.Client;
using Geta.OEmbed.Client.Models;
using Geta.OEmbed.Client.Providers;
using Geta.OEmbed.Optimizely.Handlers;
using Geta.OEmbed.Optimizely.Tests.Fakes;
using Geta.OEmbed.Optimizely.Tests.Memory;
using Geta.OEmbed.Tests.Common.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace Geta.OEmbed.Optimizely.Tests
{
    public class ControllerTests : IDisposable
    {      
        private readonly ServiceProvider _serviceProvider;
        private readonly MemoryUrlResolver _memoryUrlResolver;

        public ControllerTests()
        {
            var serviceCollection = new ServiceCollection();

            _memoryUrlResolver = new MemoryUrlResolver();

            var providerClientFactory = CreateHttpClientFactory(() => GetProviderMessageHandler());
            var endpointClientFactory = CreateHttpClientFactory(() => GetEndpointMessageHandler());

            var config = new OEmbedConfiguration();
            var manifestLoader = new HttpClientManifestLoader(config, providerClientFactory.CreateClient());
            var repository = new OEmbedProviderRepository(manifestLoader);
            var urlBuilders = new[] { new DefaultProviderUrlBuilder() };
            var embedFormatters = Enumerable.Empty<IProviderResponseFormatter>();

            var embedService = new OEmbedService(repository, urlBuilders, embedFormatters, endpointClientFactory.CreateClient());

            serviceCollection.AddSingleton<IOEmbedService>(embedService);
            serviceCollection.AddSingleton<IUrlResolver>(_memoryUrlResolver);
            serviceCollection.AddSingleton<IOptimizelyOEmbedHandler, OptimizelyOEmbedHandler>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task OEmbedController_can_Get()
        {
            var url = "https://www.youtube.com/watch?v=HmZKgaHa3Fg";
            var subject = ActivatorUtilities.CreateInstance<OEmbedController>(_serviceProvider);
            var request = new OEmbedRequest
            {
                Url = url,
                Autoplay = true,
                Loop = true,
                Muted = true
            };

            var result = await subject.Get(request, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(typeof(OkObjectResult), result!.GetType());

            var objectValue = ((OkObjectResult)result).Value;
            Assert.NotNull(objectValue);
            Assert.Equal(typeof(OEmbedResponse), objectValue!.GetType());
        }

        [Fact]
        public async Task CmsOEmbedController_can_Get()
        {
            var url = "https://localhost/globalassets/test.jpg";
            var subject = ActivatorUtilities.CreateInstance<OEmbedCmsController>(_serviceProvider);
            var request = new OEmbedRequest
            {
                Url = url,
                Autoplay = true,
                Loop = true,
                Muted = true
            };
            var expected = new TestOEmbedMedia
            {
                Name = "test.jpg",
                Width = 200,
                Height = 150                
            };

            _memoryUrlResolver.Clear();
            _memoryUrlResolver.Add(url, expected);

            var result = await subject.Index(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(typeof(OkObjectResult), result!.GetType());

            var objectValue = ((OkObjectResult)result).Value;
            Assert.NotNull(objectValue);
            Assert.Equal(typeof(OEmbedResponse), objectValue!.GetType());

            var response = (OEmbedResponse)objectValue;
            Assert.Equal(response.Width, expected.Width);
            Assert.Equal(response.Height, expected.Height);
            Assert.Equal(response.Title, expected.Name);

            request.Url = "https://localhost/globalassets/notfound.jpg";
            result = await subject.Index(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(typeof(NotFoundResult), result!.GetType());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceProvider?.Dispose();
            }
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
            return GetHttpMessageHandler((urlGlob, action));
        }

        private static MockHttpMessageHandler GetHttpMessageHandler(params (string, Action<MockedRequest>)[] data)
        {
            var mockHttp = new MockHttpMessageHandler();

            foreach (var (glob, action) in data)
            {
                var mockRequest = mockHttp.When(glob);
                action(mockRequest);
            }

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
