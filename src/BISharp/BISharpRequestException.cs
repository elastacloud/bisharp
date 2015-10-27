using System;
using System.Runtime.Serialization;
using RestSharp;

namespace BISharp
{
    [Serializable]
    internal class BISharpRequestException : Exception
    {
        private IRestResponse response;

        public BISharpRequestException()
        {
        }

        public BISharpRequestException(string message) : base(message)
        {
        }

        public BISharpRequestException(IRestResponse response) : 
            this(string.Format("BISharp failed to create a valid request: {0}", response.ErrorMessage), 
                response.ErrorException)
        {
            this.response = response;
        }

        public BISharpRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BISharpRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}