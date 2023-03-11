using System.Runtime.Serialization;

namespace PokeGuessr.Exceptions
{
    [Serializable]
    internal class OutOfTheBoxException : Exception
    {
        public OutOfTheBoxException()
        {
        }

        public OutOfTheBoxException(string? message) : base(message)
        {
        }

        public OutOfTheBoxException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OutOfTheBoxException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}