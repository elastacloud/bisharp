using BISharp.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp.Tests.Authentication
{
    [TestClass]
    public class DashboardClientTests
    {
        [TestMethod]
        public async Task ListDashboard_AllGood_SetsAuthHeader()
        {
            Mock<IPowerBiAuthentication> mockPBi = new Mock<IPowerBiAuthentication>();
            mockPBi.Setup(t => t.GetAccessToken()).Returns("hello");

            var dashboardClient = new DatasetsClient(mockPBi.Object);
            await dashboardClient.List();

            mockPBi.Verify(pbi => pbi.GetAccessToken(), Times.Once);
        }
    }
}
