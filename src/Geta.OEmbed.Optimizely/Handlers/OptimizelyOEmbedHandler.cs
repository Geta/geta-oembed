// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Geta.OEmbed.Client.Models;
using Geta.OEmbed.Optimizely.Models;
using File = TagLib.File;

namespace Geta.OEmbed.Optimizely.Handlers
{
    public class OptimizelyOEmbedHandler
    {
        private readonly IUrlResolver _urlResolver;

        public OptimizelyOEmbedHandler(IUrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        public async Task<OEmbedResponse?> HandleAsync(OEmbedRequest request, CancellationToken cancellationToken)
        {
            if (_urlResolver.Route(new UrlBuilder(request.Url)) is not IOEmbedMedia content)
            {
                return null;
            }

            var (width, height) = await GetDimensionsAsync(content, cancellationToken);
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

        protected virtual async Task<(int, int)> GetDimensionsAsync(IOEmbedMedia content, CancellationToken cancellationToken)
        {
            if (content.Width.HasValue && content.Height.HasValue)
            {
                return (content.Width.Value, content.Height.Value);
            }

            var isImage = content is IContentImage;
            var isVideo = content is IContentVideo;

            var fileInfo = await content.BinaryData.AsFileInfoAsync();

            if (!fileInfo.Exists || string.IsNullOrEmpty(fileInfo.PhysicalPath))
            {
                return (default, default);
            }

            var tag = File.Create(fileInfo.PhysicalPath);

            if (isImage)
            {
                return (tag.Properties.PhotoWidth, tag.Properties.PhotoHeight);
            }

            if (isVideo)
            {
                return (tag.Properties.VideoWidth, tag.Properties.VideoHeight);
            }

            return (default, default);
        }
    }
}
