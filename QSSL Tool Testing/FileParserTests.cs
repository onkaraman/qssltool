using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSSLTool.FileParsers;
using SSLLabsApiWrapper.Models.Response.EndpointSubModels;
using System.Collections.Generic;

namespace QSSL_Tool_Testing
{
    [TestClass]
    public class FileParserTests
    {
        [TestMethod]
        public void UnixToDateTime()
        {
            long milseconds = 1451606400000;
            DateTime dt = DataFormatter.Static.UnixToDateTime(milseconds);
            Assert.IsTrue(dt.Year == 2016);
            Assert.IsTrue(dt.Month == 1);
            Assert.IsTrue(dt.Day == 1);
            Assert.IsTrue(dt.Hour == 0);
            Assert.IsTrue(dt.Minute == 0);
        }

        [TestMethod]
        public void TLSListToString()
        {
            Protocol a = new Protocol { name = "TLS", version = "1.0" };
            Protocol b = new Protocol { name = "TLS", version = "1.1" };

            List<Protocol> l = new List<Protocol>();
            l.Add(a);
            l.Add(b);

            string s = DataFormatter.Static.TLSListToString(l);
            Assert.IsTrue(s.Contains(a.version));
            Assert.IsTrue(s.Contains(b.version));
        }
    }
}
