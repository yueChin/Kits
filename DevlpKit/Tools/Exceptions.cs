using System;
using System.Runtime.Serialization;

namespace Kits.DevlpKit.Tools
{
    [Serializable]
    public class Exceptions : Exception
    {
        public Exceptions()
            : base()
        {
        }

        public Exceptions(string message)
            : base(message)
        {
        }

        public Exceptions(string message, Exception innerException) : base(message, innerException)
        {
        }
        
        protected Exceptions(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}