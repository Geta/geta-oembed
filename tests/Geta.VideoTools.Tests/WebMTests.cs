// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.VideoTools.Tests.Streams;
using Geta.VideoTools.WebM;

namespace Geta.VideoTools.Tests;

public class WebMTests
{
    [Theory]
    [InlineData("Resources/sample_640.webm", 640, 360)]
    [InlineData("Resources/sample_480.webm", 480, 270)]
    [InlineData("Resources/sample_960.webm", 960, 540)]
    public void WebMMetadataParser_can_Parse_Metadata(string file, int expectedWidth, int expectedHeight)
    {
        var subject = new WebMMetadataParser();
        var testStream = File.OpenRead(file);

        var meta = subject.Parse(testStream);

        Assert.NotNull(meta);
        Assert.Equal(expectedWidth, meta.Width);
        Assert.Equal(expectedHeight, meta.Height);
    }

    [Theory]
    [InlineData("Resources/sample_640.webm", 640, 360)]
    [InlineData("Resources/sample_480.webm", 480, 270)]
    [InlineData("Resources/sample_960.webm", 960, 540)]
    public void WebMMetadataParser_can_Parse_Metadata_in_not_seekable_Stream(string file, int expectedWidth, int expectedHeight)
    {
        var subject = new WebMMetadataParser();
        var fileStream  = File.OpenRead(file);
        var testStream = new UnseekableStream(fileStream);

        var meta = subject.Parse(testStream);

        Assert.NotNull(meta);
        Assert.Equal(expectedWidth, meta.Width);
        Assert.Equal(expectedHeight, meta.Height);
    }
}
