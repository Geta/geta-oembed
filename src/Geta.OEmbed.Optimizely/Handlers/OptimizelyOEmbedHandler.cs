// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Geta.OEmbed.Client.Models;
using Geta.OEmbed.Optimizely.Models;
using Geta.VideoTools.Common;
using Geta.VideoTools.Common.Models;
using Geta.VideoTools.Mp4;
using Geta.VideoTools.WebM;
using Microsoft.Extensions.Logging;

namespace Geta.OEmbed.Optimizely.Handlers
{
    public class OptimizelyOEmbedHandler : IOptimizelyOEmbedHandler
    {
        private readonly IUrlResolver _urlResolver;
        private readonly ILogger<OptimizelyOEmbedHandler> _logger;

        public OptimizelyOEmbedHandler(IUrlResolver urlResolver, ILogger<OptimizelyOEmbedHandler> logger)
        {
            _urlResolver = urlResolver;
            _logger = logger;
        }

        public virtual async Task<OEmbedResponse?> HandleAsync(OEmbedRequest request, CancellationToken cancellationToken)
        {
            var routedContent = _urlResolver.Route(new UrlBuilder(request.Url));

            if (routedContent is not IOEmbedMedia content)
            {
                return null;
            }

            var (width, height) = await GetDimensions(content);
            var contentIsVideo = content is IContentVideo;
            var html = contentIsVideo ? RenderVideoHtml(request, content, width, height)
                                      : RenderImageHtml(request, content, width, height);

            var oEmbedEntry = new OEmbedResponse
            {
                Type = contentIsVideo ? "video" : "image",
                Title = content.Title ?? content.Name,
                Width = width,
                Height = height,
                Html = html,
            };

            return oEmbedEntry;
        }

        protected virtual string RenderVideoHtml(OEmbedRequest request, IOEmbedMedia content, int width, int height)
        {
            var videoAttributes = FormatVideoAttributes(request);
            var friendlyUrl = FormatVideoUrl(request);

            return @$"<video width=""{width}"" height=""{height}"" {videoAttributes}><source src=""{friendlyUrl}""></video>";
        }

        protected virtual string RenderImageHtml(OEmbedRequest request, IOEmbedMedia content, int width, int height)
        {
            var friendlyUrl = FormatImageUrl(request);

            return @$"<img src=""{friendlyUrl}"" alt=""{content.Title}"" />";
        }

        protected virtual string FormatVideoAttributes(OEmbedRequest request)
        {
            var videoAttributes = new Dictionary<string, string>(4);

            if (request.Autoplay)
            {
                videoAttributes.Add("autoplay", "");
                videoAttributes.Add("playsinline", "");
            }

            if (request.Controls)
            {
                videoAttributes.Add("controls", "");
            }

            if (request.Loop)
            {
                videoAttributes.Add("loop", "");
            }

            if (request.Muted)
            {
                videoAttributes.Add("muted", "");
            }

            var attributes = videoAttributes.Select(attr => @$"{attr.Key}=""{attr.Value}""");

            return string.Join(" ", attributes);
        }

        protected virtual string FormatVideoUrl(OEmbedRequest request)
        {
            return _urlResolver.GetUrl(request.Url);
        }

        protected virtual string FormatImageUrl(OEmbedRequest request)
        {
            return _urlResolver.GetUrl(request.Url);
        }

        protected virtual async Task<(int, int)> GetDimensions(IOEmbedMedia content)
        {
            if (content.Width.HasValue && content.Height.HasValue)
            {
                return (content.Width.Value, content.Height.Value);
            }

            var isVideo = content is IContentVideo;
            if (!isVideo)
            {
                return (default, default);
            }

            using var contentStream = await TryGetContentStream(content);
            if (contentStream is null)
            {
                return (default, default);
            }

            var extension = Path.GetExtension(content.Name);
            var parser = GetVideoParser(extension);
            if (parser is null)
            {
                return (default, default);
            }

            try
            {
                var dimensions = parser.Parse(contentStream);
                if (dimensions is null)
                {
                    return (default, default);
                }

                return (dimensions.Width, dimensions.Height);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing video dimensions: {Message}", ex.Message);

                return (default, default);
            }
        }

        protected virtual IStreamParser<VideoMeta>? GetVideoParser(string extension)
        {
            if (extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase))
                return new Mp4MetadataParser();

            if (extension.Equals(".webm", StringComparison.OrdinalIgnoreCase))
                return new WebMMetadataParser();

            return null;
        }

        protected virtual Task<Stream?> TryGetContentStream(IOEmbedMedia content)
        {
            try
            {
                var stream = content?.BinaryData?.OpenRead();
                return Task.FromResult(stream);
            }
            catch (IOException)
            {
                return Task.FromResult<Stream?>(null);
            }
        }
    }
}
