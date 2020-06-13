using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AppSettings;
using Diagnostic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThrowAcquisition;
using ThrowAcquisition.Controllers;
using ThrowAPI;
using Environment = ThrowAPI.Environment;

namespace ThrowAcquisition.Tests.Controllers
{
    [TestClass]
    public class AcquisitionControllerTest
    {
        //[TestMethod]
        //public void EntryPointTest()
        //{
        //    #region Arrange
        //    string TokenID = "466a8198-8ad0-11ea-bd15-005056917e4a";
        //    string expected = "http://www.servicelayer.mobi:9090/stg3/murp.ashx?PartnerID=1&TokenID=1234321&BaseReturnURL=http%3a%2f%2fthrowacquisition.azurewebsites.net%2facquisition%2fbasereturn%2f1111_ch_test_2222_tmplid_1234&ServID=1111";
        //    var queryString = new NameValueCollection {
        //        { "sid", "300" },{"ch","ch_test"},{"tid","2222"}
        //    };

        //    IService service = new Service();

        //    Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
        //    mock_trace.Setup(x => x.trace(It.IsAny<string>()));
        //    mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

        //    Mock<ILogin> mock_login = new Mock<ILogin>();
        //    mock_login.Setup(x => x.GetTokenID()).Returns(TokenID);

        //    var mock_appSettings = new Mock<IAppSettings>();
        //    mock_appSettings.Setup(x => x["Login_URL"]).Returns("http://stg3.lanciobp.net:4700/api/Login");
        //    mock_appSettings.Setup(x => x["PartnerID"]).Returns("366");
        //    mock_appSettings.Setup(x => x["Password"]).Returns("pvskpn8dbfkhe9o");
        //    mock_appSettings.Setup(x => x["PageRequest_URL"]).Returns("http://stg3.lanciobp.net:4700/api/PageRequest");
        //    mock_appSettings.Setup(x => x["murp_URL"]).Returns("http://www.servicelayer.mobi:9090/stg3/murp.ashx");

        //    IMobileUserRecognition mobileUserRecognition = new MobileUserRecognition(mock_appSettings.Object,mock_login.Object);
        //    Mock<IActivation> mock_activation = new Mock<IActivation>();

        //    var mock_request = new Mock<HttpRequestBase>();
        //    mock_request.Setup(x => x.HttpMethod).Returns("GET");
        //    mock_request.SetupGet(x => x.QueryString).Returns(queryString);
        //    mock_request.SetupGet(x => x.Headers).Returns(
        //        new System.Net.WebHeaderCollection {
        //            //{"X-Requested-With", "XMLHttpRequest"}
        //            {"X-Requested-With", "SAFE"}
        //        });

        //    AcquisitionController controller = new AcquisitionController(mock_trace.Object, mobileUserRecognition, mock_activation.Object,service);
        //    var context = new Mock<HttpContextBase>();
        //    context.SetupGet(x => x.Request).Returns(mock_request.Object);
            
        //    controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);            
        //    #endregion

        //    #region Act
        //    var actual = controller.EntryPoint() as RedirectResult;
        //    #endregion

        //    #region Assert
        //    Assert.AreEqual(expected,actual.Url);
        //    #endregion
        //}

        //[TestMethod]
        //public void BaseReturnTest()
        //{
        //    #region Arrange
        //    string id = "1111_chtest_2222_tmplid1234";
        //    string expected = "http://CARRIER_PAGE";

        //    IService service = new Service();

        //    Dictionary<string, object> expectedPageRequest = new Dictionary<string, object>()
        //    {
        //        {"RetCode","1010"},
        //        {"Description","Url is ok"},
        //        {"RedirectUrl","http://CARRIER_PAGE"}
        //    };

        //    var queryString = new NameValueCollection {
        //        { "EndUserID", "1111" },{"CarrID","100"},{"RetCode","1000"},{"TmpEndUserID","1111"}
        //    };

        //    Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
        //    mock_trace.Setup(x => x.trace(It.IsAny<string>()));
        //    mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

        //    Mock<IMobileUserRecognition> mock_mobileUserRecognition = new Mock<IMobileUserRecognition>();

        //    var mock_appSettings = new Mock<IAppSettings>();
        //    mock_appSettings.Setup(x => x["Login_URL"]).Returns("http://stg3.lanciobp.net:4700/api/Login");
        //    mock_appSettings.Setup(x => x["PartnerID"]).Returns("366");
        //    mock_appSettings.Setup(x => x["Password"]).Returns("pvskpn8dbfkhe9o");
        //    mock_appSettings.Setup(x => x["PageRequest_URL"]).Returns("http://stg3.lanciobp.net:4700/api/PageRequest");
        //    mock_appSettings.Setup(x => x["murp_URL"]).Returns("http://www.servicelayer.mobi:9090/stg3/murp.ashx");

        //    Mock<IActivation> mock_activation = new Mock<IActivation>();
        //    mock_activation.Setup(x => x.PageRequest(It.IsAny<Dictionary<string, object>>())).Returns(expectedPageRequest); ;
            
        //    var mock_request = new Mock<HttpRequestBase>();
        //    mock_request.Setup(x => x.HttpMethod).Returns("GET");
        //    mock_request.SetupGet(x => x.QueryString).Returns(queryString);
        //    mock_request.SetupGet(x => x.Headers).Returns(
        //        new System.Net.WebHeaderCollection {
        //            //{"X-Requested-With", "XMLHttpRequest"}
        //            {"X-Requested-With", "SAFE"}
        //        });

        //    AcquisitionController controller = new AcquisitionController(mock_trace.Object, mock_mobileUserRecognition.Object, mock_activation.Object,service);
        //    var context = new Mock<HttpContextBase>();
        //    context.SetupGet(x => x.Request).Returns(mock_request.Object);

        //    controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);
        //    #endregion

        //    #region Act
        //    var actual = controller.BaseReturn(id) as RedirectResult;
        //    #endregion

        //    #region Assert
        //    Assert.AreEqual(expected, actual.Url);
        //    #endregion
        //}
    }
}