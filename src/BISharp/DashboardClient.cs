using BISharp.Addressing;
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
        private IPowerBiAuthentication _bi;
        private RestClient _client;
        private CancellationTokenSource _cancellationToken;
        private PowerBiAddresses _addresses;

        public DashboardClient(IPowerBiAuthentication bi)
        {
            this._bi = bi;
            this._client = new RestClient("https://api.powerbi.com");
            this._client.AddDefaultHeader("Authorization", _bi.GetAccessToken());
            this._cancellationToken = new CancellationTokenSource();
            this._addresses = new PowerBiAddresses();
        }

        public async Task<Dashboards> List()
        {
            return await List(string.Empty);
        }
        public async Task<Dashboards> List(string groupId)
        {
            var request = new RestRequest(_addresses.GetDashboards(groupId), Method.GET);

            var response = await _client.ExecuteTaskAsync<Dashboards>(request, _cancellationToken.Token);
            return response.Data;
        }
        public async Task<Dashboard> Get(string dashboardId)
        {
            return await Get(string.Empty, dashboardId);
        }
        public async Task<Dashboard> Get(string groupId, string dashboardId)
        {
            var request = new RestRequest(_addresses.GetDashboard(groupId), Method.GET);
            request.AddUrlSegment("dashboardId", dashboardId);

            var response = await _client.ExecuteTaskAsync<Dashboard>(request, _cancellationToken.Token);
            return response.Data;
        }
        public async Task<Tiles> Tiles(string dashboardId)
        {
            return await Tiles(string.Empty, dashboardId);
        }
        public async Task<Tiles> Tiles(string groupId, string dashboardId)
        {
            var request = new RestRequest(_addresses.GetDashboardTiles(groupId), Method.GET);
            request.AddUrlSegment("dashboardId", dashboardId);

            var response = await _client.ExecuteTaskAsync<Tiles>(request, _cancellationToken.Token);
            return response.Data;
        }
        public async Task<Tile> TilesGet(string dashboardId, string tileId)
        {
            return await TilesGet(string.Empty, dashboardId, tileId);
        }
        public async Task<Tile> TilesGet(string groupId, string dashboardId, string tileId)
        {
            var request = new RestRequest(_addresses.GetDashboardTile(groupId), Method.GET);
            request.AddUrlSegment("dashboardId", dashboardId);
            request.AddUrlSegment("tileId", tileId);

            var response = await _client.ExecuteTaskAsync<Tile>(request, _cancellationToken.Token);
            return response.Data;
        }
    }
}
