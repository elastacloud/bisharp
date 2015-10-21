using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp.Contracts
{
    public class Dashboards
    {
        public List<Dashboard> value { get; set; }
    }

    public class Dashboard
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public bool isReadOnly { get; set; }
    }
}
