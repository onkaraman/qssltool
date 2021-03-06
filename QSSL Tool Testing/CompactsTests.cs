﻿using System;
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
            HostEntry he = new HostEntry("demo.de", "https");
            he.SetIP("1.1.1.1");
            he.SetRanking("C");
            he.SetFingerPrintCert("SHA1");
            he.SetExpirationDate(DateTime.Now.ToLongDateString());

            Assert.IsNotNull(he);
        }

        [TestMethod]
        public void HostEntryCreation_Empty()
        {
            HostEntry he = new HostEntry(null, "https");
            he.SetRanking("C");
            he.SetFingerPrintCert("SHA1");
            he.SetExpirationDate(DateTime.Now.ToLongDateString());

            Assert.IsTrue(he.IsEmpty());
        }

        [TestMethod]
        public void HostEntry_Differences()
        {
            HostEntry a = new HostEntry("demo.de", "https");
            a.SetIP("1.1.1.1");
            a.SetRanking("C");
            a.SetFingerPrintCert("SHA1");
            a.SetExpirationDate(DateTime.Now.ToLongDateString());

            HostEntry b = new HostEntry("demo.de", "https");
            b.SetIP("1.1.1.2");
            b.SetRanking("C");
            b.SetFingerPrintCert("SHA1");
            b.SetExpirationDate(DateTime.Now.ToLongDateString());

            a.CheckDifferences(b);
            Assert.IsTrue(a.Differences.Count >= 2);
            Assert.IsTrue(a.HasDifference("IP"));
            Assert.IsTrue(a.HasDifference("URL"));
        }

        [TestMethod]
        public void HostEntry_AddDifference()
        {
            HostEntry he = new HostEntry("demo.de", "https");
            he.SetIP("1.1.1.1");
            he.SetRanking("C");
            he.SetFingerPrintCert("SHA1");
            he.SetExpirationDate(DateTime.Now.ToLongDateString());

            he.AddDifference("a", "b");
            Assert.IsTrue(he.Differences.Count > 0);
        }

        [TestMethod]
        public void HostEntry_AddDifferenceEmpty()
        {
            HostEntry he = new HostEntry("demo.de", "https");
            he.SetIP("1.1.1.1");
            he.SetRanking("C");
            he.SetFingerPrintCert("SHA1");
            he.SetExpirationDate(DateTime.Now.ToLongDateString());

            he.AddDifference(null, "b");
            he.AddDifference("a", null);
            Assert.IsTrue(he.Differences.Count == 0);
        }

        [TestMethod]
        public void HostEntryAttribute_Positive()
        {
            string ip = "1.1.1.1";
            HostEntryAttribute hea = 
                new HostEntryAttribute(HostEntryAttribute.Type.IP, 
                ip);
            Assert.AreEqual(ip, hea.ToString());
        }

        [TestMethod]
        public void HostEntryAttribute_Negative()
        {
            HostEntryAttribute hea =
                new HostEntryAttribute(HostEntryAttribute.Type.IP,
                null);
            Assert.AreEqual("?", hea.ToString());
        }

        [TestMethod]
        public void HostEntryAttribute_Equals()
        {
            string ip = "1.1.1.1";
            HostEntryAttribute.Type type = 
                HostEntryAttribute.Type.IP;
            HostEntryAttribute hea = new HostEntryAttribute(type, ip);
            HostEntryAttribute hea2 = new HostEntryAttribute(type, ip);

            Assert.AreEqual(hea, hea2);
        }
    }
}
