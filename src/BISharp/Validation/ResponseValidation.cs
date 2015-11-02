using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp.Validation
{
    public static class ResponseValidation
    {
        public static void HandleResponseErrors(IRestResponse response)
        {
            if (((int)response.StatusCode) == 400)
            {
                throw new BISharpRequestException(response);
            }

            if (((int)response.StatusCode) == 401)
            {
                throw new BISharpAuthenticationException(response);
            }

            if (response.ErrorException != null)
                throw response.ErrorException;
        }
    }
}
