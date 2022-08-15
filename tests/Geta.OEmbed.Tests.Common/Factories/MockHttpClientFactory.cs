// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using RichardSzalay.MockHttp;

namespace Geta.OEmbed.Tests.Common.Factories
{
    public class MockHttpClientFactory : IHttpClientFactory
    {
        private readonly Func<MockHttpMessageHandler> _handlerFactory;

        public MockHttpClientFactory(Func<MockHttpMessageHandler> handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }

        public HttpClient CreateClient(string name)
        {
            var handler = _handlerFactory();
            var client = new HttpClient(handler);

            return client;
        }
    }
}
