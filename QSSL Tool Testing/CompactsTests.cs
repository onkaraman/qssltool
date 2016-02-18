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

        [TestMethod]
        public void HostEntry_Differences()
        {
            HostEntry a = new HostEntry(
                "1.1.1.1", "demo.de",
                "https", "C",
                "SHA1", DateTime.Now,
                "False", "False"
                );

            HostEntry b = new HostEntry(
                "1.1.1.2", "demo.com",
                "https", "C",
                "SHA1", DateTime.Now,
                "False", "False"
                );

            a.CheckDifferences(b);
            Assert.IsTrue(a.Differences.Count == 2);
            Assert.IsTrue(a.HasDifference("IP"));
            Assert.IsTrue(a.HasDifference("URL"));
        }

        [TestMethod]
        public void HostEntry_NoDifferences()
        {
            HostEntry a = new HostEntry(
                "1.1.1.1", "demo.de",
                "https", "C",
                "SHA1", DateTime.Now,
                "False", "False"
                );

            a.CheckDifferences(a);
            Assert.IsTrue(a.Differences.Count == 0);
        }

        [TestMethod]
        public void HostEntry_AddDifference()
        {
            HostEntry a = new HostEntry(
               "1.1.1.1", "demo.de",
               "https", "C", "SHA1", DateTime.Now,
               "False", "False"
               );

            a.AddDifference("a", "b");
            Assert.IsTrue(a.Differences.Count > 0);
        }

        [TestMethod]
        public void HostEntry_AddDifferenceEmpty()
        {
            HostEntry a = new HostEntry(
               "1.1.1.1", "demo.de",
               "https", "C", "SHA1", DateTime.Now,
               "False", "False"
               );

            a.AddDifference(null, "b");
            a.AddDifference("a", null);
            Assert.IsTrue(a.Differences.Count == 0);
        }

        [TestMethod]
        public void HostEntryAttribute_Positive()
        {
            string ip = "1.1.1.1";
            HostEntryAttribute hea = 
                new HostEntryAttribute(HostEntryAttribute.AttributeType.IP, 
                ip);
            Assert.AreEqual(ip, hea.ToString());
        }

        [TestMethod]
        public void HostEntryAttribute_Negative()
        {
            HostEntryAttribute hea =
                new HostEntryAttribute(HostEntryAttribute.AttributeType.IP,
                null);
            Assert.AreEqual("?", hea.ToString());
        }

        [TestMethod]
        public void HostEntryAttribute_Equals()
        {
            string ip = "1.1.1.1";
            HostEntryAttribute.AttributeType type = 
                HostEntryAttribute.AttributeType.IP;
            HostEntryAttribute hea = new HostEntryAttribute(type, ip);
            HostEntryAttribute hea2 = new HostEntryAttribute(type, ip);

            Assert.AreEqual(hea, hea2);
        }
    }
}
