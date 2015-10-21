using BISharp.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISharp.Tests.DataSchema
{
    [TestClass]
    public class ColumnsFromTypeTests
    {
        [TestMethod]
        public void KnownClass_FromType_GoodName()
        {
            var type = typeof(Uri);
            var actual = Table.FromType(type);

            Assert.AreEqual(actual.name, "Uri");
        }
        [TestMethod]
        public void KnownClass_FromType_NonnullColumns()
        {
            var type = typeof(Uri);
            var actual = Table.FromType(type);

            Assert.IsNotNull(actual.columns);
        }
        [TestMethod]
        public void KnownClass_FromType_ColumnContained()
        {
            var type = typeof(Uri);
            var actual = Table.FromType(type);

            Assert.AreEqual("AbsolutePath", actual.columns.First().name);
        }
        [TestMethod]
        public void KnownClass_FromType_ColumnContainedByType()
        {
            var type = typeof(Uri);
            var actual = Table.FromType(type);

            Assert.AreEqual("String", actual.columns.First().dataType);
        }
    }
}
