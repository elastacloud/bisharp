using BISharp;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp_RoleClaims_DotNet.Utils;

namespace WebApp_RoleClaims_DotNet.Controllers
{
    public class powerbiWebToken : IExternalAuth
    {
        private IPrincipal _principal;

        public powerbiWebToken()
        {
        }
        public string GetBearerToken()
        {
            string resourceUri = "https://analysis.windows.net/powerbi/api";
            Claim userObjectIdClaim = ClaimsPrincipal.Current.FindFirst(Globals.ObjectIdClaimType);
            Claim tenantIdClaim = ClaimsPrincipal.Current.FindFirst(Globals.TenantIdClaimType);
            if (userObjectIdClaim != null && tenantIdClaim != null)
            {
                var authContext = new AuthenticationContext(
                    String.Format(CultureInfo.InvariantCulture, ConfigHelper.AadInstance, tenantIdClaim.Value),
                    new TokenDbCache(userObjectIdClaim.Value));
                var code = authContext.TokenCache.ReadItems().First().RefreshToken;
                var token = authContext.AcquireTokenByRefreshToken(code, new ClientCredential(ConfigHelper.ClientId, ConfigHelper.AppKey), resourceUri);

                return token.CreateAuthorizationHeader();
            }

            throw new UnauthorizedAccessException();
        }
    }
    [Authorize]
    public class PowerBIController : Controller
    {
        // GET: PowerBI
        public async Task<ActionResult> Index()
        {
            var pbi = new PowerBiAuthentication(new powerbiWebToken());
            var dashboardClient = new DashboardClient(pbi);
            var dashes = await dashboardClient.List();
            ViewBag.dashes = dashes.value;
            ViewBag.accessToken = pbi.GetAccessToken();

            var firstDash = dashes.value.First();
            var tiles = await dashboardClient.Tiles(firstDash.id);

            return View(model: tiles);
        }
        public async Task<ActionResult> Dash(string dashId)
        {
            var pbi = new PowerBiAuthentication(new powerbiWebToken());
            var dashboardClient = new DashboardClient(pbi);
            var dashes = await dashboardClient.List();
            ViewBag.dashes = dashes.value;
            ViewBag.accessToken = pbi.GetAccessToken();

            var firstDash = dashes.value.First(d=>d.id == dashId);
            var tiles = await dashboardClient.Tiles(firstDash.id);

            return View("Index", model: tiles);
        }
    }
}