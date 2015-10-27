using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp.Contracts
{
    public class Datasets
    {
        public enum DataType
        {
            Int64,
            Double,
            Boolean,
            Datetime,
            String
        }
        public List<Dataset> value { get; set; }
    }

    public class Dataset { 
        public string id { get; set; }
        public string name { get; set; }
        public string defaultRetentionPolicy { get; set; }
        public List<Table> tables { get; set; }
    }
}
