using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace BISharp.Addressing
{
    public class PowerBiAddresses
    {
        public string CreateDataset(string groupId)
        {
            if (string.IsNullOrEmpty(groupId))
            {
                return "v1.0/myorg/datasets?defaultRetentionPolicy={defaultRetentionPolicy}";
            }
            else
            {
                return "v1.0/myorg/groups/" + groupId + "/datasets?defaultRetentionPolicy={defaultRetentionPolicy}";
            }
        }
        public string GetDatasets(string groupId)
        {
            if (string.IsNullOrEmpty(groupId))
            {
                return "v1.0/myorg/datasets";
            }
            else
            {
                return "v1.0/myorg/groups/" + groupId + "/datasets";
            }            
        }

        public string GetDatasetTables(string groupId)
        {
            if (string.IsNullOrEmpty(groupId))
            {
                return "v1.0/myorg/datasets/{datasetId}/tables";
            }
            else
            {
                return "v1.0/myorg/groups/" + groupId + "/datasets/{datasetId}/tables";
            }
        }

        public string UpdateTableSchema(string groupId)
        {
            //"v1.0/myorg/datasets/{datasetId}/tables/{tableName}"
            if (string.IsNullOrEmpty(groupId))
            {
                return "v1.0/myorg/datasets/{datasetId}/tables/{tableName}";
            }
            else
            {
                return "v1.0/myorg/datasets/groups/" + groupId + "/{datasetId}/tables/{tableName}";
            }
        }
        public string AddOrRemoveRows(string groupId)
        {
            //"v1.0/myorg/groups/{group_id}/datasets/{dataset_id}/tables/{table_name}/rows"
            if (string.IsNullOrEmpty(groupId))
            {
                return "v1.0/myorg/datasets/{datasetId}/tables/{tableName}/rows";
            }
            else
            {
                return "v1.0/myorg/groups/"+ groupId +"/datasets/{datasetId}/tables/{tableName}/rows";
            }
        }
    }
}
