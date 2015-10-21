using BISharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BISharp.Contracts;
using System.Threading;

namespace resourceManagement
{
    class resourceMeasures
    {
        public int RPM { get; set; }
        public double Temperature { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime LastMeasure { get; set; }
        public DateTime InServiceDate { get; set; }
        public bool IsRunning { get; set; }
    }
    class resourceMeasures2
    {
        public int RPM { get; set; }
        public double Temperature { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime LastMeasure { get; set; }
        public DateTime InServiceDate { get; set; }
        public bool IsRunning { get; set; }
        public bool RequiresService { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var dash = new DashboardClient(new PowerBI());
            var dashboards = dash.List();

            foreach (var dashboard in dashboards.value)
            {
                Console.WriteLine("{0}\t{1}", dashboard.displayName, dashboard.id);

                var tiles = dash.Tiles(dashboard.id);
                foreach (var tile in tiles.value)
                {
                    Console.WriteLine(tile.embedUrl);
                }
            }

            var dataClient = new DatasetsClient(new PowerBI());
            var datasets = dataClient.List();

            foreach (var dataset in datasets.value)
            {
                Console.WriteLine("{0}\t{1}", dataset.name, dataset.id);
            }

            Console.WriteLine("Create a data set?");
            Dataset created;
            if (Console.ReadKey().KeyChar == 'Y')
            {
                created = dataClient.Create("resourceManager", true, typeof(resourceMeasures));
            }
            else {
                created = dataClient.List().value.First(ds => ds.name == "resourceManager");
            }

            var tables = dataClient.ListTables(created.id);
            //ListTables(tables);
            //var table = dataClient.GetTable(created.id, tables.value.First().name);
            //ListColumns(table.columns);

            //data.UpdateTable(created.id, tables.value.First().name, typeof(resourceMeasures2));
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
            dataClient.ClearRows(created.id, tables.value.First().name);
            dataClient.AddRows(created.id, tables.value.First().name, dataRows);
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

                dataClient.AddRows(created.id, tables.value.First().name, dataRows);
            }
            dataClient.ClearRows(created.id, tables.value.First().name);

            Console.Read();
        }

        private static void ListTables(BISharp.Contracts.Tables tables)
        {
            foreach (var table in tables.value)
            {
                Console.WriteLine("{0}\t{1}", table.name, table.columns.Count);
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
