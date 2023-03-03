using System.Runtime.Serialization;

namespace MyWebsite.Exceptions
{
    [Serializable]
    internal class PosNotFoundExcpetion : Exception
    {
        public PosNotFoundExcpetion()
        {
        }

        public PosNotFoundExcpetion(string? message) : base(message)
        {
        }

        public PosNotFoundExcpetion(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PosNotFoundExcpetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}