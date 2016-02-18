using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSSLTool.Compacts;

namespace QSSL_Tool_Testing
{
    /// <summary>
    /// This class contains tests for the QSSLTool.Compact namespace.
    /// </summary>
    [TestClass]
    public class CompactsTests
    {
        [TestMethod]
        public void HostEntryCreation_Positive()
        {
            HostEntry he = new HostEntry(
                "1.1.1.1", "demo.de",
                "https", "C",
                "SHA1", DateTime.Now,
                "False", "False" 
                );

            Assert.IsNotNull(he);
        }

        [TestMethod]
        public void HostEntryCreation_NullParams()
        {
            HostEntry he = new HostEntry(
                null, null,
                null, null,
                null, DateTime.Now,
                null, null
                );

            Assert.AreEqual(he.IP.ToString(), "?");
            Assert.AreEqual(he.URL.ToString(), "?");
            Assert.AreEqual(he.Ranking.ToString(), "?");
            Assert.AreEqual(he.Protocol.ToString(), "?");
            Assert.AreEqual(he.FingerPrintCert.ToString(), "?");
            Assert.AreEqual(he.TLS.ToString(), "?");
            Assert.AreEqual(he.RC4.ToString(), "?");

            Assert.IsNotNull(he.Differences);
            Assert.IsTrue(he.Differences.Count == 0);
        }

        [TestMethod]
        public void HostEntryCreation_Empty()
        {
            HostEntry he = new HostEntry(
                null, "-",
                "https", "C",
                "SHA1", DateTime.Now,
                "False", "False"
                );
            Assert.IsTrue(he.IsEmpty());
        }
    }
}
