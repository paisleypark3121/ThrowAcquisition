using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThrowAcquisition.Tests.Controllers
{
    [TestClass]
    public class FunctionsTests
    {
        [TestMethod]
        public void XmlParseTest()
        {
            string xml = "<PageRequestResult xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/LBP_CarrierAPI_BusinessLogic.Model\"><Description>User Error. Generic</Description><RedirectUrl i:nil=\"true\" /><RetCode>GenericUserError</RetCode></PageRequestResult>";
            
            var parser = XElement.Parse(xml);
            var nodes=parser.Elements();
            foreach (XElement item in nodes)
            {
                string name=item.Name.LocalName;
                string value = item.Value;

            }
        }
    }
}
