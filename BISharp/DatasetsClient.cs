using BISharp.Addressing;
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
        private PowerBiAddresses _addresses;

        public DatasetsClient(IPowerBiAuthentication bi) : this(bi, new RestClient("https://api.powerbi.com"))
        {
        }
        public DatasetsClient(IPowerBiAuthentication bi, IRestClient client)
        {
            this._bi = bi;
            this._client = client;
            this._client.AddDefaultHeader("Authorization", _bi.GetAccessToken());
            this._addresses = new PowerBiAddresses();
        }

        public async Task<Datasets> List()
        {
            return await List(string.Empty);
        }
        public async Task<Datasets> List(string groupId)
        {
            IRestRequest request = new RestRequest(_addresses.GetDatasets(groupId), Method.GET);

            var response = await _client.ExecuteTaskAsync<Datasets>(request);
            return response.Data;
        }
        public async Task<Dataset> Create<T1>(string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(datasetName, string.Empty, useBasicFifoRetentionPolicy, new[] { typeof(T1) });
        }
        public async Task<Dataset> Create<T1, T2>(string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(datasetName, string.Empty, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2) });
        }
        public async Task<Dataset> Create<T1, T2, T3>(string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(datasetName, string.Empty, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2), typeof(T3) });
        }
        public async Task<Dataset> Create<T1, T2, T3, T4>(string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(datasetName, string.Empty, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) });
        }
        public async Task<Dataset> Create<T1, T2, T3, T4, T5>(string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(datasetName, string.Empty, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) });
        }
        public async Task<Dataset> Create<T1, T2, T3, T4, T5, T6>(string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(datasetName, string.Empty, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) });
        }
        public async Task<Dataset> Create<T1>(string groupId, string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(groupId, datasetName, useBasicFifoRetentionPolicy, new[] { typeof(T1) });
        }
        public async Task<Dataset> Create<T1, T2>(string groupId, string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(groupId, datasetName, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2) });
        }
        public async Task<Dataset> Create<T1, T2, T3>(string groupId, string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(groupId, datasetName, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2), typeof(T3) });
        }
        public async Task<Dataset> Create<T1, T2, T3, T4>(string groupId, string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(groupId, datasetName, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) });
        }
        public async Task<Dataset> Create<T1, T2, T3, T4, T5>(string groupId, string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(groupId, datasetName, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) });
        }
        public async Task<Dataset> Create<T1, T2, T3, T4, T5, T6>(string groupId, string datasetName, bool useBasicFifoRetentionPolicy)
        {
            return await Create(groupId, datasetName, useBasicFifoRetentionPolicy, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) });
        }
        public async Task<Dataset> Create(string datasetName, bool useBasicFifoRetentionPolicy, params Type[] tableStructures)
        {
            return await Create(string.Empty, datasetName, useBasicFifoRetentionPolicy, tableStructures);
        }
        public async Task<Dataset> Create(string groupId, string datasetName, bool useBasicFifoRetentionPolicy, params Type[] tableStructures)
        {
            var defaultRetentionPolicy = useBasicFifoRetentionPolicy ? "basicFIFO" : "None";
            var tables = tableStructures.Select(t => Table.FromType(t)).ToList();
            var dataset = new Dataset { name = datasetName, tables = tables };

            var request = new RestRequest(_addresses.CreateDataset(groupId), Method.POST)
            { JsonSerializer = new Serialization.JsonSerializer() };
            request.RequestFormat = DataFormat.Json;
            request.AddUrlSegment("defaultRetentionPolicy", defaultRetentionPolicy);
            request.AddBody(dataset);

            var response = await _client.ExecuteTaskAsync<Dataset>(request);
            return response.Data;
        }
        public async Task<Tables> ListTables(string datasetId)
        {
            return await ListTables(string.Empty, datasetId);
        }
        public async Task<Tables> ListTables(string groupId, string datasetId)
        {
            var request = new RestRequest(_addresses.GetDatasetTables(groupId), Method.GET);
            request.AddUrlSegment("datasetId", datasetId);

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
        public async Task<Table> UpdateTableSchema<TnewTableStructure>(string groupId, string datasetId, string tableName)
        {
            return await UpdateTableSchema(groupId, datasetId, tableName, typeof(TnewTableStructure));
        }
        public async Task<Table> UpdateTableSchema<TnewTableStructure>(string datasetId, string tableName)
        {
            return await UpdateTableSchema(string.Empty, datasetId, tableName, typeof(TnewTableStructure));
        }
        public async Task<Table> UpdateTableSchema(string groupId, string datasetId, string tableName, Type newTableStructure)
        {
            var table = Table.FromType(newTableStructure);
            table.name = tableName;
            var request = new RestRequest(_addresses.UpdateTableSchema(groupId), Method.PUT)
            { JsonSerializer = new Serialization.JsonSerializer() };
            request.RequestFormat = DataFormat.Json;
            request.AddBody(table);
            request.AddUrlSegment("datasetId", datasetId);
            request.AddUrlSegment("tableName", tableName);

            var response = await _client.ExecuteTaskAsync<Table>(request);
            return response.Data;
        }
        public async Task<Table> AddRows<TTableRows>(string datasetId, string tableName, TableRows<TTableRows> rows)
        {
            return await AddRows<TTableRows>(string.Empty, datasetId, tableName, rows);
        }
        public async Task<Table> AddRows<TTableRows>(string groupId, string datasetId, string tableName, TableRows<TTableRows> rows)
        {
            var request = new RestRequest(_addresses.AddOrRemoveRows(groupId), Method.POST)
            { JsonSerializer = new Serialization.JsonSerializer() };
            request.RequestFormat = DataFormat.Json;
            request.AddBody(rows);
            request.AddUrlSegment("datasetId", datasetId);
            request.AddUrlSegment("tableName", tableName);

            var response = await _client.ExecuteTaskAsync<Table>(request);
            return response.Data;
        }
        public async Task<Table> ClearRows(string datasetId, string tableName)
        {
            return await ClearRows(string.Empty, datasetId, tableName);
        }
        public async Task<Table> ClearRows(string groupId, string datasetId, string tableName)
        {
            var request = new RestRequest(_addresses.AddOrRemoveRows(groupId), Method.DELETE);
            request.AddUrlSegment("datasetId", datasetId);
            request.AddUrlSegment("tableName", tableName);

            var response = await _client.ExecuteTaskAsync<Table>(request);
            return response.Data;
        }
    }
}
