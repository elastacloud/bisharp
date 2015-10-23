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

### Authentication Setup

In order to use PowerBI from code, you'll have to have access to an Azure Active Directory. Depending on the type of integration you're implementing
you'll have to create an application of either a native app (for console apps) or a web app (for web apps ;)). When you've created this, you can 
get hold of a "ClientID" and some client secrets. See the app.config or web.config for the points of configuration
that you must complete. Note that in both cases you'll need to delegate permissions to PowerBI.  

If you need to fully understand the OAuth flow, there's a lot of interesting reading ahead of you. In short, we're authorising our applications 
(of either type) with a central IdP which is AzureAD. We really want a token that'll let us auth against PowerBI and depending on the route you'll maybe
have to get a token for "http://yourbisharppoweredwebsite/" and then acquire a new token for the powerBI resource, or you can just straight out ask
for the powerBI token. That's part of the difference between websites and native apps. Once we have a token for that, we can attach it to all of our 
HTTP requests that go out to PowerBI. We're using RestSharp for that (massive ^HT to those guys). 

## Dashboards

Dashboards are collections of Tiles that you can embed in your apps. When you do that, you'll have to do a little auth jiggery pokery to get it to display
but we've gievn you a sample of that in resourceManagment.Web.

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

## Datasets

Datasets are containers for tables which are containers for columns and rows where your data actually lives. With BISharp we've taken a .net
oriented approach to creating these Datasets, that is we're deriving schema from static typing. You pass in some POCOs or some POCO types and we'll
create a schema that matches them. In the block below, when creating a dataset, we specify that it should contain a table for "resourceMeasures" that
is defined by the type specified.

```cs
            var dataClient = new DatasetsClient(pbi);
            var datasets = await dataClient.List();

            foreach (var dataset in datasets.value)
            {
                Console.WriteLine("{0}\t{1}", dataset.name, dataset.id);
            }

            Console.WriteLine("Create a data set?");
            Dataset created;
            if (Console.ReadKey().KeyChar == 'Y')
            {
                created = await dataClient.Create<resourceMeasures>("resourceManager", true);
            }
            else
            {
                created = (await dataClient.List()).value.First(ds => ds.name == "resourceManager");
            }
```

We also allow you to migrate between schemas with an update table call, again using types. In this and many other approaches you can either
use a generic method `UpdateTableSchema<TNewSchema>(...)` or pass in a type object (i.e. typeof(myClass) or instance.GetType());

```cs
	var table = await dataClient.UpdateTableSchema(created.id, tables.value.First().name, typeof(resourceMeasures2));
```

Finally, what's a dataset without data? We support inserting data into these datasets and clearing data from them.

```cs 
private async static Task GenerateData()
        {
            var dataClient = new DatasetsClient(pbi);
            var created = (await dataClient.List()).value.First(ds => ds.name == "resourceManager");
            var tables = await dataClient.ListTables(created.id);

            var dataRows = new TableRows<resourceMeasures>();

            for (int i = 0; i < 1000; i++)
            {
                dataRows.rows.Add(new resourceMeasures()
                {
                    Id = Guid.NewGuid().ToString("n"),
                    InServiceDate = DateTime.Now.AddYears(-4),
                    IsRunning = true,
                    LastMeasure = DateTime.Now,
                    Name = "Engine" + i.ToString(),
                    RPM = 6000,
                    Temperature = 431.6
                });
            }
            await dataClient.ClearRows(created.id, tables.value.First().name);
            await dataClient.AddRows(created.id, tables.value.First().name, dataRows);
            while (true)
            {
                Console.Write(".");
                foreach (var row in dataRows.rows)
                {
                    row.RPM = (int)(6000 * Math.Sin(Math.PI * (DateTime.Now.Second / 59D)));
                    row.Temperature += new Random((int)DateTime.Now.Ticks).Next(-5, 5);
                    row.LastMeasure = DateTime.Now;
                    row.IsRunning = row.RPM > 1;
                    Thread.Sleep(1);
                }

                await dataClient.AddRows(created.id, tables.value.First().name, dataRows);
            }
            await dataClient.ClearRows(created.id, tables.value.First().name); //note: unreachable ;) o_O 
        }
```