using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp
{
    public interface IPowerBiAuthentication
    {
        string GetAccessToken();
    }
    public class PowerBiAuthentication : IPowerBiAuthentication
    {
        private AuthenticationResult _token { get; set; }
        private string _clientId;
        private IExternalAuth _externalAuth;

        public PowerBiAuthentication(string clientId)
        {
            this._clientId = clientId;
            getAccessToken();
        }

        public PowerBiAuthentication(IExternalAuth extAuth)
        { this._externalAuth = extAuth; }

        private void getAccessToken()
        {
            string resourceUri = "https://analysis.windows.net/powerbi/api";
            string redirectUri = "https://login.live.com/oauth20_desktop.srf";
            string authorityUri = "https://login.windows.net/common/oauth2/authorize";
            AuthenticationContext authContext = new AuthenticationContext(authorityUri);

            _token = authContext.AcquireToken(resourceUri, this._clientId, 
                new Uri(redirectUri), PromptBehavior.Always);            
        }

        public string GetAccessToken()
        {
            if (_externalAuth != null)
                return _externalAuth.GetBearerToken();

            return _token.CreateAuthorizationHeader();
        }
    }
}
