// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.OEmbed.Extensions
{
    public static class StringExtensions
    {
        public static string AddParameters(this string url, IDictionary<string, string> parameters)
        {
            var query = string.Join("&amp;", parameters.Select(o => $"{o.Key}={o.Value}"));

            return url.IndexOf("?", StringComparison.Ordinal) > -1
                ? $"{url}&{query}"
                : $"{url}?{query}";
        }
    }
}
