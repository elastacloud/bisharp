using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp
{
    public class PowerBiAuthentication
    {
        internal AuthenticationResult _token;
        private string _clientId;

        public PowerBiAuthentication(string clientId)
        {
            this._clientId = clientId;
            getAccessToken();
        }

        private void getAccessToken()
        {
            string resourceUri = "https://analysis.windows.net/powerbi/api";
            string redirectUri = "https://login.live.com/oauth20_desktop.srf";
            string authorityUri = "https://login.windows.net/common/oauth2/authorize";
            AuthenticationContext authContext = new AuthenticationContext(authorityUri);

            _token = authContext.AcquireToken(resourceUri, this._clientId, 
                new Uri(redirectUri), PromptBehavior.Always);            
        }

    }
}
