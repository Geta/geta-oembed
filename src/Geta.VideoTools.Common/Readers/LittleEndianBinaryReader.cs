// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Text;

namespace Geta.VideoTools.Common.Readers;

public sealed class LittleEndianBinaryReader : BinaryReader
{
    public LittleEndianBinaryReader(Stream input) : base(input)
    {
    }

    public LittleEndianBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
    {
    }

    public LittleEndianBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
    {
    }

    public override short ReadInt16()
    {
        var data = base.ReadBytes(2);
        Array.Reverse(data);

        return BitConverter.ToInt16(data, 0);
    }

    public override int ReadInt32()
    {
        var data = base.ReadBytes(4);
        Array.Reverse(data);

        return BitConverter.ToInt32(data, 0);
    }

    public override long ReadInt64()
    {
        var data = base.ReadBytes(8);
        Array.Reverse(data);

        return BitConverter.ToInt64(data, 0);
    }

    public override ushort ReadUInt16()
    {
        var data = base.ReadBytes(2);
        Array.Reverse(data);

        return BitConverter.ToUInt16(data, 0);
    }

    public override uint ReadUInt32()
    {
        var data = base.ReadBytes(4);
        Array.Reverse(data);

        return BitConverter.ToUInt32(data, 0);
    }

    public override ulong ReadUInt64()
    {
        var data = base.ReadBytes(8);
        Array.Reverse(data);

        return BitConverter.ToUInt64(data, 0);
    }
}
