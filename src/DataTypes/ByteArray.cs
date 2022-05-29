#nullable disable
using PsychoPortal;

namespace PsychonautsStudio;

public class ByteArray : IBinarySerializable
{
    public ByteArray() { }

    public ByteArray(byte[] data)
    {
        Data = data;
        Pre_Length = data.Length;
    }

    public long Pre_Length { get; set; }

    public byte[] Data { get; set; }

    public void Serialize(IBinarySerializer s)
    {
        Data = s.SerializeArray<byte>(Data, Pre_Length, name: nameof(Data));
    }
}