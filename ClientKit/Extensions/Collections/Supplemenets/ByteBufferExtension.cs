using Kits.DevlpKit.Supplements;
using UnityEngine;

namespace AFForUnity.Kits.ClientKit.Extensions.Collections.Supplemenets
{
    public static class ByteBufferEx
    {
        public static void WriteVector2(this ByteBuffer buffer, Vector2 value)
        {
            buffer.WriteFloat(value.x);
            buffer.WriteFloat(value.y);
        }
        public static void WriteVector3(this ByteBuffer buffer,Vector3 value)
        {
            buffer.WriteFloat(value.x);
            buffer.WriteFloat(value.y);
            buffer.WriteFloat(value.z);
        }
        public static void WriteVector4(this ByteBuffer buffer,Vector4 value)
        {
            buffer.WriteFloat(value.x);
            buffer.WriteFloat(value.y);
            buffer.WriteFloat(value.z);
            buffer.WriteFloat(value.w);
        }
    }
    
}

