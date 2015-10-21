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
        private PowerBI _bi;
        private RestClient _client;

        public DatasetsClient(PowerBI bi)
        {
            this._bi = bi;
            this._client = new RestClient("https://api.powerbi.com");
        }

        public Datasets List()
        {
            var request = new RestRequest("v1.0/myorg/datasets", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = _client.Execute<Datasets>(request);
            return response.Data;
        }

        public Dataset Create(string datasetName, bool useBasicFifoRetentionPolicy, params Type[] tableStructures)
        {
            var defaultRetentionPolicy = useBasicFifoRetentionPolicy ? "basicFIFO" : "None";
            var tables = tableStructures.Select(t => Table.FromType(t)).ToList();
            var dataset = new Dataset { name = datasetName, tables = tables };

            var request = new RestRequest($"v1.0/myorg/datasets?defaultRetentionPolicy={defaultRetentionPolicy}", Method.POST)
            { JsonSerializer = new Serialization.JsonSerializer() };
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());
            request.RequestFormat = DataFormat.Json;
            request.AddBody(dataset);

            var response = _client.Execute<Dataset>(request);
            return response.Data;
        }

        public Tables ListTables(string datasetId)
        {
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = _client.Execute<Tables>(request);
            return response.Data;
        }

        public Table GetTable(string datasetId, string tableName)
        {
            throw new NotImplementedException("This isn't found");
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables/{tableName}", Method.GET);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = _client.Execute<Table>(request);
            return response.Data;
        }

        public Table UpdateTable(string datasetId, string tableName, Type tableStructure)
        {
            var table = Table.FromType(tableStructure);
            table.name = tableName;
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables/{tableName}", Method.PUT)
            { JsonSerializer = new Serialization.JsonSerializer() };
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());
            request.RequestFormat = DataFormat.Json;
            request.AddBody(table);

            var response = _client.Execute<Table>(request);
            return response.Data;
        }
        public Table AddRows<TTableRows>(string datasetId, string tableName, TableRows<TTableRows> rows)
        {
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables/{tableName}/rows", Method.POST)
            { JsonSerializer = new Serialization.JsonSerializer() };
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());
            request.RequestFormat = DataFormat.Json;
            request.AddBody(rows);

            var response = _client.Execute<Table>(request);
            return response.Data;
        }
        public Table ClearRows(string datasetId, string tableName)
        {
            var request = new RestRequest($"v1.0/myorg/datasets/{datasetId}/tables/{tableName}/rows", Method.DELETE);
            request.AddHeader("Authorization", _bi._token.CreateAuthorizationHeader());

            var response = _client.Execute<Table>(request);
            return response.Data;
        }
    }
}
