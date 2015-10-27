using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp.Contracts
{
    public class Groups
    {
        public List<Group> value { get; set; }

    }
    public class Group
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
