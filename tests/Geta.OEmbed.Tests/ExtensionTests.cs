// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Extensions;

namespace Geta.OEmbed.Tests
{
    public class ExtensionTests
    {
        [Fact]
        public void StringExtensions_can_AddParameters()
        {
            var firstUrl = "https://www.youtube.com";
            var secondUrl = "https://www.youtube.com/watch?v=x2hw_ghPcQs";
            var thirdUrl = "";

            var parameters = new Dictionary<string, string>
            {
                { "autoplay", "1" },
                { "mute", "1" },
                { "fullscreen", "0" }
            };

            var queryParameters = "autoplay=1&amp;mute=1&amp;fullscreen=0";

            var subject = firstUrl.AddParameters(parameters);
            Assert.Equal(firstUrl + "?" + queryParameters, subject);

            subject = secondUrl.AddParameters(parameters);
            Assert.Equal(secondUrl + "&" + queryParameters, subject);

            subject = thirdUrl.AddParameters(parameters);
            Assert.Equal(thirdUrl + "?" + queryParameters, subject);
        }
    }
}
