using BISharp.Addressing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp.Tests.Addressing
{
    [TestClass]
    public class DatasetAddressesTest
    {
        #region CreateDatasets
        [TestMethod]
        public void NoGroupId_CreateDataset_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/datasets?defaultRetentionPolicy={defaultRetentionPolicy}";
            var addresses = new PowerBiAddresses();

            var actual = addresses.CreateDataset(string.Empty);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void WithGroupId_CreateDataset_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/groups/123/datasets?defaultRetentionPolicy={defaultRetentionPolicy}";
            var addresses = new PowerBiAddresses();

            var actual = addresses.CreateDataset("123");

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region Get Datasets
        [TestMethod]
        public void NoGroupId_GetDatasets_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/datasets";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDatasets(string.Empty);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void WithGroupId_GetDatasets_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/groups/123/datasets";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDatasets("123");

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region Get Dataset Tables
        [TestMethod]
        public void NoGroupId_GetDatasetTables_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/datasets/{datasetId}/tables";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDatasetTables(string.Empty);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void WithGroupId_GetDatasetTables_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/groups/123/datasets/{datasetId}/tables";
            var addresses = new PowerBiAddresses();

            var actual = addresses.GetDatasetTables("123");

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region Update Table Schema
        [TestMethod]
        public void NoGroupId_UpdateTableSchema_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/datasets/{datasetId}/tables/{tableName}";
            var addresses = new PowerBiAddresses();

            var actual = addresses.UpdateTableSchema(string.Empty);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void WithGroupId_UpdateTableSchema_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/groups/123/datasets/{datasetId}/tables/{tableName}";
            var addresses = new PowerBiAddresses();

            var actual = addresses.UpdateTableSchema("123");

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region Add or Remove Rows
        [TestMethod]
        public void NoGroupId_AddOrRemoveRows_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/datasets/{datasetId}/tables/{tableName}/rows";
            var addresses = new PowerBiAddresses();

            var actual = addresses.AddOrRemoveRows(string.Empty);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void WithGroupId_AddOrRemoveRows_MatchesDocumentation()
        {
            var expected = "v1.0/myorg/groups/123/datasets/{datasetId}/tables/{tableName}/rows";
            var addresses = new PowerBiAddresses();

            var actual = addresses.AddOrRemoveRows("123");

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
