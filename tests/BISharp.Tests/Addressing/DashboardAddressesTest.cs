﻿using BISharp.Addressing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp.Tests.Addressing
{
    [TestClass]
    public class DashboardAddressesTest
    {
        [TestMethod]
        public void NoGroupId_GetDashboards_ValidUrl()
        {
            var expected = "beta/myorg/dashboards";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDashboards(string.Empty);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NoGroupId_GetDashboardById_ValidUrl()
        {
            var expected = "beta/myorg/dashboards/{dashboardId}";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDashboard(string.Empty);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NoGroupId_GetDashboardTiles_ValidUrl()
        {
            var expected = "beta/myorg/dashboards/{dashboardId}/tiles";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDashboardTiles(string.Empty);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void NoGroupId_GetDashboardTileById_ValidUrl()
        {
            var expected = "beta/myorg/dashboards/{dashboardId}/tiles/{tileId}";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDashboardTile(string.Empty);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GroupId_GetDashboards_ValidUrl()
        {
            var expected = "beta/myorg/groups/123/dashboards";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDashboards("123");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GroupId_GetDashboardById_ValidUrl()
        {
            var expected = "beta/myorg/groups/123/dashboards/{dashboardId}";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDashboard("123");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GroupId_GetDashboardTiles_ValidUrl()
        {
            var expected = "beta/myorg/groups/123/dashboards/{dashboardId}/tiles";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDashboardTiles("123");

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GroupId_GetDashboardTileById_ValidUrl()
        {
            var expected = "beta/myorg/groups/123/dashboards/{dashboardId}/tiles/{tileId}";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDashboardTile("123");

            Assert.AreEqual(expected, actual);
        }
    }
}
