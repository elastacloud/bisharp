using System;
using System.Runtime.Serialization;
using RestSharp;

namespace BISharp
{
    [Serializable]
    internal class BISharpAuthenticationException : Exception
    {
        private IRestResponse response;

        public BISharpAuthenticationException()
        {
        }

        public BISharpAuthenticationException(string message) : base(message)
        {
        }

        public BISharpAuthenticationException(IRestResponse response) :
            this(string.Format("BISharp failed to authenticate correctly: {0}", response.ErrorMessage), 
                response.ErrorException)
        {
            this.response = response;
        }

        public BISharpAuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BISharpAuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}