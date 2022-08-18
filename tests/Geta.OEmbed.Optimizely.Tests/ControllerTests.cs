// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.AspNetCore.Mvc;
using Geta.OEmbed.Models;
using Geta.OEmbed.Providers;
using Geta.OEmbed.Tests.Common.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace Geta.OEmbed.Optimizely.Tests
{
    public class ControllerTests : IDisposable
    {      
        private readonly ServiceProvider _serviceProvider;

        public ControllerTests()
        {
            var serviceCollection = new ServiceCollection();

            var providerClientFactory = CreateHttpClientFactory(() => GetProviderMessageHandler());
            var endpointClientFactory = CreateHttpClientFactory(() => GetEndpointMessageHandler());

            var config = new OEmbedConfiguration();
            var manifestLoader = new HttpClientManifestLoader(config, providerClientFactory.CreateClient());
            var repository = new OEmbedProviderRepository(manifestLoader);
            var urlBuilders = new[] { new DefaultProviderUrlBuilder() };
            var embedFormatters = Enumerable.Empty<IProviderResponseFormatter>();

            var embedService = new OEmbedService(repository, urlBuilders, embedFormatters, endpointClientFactory.CreateClient());

            serviceCollection.AddSingleton<IOEmbedService>(embedService);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
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

            if (result is null)
                throw new InvalidOperationException("response cannot be null here");

            Assert.Equal(typeof(OkObjectResult), result.GetType());

            if (result is not OkObjectResult objectResult)
                throw new InvalidOperationException("objectResult cannot be null here");

            var objectValue = objectResult.Value;
            Assert.NotNull(objectValue);

            if (objectValue is null)
                throw new InvalidOperationException("objectValue cannot be null here");

            Assert.Equal(typeof(OEmbedResponse), objectValue.GetType());
        }

        [Fact]
        public async Task CmsOEmbedController_can_Get()
        {
            var url = "https://www.youtube.com/watch?v=HmZKgaHa3Fg";
            var subject = ActivatorUtilities.CreateInstance<OEmbedCmsController>(_serviceProvider);
            var request = new OEmbedRequest
            {
                Url = url,
                Autoplay = true,
                Loop = true,
                Muted = true
            };

            var result = await subject.Index(request, CancellationToken.None);
            Assert.NotNull(result);

            if (result is null)
                throw new InvalidOperationException("response cannot be null here");

            Assert.Equal(typeof(OkObjectResult), result.GetType());

            if (result is not OkObjectResult objectResult)
                throw new InvalidOperationException("objectResult cannot be null here");

            var objectValue = objectResult.Value;
            Assert.NotNull(objectValue);

            if (objectValue is null)
                throw new InvalidOperationException("objectValue cannot be null here");

            Assert.Equal(typeof(OEmbedResponse), objectValue.GetType());
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
