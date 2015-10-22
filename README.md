# bisharp

[![bisharp MyGet Build Status](https://www.myget.org/BuildSource/Badge/bisharp?identifier=1becfa4c-f07d-4a7b-b6c2-3a0222f0bd53)](https://www.myget.org/)

This project is an SDK for controlling PowerBI with C#. It's key goals are to simplify data access and authentication, allowing 
easy and rapid access to core PowerBI assets;

  1. Dashboards
    1. List
    1. Create
  1. Datasets
    1. List
	1. Create
	1. List Tables
	1. Update Table Schema
	1. Add Rows to Table
	1. Clear Rows from Table

## Authentication

There are two main approaches to authentication in BISharp, which we broadly call internal and external authentication. PowerBI uses Azure Active Directory (Azure AD).
How to configure this for external access is documented [here](https://msdn.microsoft.com/en-us/library/mt203565.aspx). You can pass BISharp a
ClientID from AzureAD and it will use that to prompt you for your credentials for PowerBI. That's all taken care of via a dependency of this 
library (adal4net) and we don't ever see the credentials being passed from the client to PowerBI. 

So that's what we call internal - we encapsulate the logic that gets the access credentials for you. On to external.

You can pass BISharp an implementation of IExternalAuth and implement the `string GetBearerToken()` interface. This can contain your own logic for
PowerBI access token generation, that we'll call. We only need that bearer token to access PowerBI, so you're free to implement that however you want.
For example, a good use case for this is when you are in a Web Application, with delegated permissions to PowerBI - you don't want to use the internal
approach because that uses an interactive desktop window to capture the Azure AD credentials. 

There's an example in ~/HardCodedExternalAuth.cs which passes a pregenerated bearer token. 

## Dashboards

```cs
    private async static Task QueryDashboards()
    {
        var dashboardClient = new DashboardClient(pbi);
        var dashboards = await dashboardClient.List();

        foreach (var dashboard in dashboards.value)
        {
            Console.WriteLine("{0}\t{1}", dashboard.displayName, dashboard.id);

            var tiles = await dashboardClient.Tiles(dashboard.id);
            foreach (var tile in tiles.value)
            {
                Console.WriteLine(tile.embedUrl);
            }
        }
    }
```