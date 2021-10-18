using System;
using System.Runtime.Serialization;

namespace Infrastructure.CQRS
{
    [Serializable]
    internal class CQRSSampleDomainException : Exception
    {
        public CQRSSampleDomainException()
        {
        }

        public CQRSSampleDomainException(string message) : base(message)
        {
        }

        public CQRSSampleDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CQRSSampleDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
