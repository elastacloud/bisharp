using BISharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BISharp.Contracts;
using System.Threading;
using resourceManagement.Contracts;
using System.Configuration;

namespace resourceManagement
{

    class Program
    {
        static PowerBiAuthentication pbi = new PowerBiAuthentication(ConfigurationManager.AppSettings["ida:ClientId"]);
        static void Main(string[] args)
        {
            Console.WriteLine("Querying Groups");
            QueryGroups().Wait();

            Console.WriteLine("Querying Dashboards");
            QueryDashboards().Wait();

            Console.WriteLine("Querying Datasets");
            QueryDatasets().Wait();
            Console.WriteLine("Generating Data");
            GenerateData().Wait();

            Console.Read();
        }
        private async static Task QueryGroups()
        {
            var groupClient = new GroupClient(pbi);
            var groups = await groupClient.Get();

            foreach (var item in groups.value)
            {
                Console.WriteLine("{0}:{1}", item.name, item.id);
            }
        }

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

        private async static Task QueryDatasets()
        {
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

            var tables = await dataClient.ListTables(created.id);
            ListTables(tables);
            //var table = dataClient.GetTable(created.id, tables.value.First().name);
            //ListColumns(table.columns);

            var table = await dataClient.UpdateTableSchema(created.id, tables.value.First().name, typeof(resourceMeasures2));
        }

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
            await dataClient.ClearRows(created.id, tables.value.First().name);
        }

        private static void ListTables(BISharp.Contracts.Tables tables)
        {
            foreach (var table in tables.value)
            {
                Console.WriteLine(table.name);
                if (table.columns != null)
                    ListColumns(table.columns);
            }
        }

        private static void ListColumns(List<Column> columns)
        {
            foreach (var column in columns)
            {
                Console.WriteLine("{0}\t{1}", column.name, column.dataType);
            }
        }
    }
}
