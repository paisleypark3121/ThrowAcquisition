using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ThrowAcquisition.Tests.Controllers
{
    [TestClass]
    public class FunctionsTests
    {
        [TestMethod]
        public void XmlParseTest()
        {
            //string xml = "<PageRequestResult xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/LBP_CarrierAPI_BusinessLogic.Model\"><Description>User Error. Generic</Description><RedirectUrl i:nil=\"true\" /><RetCode>GenericUserError</RetCode></PageRequestResult>";
            string xml="<ArrayOfCheckSubsResponseItem xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><CheckSubsResponseItem><CarrierID>1</CarrierID><DeactivationDateTime>0</DeactivationDateTime><Description>Request successful. User is Subscribed.</Description><RetCode>1000</RetCode><ServiceID>289</ServiceID><ServiceName>GirlsInAction</ServiceName><SubscriberID>14282202</SubscriberID><SubscriptionDateTime>2020-08-03 09:06:06</SubscriptionDateTime><UserID>1724521</UserID></CheckSubsResponseItem></ArrayOfCheckSubsResponseItem>";

            Dictionary<string, object> response = new Dictionary<string, object>();

            var parser = XElement.Parse(xml);
            var nodes=parser.Elements();
            foreach (XElement item in nodes)
            {
                var internal_nodes = item.Elements();
                Dictionary<string, object> _response = new Dictionary<string, object>();
                foreach (XElement internal_item in internal_nodes)
                {
                    string name = internal_item.Name.LocalName;
                    string value = internal_item.Value;
                    _response.Add(name, value);
                }
                response.Add(_response["SubscriberID"].ToString(),_response);
            }

            string SubscriberID = null;
            foreach (var element in response)
            {
                string key=element.Key;
                object value = element.Value;
                Dictionary<string, object> _value = (Dictionary<string, object>)value;
                if (_value["RetCode"].ToString() == "1000")
                    SubscriberID = key;
            }

        }
    }
}
