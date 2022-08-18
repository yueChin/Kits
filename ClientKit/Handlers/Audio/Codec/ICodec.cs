
namespace Kits.ClientKit.Handlers.Audio.Codec
{
    public interface ICodec
    {
        byte[] Encode(short[] data);
        short[] Decode(byte[] data);
    }
}
