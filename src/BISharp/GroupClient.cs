using BISharp.Contracts;
using BISharp.Validation;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BISharp
{
    public class GroupClient
    {
        private IPowerBiAuthentication _bi;
        private RestClient _client;
        private CancellationTokenSource _cancellationToken;

        public GroupClient(IPowerBiAuthentication bi)
        {
            this._bi = bi;
            this._client = new RestClient("https://api.powerbi.com");
            this._client.AddDefaultHeader("Authorization", _bi.GetAccessToken());
            this._cancellationToken = new CancellationTokenSource();
        }

        public async Task<Groups> Get()
        {
            var request = new RestRequest($"v1.0/myorg/groups", Method.GET);

            var response = await _client.ExecuteTaskAsync<Groups>(request, _cancellationToken.Token);
            ResponseValidation.HandleResponseErrors(response);
            return response.Data;
        }
    }
}
