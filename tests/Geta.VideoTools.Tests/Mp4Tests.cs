// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.VideoTools.Mp4;
using Geta.VideoTools.Tests.Streams;

namespace Geta.VideoTools.Tests;

public class Mp4Tests
{
    [Theory]
    [InlineData("Resources/sample_320.mp4", 320, 240)]
    [InlineData("Resources/sample_320_2.mp4", 320, 240)]
    [InlineData("Resources/sample_480.mp4", 480, 270)]

    public void Mp4MetadataParser_can_Parse_Metadata(string file, int expectedWidth, int expectedHeight)
    {
        var subject = new Mp4MetadataParser();
        var testStream = File.OpenRead(file);

        var meta = subject.Parse(testStream);

        Assert.NotNull(meta);
        Assert.Equal(expectedWidth, meta.Width);
        Assert.Equal(expectedHeight, meta.Height);
    }

    [Theory]
    [InlineData("Resources/sample_320.mp4", 320, 240)]
    [InlineData("Resources/sample_320_2.mp4", 320, 240)]
    [InlineData("Resources/sample_480.mp4", 480, 270)]

    public void Mp4MetadataParser_can_Parse_Metadata_in_not_seekable_Stream(string file, int expectedWidth, int expectedHeight)
    {
        var subject = new Mp4MetadataParser();
        var fileStream = File.OpenRead(file);
        var testStream = new UnseekableStream(fileStream);

        var meta = subject.Parse(testStream);

        Assert.NotNull(meta);
        Assert.Equal(expectedWidth, meta.Width);
        Assert.Equal(expectedHeight, meta.Height);
    }
}
