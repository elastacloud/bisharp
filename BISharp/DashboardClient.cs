using BISharp.Contracts;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp
{
    public class DashboardClient
    {
        private PowerBI _bi;
        private RestClient _client;

        public DashboardClient(PowerBI bi)
        {
            this._bi = bi;
            this._client = new RestClient("https://api.powerbi.com");
        }

        public Dashboards List()
        {
            var request = new RestRequest("beta/myorg/dashboards", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = _client.Execute<Dashboards>(request);
            return response.Data;
        }
        public Dashboard Get(string dashboardId)
        {
            var request = new RestRequest($"beta/myorg/dashboards/{dashboardId}", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = _client.Execute<Dashboard>(request);
            return response.Data;
        }
        public Tiles Tiles(string dashboardId)
        {
            var request = new RestRequest($"beta/myorg/dashboards/{dashboardId}/tiles", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = _client.Execute<Tiles>(request);
            return response.Data;
        }
        public Tile TilesGet(string dashboardId, string tileId)
        {
            var request = new RestRequest($"beta/myorg/dashboards/{dashboardId}/tiles/{tileId}", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = _client.Execute<Tile>(request);
            return response.Data;
        }
    }
}
