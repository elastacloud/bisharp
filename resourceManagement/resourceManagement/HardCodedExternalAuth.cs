using BISharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resourceManagement
{
    public class HardCodedExternalAuth : IExternalAuth
    {
        public string GetBearerToken()
        {
            return "Bearer abcdefg";
        }
    }
}
