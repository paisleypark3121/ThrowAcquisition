using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Diagnostic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThrowAcquisition;
using ThrowAcquisition.Controllers;
using ThrowAPI;

namespace ThrowAcquisition.Tests.Controllers
{
    [TestClass]
    public class AcquisitionControllerTest
    {
        [TestMethod]
        public void EntryPointTest()
        {
            #region Arrange
            string expected = "http://www.servicelayer.mobi:9090/stg3/murp.ashx?PartnerID=1&TokenID=1234321&BaseReturnURL=http%3a%2f%2fthrowacquisition.azurewebsites.net%2facquisition%2fbasereturn%2f1111_ch_test_2222_tmplid_1234&ServID=1111";
            var queryString = new NameValueCollection {
                { "sid", "1111" },{"ch","ch_test"},{"tid","2222"},{"tmplid","tmplid_1234"}
            };
            
            Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
            mock_trace.Setup(x => x.trace(It.IsAny<string>()));
            mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

            Mock<ILogin> mock_login = new Mock<ILogin>();
            mock_login.Setup(x => x.GetTokenID()).Returns("1234321");

            IMobileUserRecognition mobileUserRecognition = new MobileUserRecognition(mock_login.Object);
            Mock<IActivation> mock_activation = new Mock<IActivation>();

            var mock_request = new Mock<HttpRequestBase>();
            mock_request.Setup(x => x.HttpMethod).Returns("GET");
            mock_request.SetupGet(x => x.QueryString).Returns(queryString);
            mock_request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    //{"X-Requested-With", "XMLHttpRequest"}
                    {"X-Requested-With", "SAFE"}
                });

            AcquisitionController controller = new AcquisitionController(mock_trace.Object, mobileUserRecognition, mock_activation.Object);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mock_request.Object);
            
            controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);            
            #endregion

            #region Act
            var actual = controller.EntryPoint() as RedirectResult;
            #endregion

            #region Assert
            Assert.AreEqual(expected,actual.Url);
            #endregion
        }

        [TestMethod]
        public void BaseReturnTest()
        {
            #region Arrange
            string id = "1111_chtest_2222_tmplid1234";
            string expected = "http://CARRIER_PAGE";

            Dictionary<string, object> expectedPageRequest = new Dictionary<string, object>()
            {
                {"RetCode","1010"},
                {"Description","Url is ok"},
                {"RedirectUrl","http://CARRIER_PAGE"}
            };

            var queryString = new NameValueCollection {
                { "EndUserID", "1111" },{"CarrID","100"},{"RetCode","1000"},{"TmpEndUserID","1111"}
            };

            Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
            mock_trace.Setup(x => x.trace(It.IsAny<string>()));
            mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

            Mock<IMobileUserRecognition> mock_mobileUserRecognition = new Mock<IMobileUserRecognition>();

            Mock<IActivation> mock_activation = new Mock<IActivation>();
            mock_activation.Setup(x => x.PageRequest(It.IsAny<Dictionary<string, object>>())).Returns(expectedPageRequest); ;
            
            var mock_request = new Mock<HttpRequestBase>();
            mock_request.Setup(x => x.HttpMethod).Returns("GET");
            mock_request.SetupGet(x => x.QueryString).Returns(queryString);
            mock_request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    //{"X-Requested-With", "XMLHttpRequest"}
                    {"X-Requested-With", "SAFE"}
                });

            AcquisitionController controller = new AcquisitionController(mock_trace.Object, mock_mobileUserRecognition.Object, mock_activation.Object);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mock_request.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);
            #endregion

            #region Act
            var actual = controller.BaseReturn(id) as RedirectResult;
            #endregion

            #region Assert
            Assert.AreEqual(expected, actual.Url);
            #endregion
        }
    }
}
