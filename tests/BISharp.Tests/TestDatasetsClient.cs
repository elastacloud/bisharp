using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BISharp.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BISharp.Tests
{
    [TestClass]
    class TestDatasetsClient
    {
        private class Row
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        [TestMethod]
        public void TestAddRows()
        {
            var dataset = new Dataset();
            dataset.id = Guid.NewGuid().ToString();

            Mock<IPowerBiAuthentication> auth = new Mock<IPowerBiAuthentication>();
            Mock<DatasetsClient> client = new Mock<DatasetsClient>(auth);
            client.Setup(inst => inst.Create("PowerByTheHour", false, typeof (Row)).Result).Returns(dataset);
            var tableId = client.Object.Create("PowerByTheHour", false, typeof (Row)).Result;

            List<Row> table =  new List<Row>();

            for (int i = 0; i < 5000; i++)
            {
                table.Add(new Row()
                {
                    key = Guid.NewGuid().ToString(),
                    value = Guid.NewGuid().ToString()
                });
            }

            var response = client.Object.AddRows(tableId.id, typeof (Row).FullName, table);
            Assert.IsNotNull(response.Result);

        }
    }
}
