using BISharp.Contracts;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp
{
    public class DatasetsClient
    {
        private IPowerBiAuthentication _bi;
        private IRestClient _client;

        public DatasetsClient(IPowerBiAuthentication bi) : this(bi, new RestClient("https://api.powerbi.com"))
        {
        }
        public DatasetsClient(IPowerBiAuthentication bi, IRestClient client)
        {
            this._bi = bi;
            this._client = client;
            this._client.AddDefaultHeader("Authorization", _bi.GetAccessToken());
        }

        public async Task<Datasets> List()
        {
            IRestRequest request = new RestRequest("v1.0/myorg/datasets", Method.GET);

            var response = await _client.ExecuteTaskAsync<Datasets>(request);
            return response.Data;
        }

        public async Task<Dataset> Create(string datasetName, bool useBasicFifoRetentionPolicy, params Type[] tableStructures)
        {
            var defaultRetentionPolicy = useBasicFifoRetentionPolicy ? "basicFIFO" : "None";
            var tables = tableStructures.Select(t => Table.FromType(t)).ToList();
            var dataset = new Dataset { name = datasetName, tables = tables };

            var request = new RestRequest($"v1.0/myorg/datasets?defaultRetentionPolicy={defaultRetentionPolicy}", Method.POST)
            { JsonSerializer = new Serialization.JsonSerializer() };
            request.RequestFormat = DataFormat.Json;
            request.AddBody(dataset);

            var response = await _client.ExecuteTaskAsync<Dataset>(request);
            return response.Data;
        }

        public async Task<Tables> ListTables(string datasetId)
        {
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables", Method.GET);

            var response = await _client.ExecuteTaskAsync<Tables>(request);
            return response.Data;
        }

        public async Task<Table> GetTable(string datasetId, string tableName)
        {
            throw new NotImplementedException("This isn't found");
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables/{tableName}", Method.GET);

            var response = await _client.ExecuteTaskAsync<Table>(request);
            return response.Data;
        }

        public async Task<Table> UpdateTableSchema(string datasetId, string tableName, Type newTableStructure)
        {
            var table = Table.FromType(newTableStructure);
            table.name = tableName;
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables/{tableName}", Method.PUT)
            { JsonSerializer = new Serialization.JsonSerializer() };
            request.RequestFormat = DataFormat.Json;
            request.AddBody(table);

            var response = await _client.ExecuteTaskAsync<Table>(request);
            return response.Data;
        }
        public async Task<Table> AddRows<TTableRows>(string datasetId, string tableName, TableRows<TTableRows> rows)
        {
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables/{tableName}/rows", Method.POST)
            { JsonSerializer = new Serialization.JsonSerializer() };
            request.RequestFormat = DataFormat.Json;
            request.AddBody(rows);

            var response = await _client.ExecuteTaskAsync<Table>(request);
            return response.Data;
        }
        public async Task<Table> ClearRows(string datasetId, string tableName)
        {
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables/{tableName}/rows", Method.DELETE);

            var response = await _client.ExecuteTaskAsync<Table>(request);
            return response.Data;
        }
    }
}
