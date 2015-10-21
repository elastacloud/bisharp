using BISharp.Contracts;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BISharp
{
    public class DashboardClient
    {
        private PowerBiAuthentication _bi;
        private RestClient _client;
        private CancellationTokenSource _cancellationToken;

        public DashboardClient(PowerBiAuthentication bi)
        {
            this._bi = bi;
            this._client = new RestClient("https://api.powerbi.com");
            this._cancellationToken = new CancellationTokenSource();
        }

        public async Task<Dashboards> List()
        {
            var request = new RestRequest("beta/myorg/dashboards", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = await _client.ExecuteTaskAsync<Dashboards>(request, _cancellationToken.Token);
            return response.Data;
        }
        public async Task<Dashboard> Get(string dashboardId)
        {
            var request = new RestRequest($"beta/myorg/dashboards/{dashboardId}", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = await _client.ExecuteTaskAsync<Dashboard>(request, _cancellationToken.Token);
            return response.Data;
        }
        public async Task<Tiles> Tiles(string dashboardId)
        {
            var request = new RestRequest($"beta/myorg/dashboards/{dashboardId}/tiles", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = await _client.ExecuteTaskAsync<Tiles>(request, _cancellationToken.Token);
            return response.Data;
        }
        public async Task<Tile> TilesGet(string dashboardId, string tileId)
        {
            var request = new RestRequest($"beta/myorg/dashboards/{dashboardId}/tiles/{tileId}", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = await _client.ExecuteTaskAsync<Tile>(request, _cancellationToken.Token);
            return response.Data;
        }
    }
}
