﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AppSettings;
using Call;
using Diagnostic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThrowAcquisition;
using ThrowAcquisition.Controllers;
using ThrowAPI;

namespace ThrowAcquisition.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexTest()
        {
            #region Arrange
            string TokenID = "466a8198-8ad0-11ea-bd15-005056917e4a";
            string password = "pvskpn8dbfkhe9o";
            string expected_PartnerID = "366";
            string expected_ServID = "300";
            string murp_URL = "http://www.servicelayer.mobi:9090/stg3/murp.ashx";
            string Login_URL="http://stg3.lanciobp.net:4700/api/Login";
            string ActivationRequest_URL = "http://stg3.lanciobp.net:4700/api/PageRequest";
            string DeactivationRequest_URL = "http://stg3.lanciobp.net:4700/api/Deactivate";
            string EndUserManagement_URL = "http://stg3.lanciobp.net:4700/api/";
            string OTP_URL = "http://www.servicelayer.mobi:9090/STG3/otp.ashx";
            string CheckSubs_URL = "http://stg3.lanciobp.net:4700/api/CheckSubs";

            string domain_url = "astrotoday.org";
            string expected_basereturn = "http://" + domain_url + "/BR";
            var queryString = new NameValueCollection
            {
                //{ "sid", "300" },{"ch","ch_test"},{"tid","2222"}
            };

            IService service = new Service();

            Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
            mock_trace.Setup(x => x.trace(It.IsAny<string>()));
            mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

            Mock<ILogin> mock_login = new Mock<ILogin>();
            mock_login.Setup(x => x.GetTokenID()).Returns(TokenID);

            Mock<ICall> mock_call = new Mock<ICall>();
            mock_call.Setup(x => x.doCall(
                It.IsAny<object>(),It.IsAny<CallMethod>(),It.IsAny<CallContentType>(),It.IsAny<string>(),It.IsAny<IResponseParser>(),It.IsAny<string>())
            ).Returns(new Dictionary<string, object>() 
            {
                {"RetCode","OK"},
                {"Description","TEST"},
            });

            var mock_appSettings = new Mock<IAppSettings>();
            mock_appSettings.Setup(x => x["Login_URL"]).Returns(Login_URL);
            mock_appSettings.Setup(x => x["PartnerID"]).Returns(expected_PartnerID);
            mock_appSettings.Setup(x => x["Password"]).Returns(password);
            mock_appSettings.Setup(x => x["murp_URL"]).Returns(murp_URL);
            mock_appSettings.Setup(x => x["ActivationRequest_URL"]).Returns(ActivationRequest_URL);
            mock_appSettings.Setup(x => x["DeactivationRequest_URL"]).Returns(DeactivationRequest_URL);
            mock_appSettings.Setup(x => x["EndUserManagement_URL"]).Returns(EndUserManagement_URL);
            mock_appSettings.Setup(x => x["OTP_URL"]).Returns(OTP_URL);
            mock_appSettings.Setup(x => x["CheckSubs_URL"]).Returns(CheckSubs_URL);

            IEndUser endUser = new EndUser(mock_appSettings.Object, mock_call.Object, mock_trace.Object, mock_login.Object);

            var mock_request = new Mock<HttpRequestBase>();
            mock_request.Setup(x => x.HttpMethod).Returns("GET");
            mock_request.Setup(x => x.Url).Returns(new Uri("http://" + domain_url));
            mock_request.SetupGet(x => x.QueryString).Returns(queryString);
            mock_request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    //{"X-Requested-With", "XMLHttpRequest"}
                    {"X-Requested-With", "SAFE"}
                });

            HomeController controller = new HomeController(mock_trace.Object, endUser, service);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mock_request.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);
            #endregion

            #region Act
            var actual = controller.Index() as RedirectResult;
            string actual_PartnerId = HttpUtility.ParseQueryString(new Uri(actual.Url).Query).Get("PartnerID");
            string actual_BaseReturnURL = HttpUtility.ParseQueryString(new Uri(actual.Url).Query).Get("BaseReturnURL");
            string actual_ServID = HttpUtility.ParseQueryString(new Uri(actual.Url).Query).Get("ServID");
            #endregion

            #region Assert
            Assert.AreEqual(expected_PartnerID, actual_PartnerId);
            Assert.AreEqual(expected_ServID, actual_ServID);
            Assert.IsTrue(actual_BaseReturnURL.Contains(expected_basereturn));
            #endregion
        }

        [TestMethod]
        public void BRTest()
        {
            #region Arrange
            string id = "300_test_637269555781507629";
            var queryString = new NameValueCollection {
                { "EndUserID", "1111" },{"CarrID","1"},{"RetCode","1000"},{"TmpEndUserID","1111"}
            };
            string expected = "http://it.astrotoday.prodactive.it/?ticket=anubi";

            string TokenID = "466a8198-8ad0-11ea-bd15-005056917e4a";
            string password = "pvskpn8dbfkhe9o";
            string expected_PartnerID = "366";
            string murp_URL = "http://www.servicelayer.mobi:9090/stg3/murp.ashx";
            string Login_URL = "http://stg3.lanciobp.net:4700/api/Login";
            string ActivationRequest_URL = "http://stg3.lanciobp.net:4700/api/PageRequest";
            string DeactivationRequest_URL = "http://stg3.lanciobp.net:4700/api/Deactivate";
            string EndUserManagement_URL = "http://stg3.lanciobp.net:4700/api/";
            string OTP_URL = "http://www.servicelayer.mobi:9090/STG3/otp.ashx";
            string CheckSubs_URL = "http://stg3.lanciobp.net:4700/api/CheckSubs";

            string domain_url = "astrotoday.org";
            string expected_basereturn = "http://" + domain_url + "/BR";

            IService service = new Service();

            Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
            mock_trace.Setup(x => x.trace(It.IsAny<string>()));
            mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

            Mock<ILogin> mock_login = new Mock<ILogin>();
            mock_login.Setup(x => x.GetTokenID()).Returns(TokenID);

            Mock<ICall> mock_call = new Mock<ICall>();
            mock_call.Setup(x => x.doCall(
                It.IsAny<object>(), It.IsAny<CallMethod>(), It.IsAny<CallContentType>(), It.IsAny<string>(), It.IsAny<IResponseParser>(), It.IsAny<string>())
            ).Returns(new Dictionary<string, object>()
            {
                {"RetCode","OK"},
                {"Description","TEST"},
            });

            var mock_appSettings = new Mock<IAppSettings>();
            mock_appSettings.Setup(x => x["Login_URL"]).Returns(Login_URL);
            mock_appSettings.Setup(x => x["PartnerID"]).Returns(expected_PartnerID);
            mock_appSettings.Setup(x => x["Password"]).Returns(password);
            mock_appSettings.Setup(x => x["murp_URL"]).Returns(murp_URL);
            mock_appSettings.Setup(x => x["ActivationRequest_URL"]).Returns(ActivationRequest_URL);
            mock_appSettings.Setup(x => x["DeactivationRequest_URL"]).Returns(DeactivationRequest_URL);
            mock_appSettings.Setup(x => x["EndUserManagement_URL"]).Returns(EndUserManagement_URL);
            mock_appSettings.Setup(x => x["OTP_URL"]).Returns(OTP_URL);
            mock_appSettings.Setup(x => x["CheckSubs_URL"]).Returns(CheckSubs_URL);

            IEndUser endUser = new EndUser(mock_appSettings.Object, mock_call.Object, mock_trace.Object, mock_login.Object);

            var mock_request = new Mock<HttpRequestBase>();
            mock_request.Setup(x => x.HttpMethod).Returns("GET");
            mock_request.Setup(x => x.Url).Returns(new Uri("http://" + domain_url));
            mock_request.SetupGet(x => x.QueryString).Returns(queryString);
            mock_request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    //{"X-Requested-With", "XMLHttpRequest"}
                    {"X-Requested-With", "SAFE"}
                });

            HomeController controller = new HomeController(mock_trace.Object, endUser, service);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mock_request.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);
            #endregion

            #region Act
            var actual = controller.BR(id) as RedirectResult;
            #endregion

            #region Assert
            Assert.AreEqual(expected, actual.Url);
            #endregion
        }

        [TestMethod]
        public void BRLancioTest()
        {
            #region Arrange
            string id = "300_test_637269555781507629";
            var queryString = new NameValueCollection {
                { "EndUserID", "1111" },{"CarrID","4"},{"RetCode","1000"},{"TmpEndUserID","1111"}
            };
            string expected = "http://ok_URL";

            string TokenID = "466a8198-8ad0-11ea-bd15-005056917e4a";
            string password = "pvskpn8dbfkhe9o";
            string expected_PartnerID = "366";
            string murp_URL = "http://www.servicelayer.mobi:9090/stg3/murp.ashx";
            string Login_URL = "http://stg3.lanciobp.net:4700/api/Login";
            string ActivationRequest_URL = "http://stg3.lanciobp.net:4700/api/PageRequest";
            string DeactivationRequest_URL = "http://stg3.lanciobp.net:4700/api/Deactivate";
            string EndUserManagement_URL = "http://stg3.lanciobp.net:4700/api/";
            string OTP_URL = "http://www.servicelayer.mobi:9090/STG3/otp.ashx";
            string CheckSubs_URL = "http://stg3.lanciobp.net:4700/api/CheckSubs";

            string domain_url = "astrotoday.org";
            string expected_basereturn = "http://" + domain_url + "/BR";

            IService service = new Service();

            Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
            mock_trace.Setup(x => x.trace(It.IsAny<string>()));
            mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

            Mock<ILogin> mock_login = new Mock<ILogin>();
            mock_login.Setup(x => x.GetTokenID()).Returns(TokenID);

            Mock<ICall> mock_call = new Mock<ICall>();
            mock_call.Setup(x => x.doCall(
                It.IsAny<object>(), It.IsAny<CallMethod>(), It.IsAny<CallContentType>(), It.IsAny<string>(), It.IsAny<IResponseParser>(), It.IsAny<string>())
            ).Returns(new Dictionary<string, object>()
            {
                {"RetCode","1010"},
                {"Description","OK"},
                {"RedirectUrl",expected},
            });

            var mock_appSettings = new Mock<IAppSettings>();
            mock_appSettings.Setup(x => x["Login_URL"]).Returns(Login_URL);
            mock_appSettings.Setup(x => x["PartnerID"]).Returns(expected_PartnerID);
            mock_appSettings.Setup(x => x["Password"]).Returns(password);
            mock_appSettings.Setup(x => x["murp_URL"]).Returns(murp_URL);
            mock_appSettings.Setup(x => x["ActivationRequest_URL"]).Returns(ActivationRequest_URL);
            mock_appSettings.Setup(x => x["DeactivationRequest_URL"]).Returns(DeactivationRequest_URL);
            mock_appSettings.Setup(x => x["EndUserManagement_URL"]).Returns(EndUserManagement_URL);
            mock_appSettings.Setup(x => x["OTP_URL"]).Returns(OTP_URL);
            mock_appSettings.Setup(x => x["CheckSubs_URL"]).Returns(CheckSubs_URL);

            IEndUser endUser = new EndUser(mock_appSettings.Object, mock_call.Object, mock_trace.Object, mock_login.Object);

            var mock_request = new Mock<HttpRequestBase>();
            mock_request.Setup(x => x.HttpMethod).Returns("GET");
            mock_request.Setup(x => x.Url).Returns(new Uri("http://" + domain_url));
            mock_request.SetupGet(x => x.QueryString).Returns(queryString);
            mock_request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    //{"X-Requested-With", "XMLHttpRequest"}
                    {"X-Requested-With", "SAFE"}
                });

            HomeController controller = new HomeController(mock_trace.Object, endUser, service);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mock_request.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);
            #endregion

            #region Act
            var actual = controller.BR(id) as RedirectResult;
            #endregion

            #region Assert
            Assert.AreEqual(expected, actual.Url);
            #endregion
        }

        [TestMethod]
        public void New_Redirect2CatalogueTest()
        {
            #region Arrange
            string TokenID = "466a8198-8ad0-11ea-bd15-005056917e4a";
            string PartnerID = "366";
            string password = "pvskpn8dbfkhe9o";
            string murp_URL = "http://www.servicelayer.mobi:9090/stg3/murp.ashx";
            string Login_URL = "http://stg3.lanciobp.net:4700/api/Login";
            string ActivationRequest_URL = "http://stg3.lanciobp.net:4700/api/PageRequest";
            string DeactivationRequest_URL = "http://stg3.lanciobp.net:4700/api/Deactivate";
            string EndUserManagement_URL = "http://stg3.lanciobp.net:4700/api/";
            string OTP_URL = "http://www.servicelayer.mobi:9090/STG3/otp.ashx";
            string CheckSubs_URL = "http://stg3.lanciobp.net:4700/api/CheckSubs";
            string GetMSISDN_URL = "http://stg3.lanciobp.net:4700/api/GetMSISDN";

            string domain_url = "new.astrotoday.org";
            string expected = "new2.astrotoday.org";
            var queryString = new NameValueCollection
            {
                //{ "sid", "300" },{"ch","ch_test"},{"tid","2222"}
            };

            IService service = new Service();

            Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
            mock_trace.Setup(x => x.trace(It.IsAny<string>()));
            mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

            Mock<ILogin> mock_login = new Mock<ILogin>();
            mock_login.Setup(x => x.GetTokenID()).Returns(TokenID);

            Mock<ICall> mock_call = new Mock<ICall>();

            var mock_appSettings = new Mock<IAppSettings>();
            mock_appSettings.Setup(x => x["Login_URL"]).Returns(Login_URL);
            mock_appSettings.Setup(x => x["PartnerID"]).Returns(PartnerID);
            mock_appSettings.Setup(x => x["Password"]).Returns(password);
            mock_appSettings.Setup(x => x["murp_URL"]).Returns(murp_URL);
            mock_appSettings.Setup(x => x["ActivationRequest_URL"]).Returns(ActivationRequest_URL);
            mock_appSettings.Setup(x => x["DeactivationRequest_URL"]).Returns(DeactivationRequest_URL);
            mock_appSettings.Setup(x => x["EndUserManagement_URL"]).Returns(EndUserManagement_URL);
            mock_appSettings.Setup(x => x["OTP_URL"]).Returns(OTP_URL);
            mock_appSettings.Setup(x => x["CheckSubs_URL"]).Returns(CheckSubs_URL);
            mock_appSettings.Setup(x => x["GetMSISDN_URL"]).Returns(GetMSISDN_URL);

            IEndUser endUser = new EndUser(mock_appSettings.Object, mock_call.Object, mock_trace.Object, mock_login.Object);

            var mock_request = new Mock<HttpRequestBase>();
            mock_request.Setup(x => x.HttpMethod).Returns("GET");
            mock_request.Setup(x => x.Url).Returns(new Uri("http://" + domain_url));
            mock_request.SetupGet(x => x.QueryString).Returns(queryString);
            mock_request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    //{"X-Requested-With", "XMLHttpRequest"}
                    {"X-Requested-With", "SAFE"}
                });

            HomeController controller = new HomeController(mock_trace.Object, endUser, service);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mock_request.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);
            #endregion

            #region Act
            var actual = controller.Index() as RedirectResult;
            #endregion

            #region Assert
            Assert.AreEqual(expected, actual.Url);
            #endregion
        }

        [TestMethod]
        public void New_Redirect2CatalogueNotModeratedTest()
        {
            #region Arrange
            string TokenID = "466a8198-8ad0-11ea-bd15-005056917e4a";
            string PartnerID = "366";
            string password = "pvskpn8dbfkhe9o";
            string murp_URL = "http://www.servicelayer.mobi:9090/stg3/murp.ashx";
            string Login_URL = "http://stg3.lanciobp.net:4700/api/Login";
            string ActivationRequest_URL = "http://stg3.lanciobp.net:4700/api/PageRequest";
            string DeactivationRequest_URL = "http://stg3.lanciobp.net:4700/api/Deactivate";
            string EndUserManagement_URL = "http://stg3.lanciobp.net:4700/api/";
            string OTP_URL = "http://www.servicelayer.mobi:9090/STG3/otp.ashx";
            string CheckSubs_URL = "http://stg3.lanciobp.net:4700/api/CheckSubs";
            string GetMSISDN_URL = "http://stg3.lanciobp.net:4700/api/GetMSISDN";

            string domain_url = "new.astrotoday.org";
            string expected = "new2.astrotoday.org";
            var queryString = new NameValueCollection
            {
                { "CarrID", "1" }
            };

            IService service = new Service();

            Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
            mock_trace.Setup(x => x.trace(It.IsAny<string>()));
            mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

            Mock<ILogin> mock_login = new Mock<ILogin>();
            mock_login.Setup(x => x.GetTokenID()).Returns(TokenID);

            Mock<ICall> mock_call = new Mock<ICall>();

            var mock_appSettings = new Mock<IAppSettings>();
            mock_appSettings.Setup(x => x["Login_URL"]).Returns(Login_URL);
            mock_appSettings.Setup(x => x["PartnerID"]).Returns(PartnerID);
            mock_appSettings.Setup(x => x["Password"]).Returns(password);
            mock_appSettings.Setup(x => x["murp_URL"]).Returns(murp_URL);
            mock_appSettings.Setup(x => x["ActivationRequest_URL"]).Returns(ActivationRequest_URL);
            mock_appSettings.Setup(x => x["DeactivationRequest_URL"]).Returns(DeactivationRequest_URL);
            mock_appSettings.Setup(x => x["EndUserManagement_URL"]).Returns(EndUserManagement_URL);
            mock_appSettings.Setup(x => x["OTP_URL"]).Returns(OTP_URL);
            mock_appSettings.Setup(x => x["CheckSubs_URL"]).Returns(CheckSubs_URL);
            mock_appSettings.Setup(x => x["GetMSISDN_URL"]).Returns(GetMSISDN_URL);

            IEndUser endUser = new EndUser(mock_appSettings.Object, mock_call.Object, mock_trace.Object, mock_login.Object);

            var mock_request = new Mock<HttpRequestBase>();
            mock_request.Setup(x => x.HttpMethod).Returns("GET");
            mock_request.Setup(x => x.Url).Returns(new Uri("http://" + domain_url));
            mock_request.SetupGet(x => x.QueryString).Returns(queryString);
            mock_request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    //{"X-Requested-With", "XMLHttpRequest"}
                    {"X-Requested-With", "SAFE"}
                });

            HomeController controller = new HomeController(mock_trace.Object, endUser, service);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mock_request.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);
            #endregion

            #region Act
            var actual = controller.Index() as RedirectResult;
            #endregion

            #region Assert
            Assert.AreEqual(expected, actual.Url);
            #endregion
        }

        [TestMethod]
        public void New_Redirect2CatalogueModeratedNotEndUserIDTest()
        {
            #region Arrange
            string TokenID = "466a8198-8ad0-11ea-bd15-005056917e4a";
            string PartnerID = "366";
            string password = "pvskpn8dbfkhe9o";
            string murp_URL = "http://www.servicelayer.mobi:9090/stg3/murp.ashx";
            string Login_URL = "http://stg3.lanciobp.net:4700/api/Login";
            string ActivationRequest_URL = "http://stg3.lanciobp.net:4700/api/PageRequest";
            string DeactivationRequest_URL = "http://stg3.lanciobp.net:4700/api/Deactivate";
            string EndUserManagement_URL = "http://stg3.lanciobp.net:4700/api/";
            string OTP_URL = "http://www.servicelayer.mobi:9090/STG3/otp.ashx";
            string CheckSubs_URL = "http://stg3.lanciobp.net:4700/api/CheckSubs";
            string GetMSISDN_URL = "http://stg3.lanciobp.net:4700/api/GetMSISDN";

            string domain_url = "new.ragazzelibere.fun";
            string expected = "www.ragazzelibere.fun";
            var queryString = new NameValueCollection
            {
                { "CarrID", "1" }
            };

            IService service = new Service();

            Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
            mock_trace.Setup(x => x.trace(It.IsAny<string>()));
            mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

            Mock<ILogin> mock_login = new Mock<ILogin>();
            mock_login.Setup(x => x.GetTokenID()).Returns(TokenID);

            Mock<ICall> mock_call = new Mock<ICall>();

            var mock_appSettings = new Mock<IAppSettings>();
            mock_appSettings.Setup(x => x["Login_URL"]).Returns(Login_URL);
            mock_appSettings.Setup(x => x["PartnerID"]).Returns(PartnerID);
            mock_appSettings.Setup(x => x["Password"]).Returns(password);
            mock_appSettings.Setup(x => x["murp_URL"]).Returns(murp_URL);
            mock_appSettings.Setup(x => x["ActivationRequest_URL"]).Returns(ActivationRequest_URL);
            mock_appSettings.Setup(x => x["DeactivationRequest_URL"]).Returns(DeactivationRequest_URL);
            mock_appSettings.Setup(x => x["EndUserManagement_URL"]).Returns(EndUserManagement_URL);
            mock_appSettings.Setup(x => x["OTP_URL"]).Returns(OTP_URL);
            mock_appSettings.Setup(x => x["CheckSubs_URL"]).Returns(CheckSubs_URL);
            mock_appSettings.Setup(x => x["GetMSISDN_URL"]).Returns(GetMSISDN_URL);

            IEndUser endUser = new EndUser(mock_appSettings.Object, mock_call.Object, mock_trace.Object, mock_login.Object);

            var mock_request = new Mock<HttpRequestBase>();
            mock_request.Setup(x => x.HttpMethod).Returns("GET");
            mock_request.Setup(x => x.Url).Returns(new Uri("http://" + domain_url));
            mock_request.SetupGet(x => x.QueryString).Returns(queryString);
            mock_request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    //{"X-Requested-With", "XMLHttpRequest"}
                    {"X-Requested-With", "SAFE"}
                });

            HomeController controller = new HomeController(mock_trace.Object, endUser, service);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mock_request.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);
            #endregion

            #region Act
            var actual = controller.Index() as RedirectResult;
            #endregion

            #region Assert
            Assert.AreEqual(expected, actual.Url);
            #endregion
        }

        [TestMethod]
        public void New_Redirect2CatalogueModeratedOKTest()
        {
            #region Arrange
            string TokenID = "466a8198-8ad0-11ea-bd15-005056917e4a";
            string PartnerID = "366";
            string password = "pvskpn8dbfkhe9o";
            string murp_URL = "http://www.servicelayer.mobi:9090/stg3/murp.ashx";
            string Login_URL = "http://stg3.lanciobp.net:4700/api/Login";
            string ActivationRequest_URL = "http://stg3.lanciobp.net:4700/api/PageRequest";
            string DeactivationRequest_URL = "http://stg3.lanciobp.net:4700/api/Deactivate";
            string EndUserManagement_URL = "http://stg3.lanciobp.net:4700/api/";
            string OTP_URL = "http://www.servicelayer.mobi:9090/STG3/otp.ashx";
            string CheckSubs_URL = "http://stg3.lanciobp.net:4700/api/CheckSubs";
            string GetMSISDN_URL = "http://stg3.lanciobp.net:4700/api/GetMSISDN";

            string domain_url = "new.ragazzelibere.fun";
            string expected = "new2.ragazzelibere.fun?EndUserID=1234&CarrID=1";
            var queryString = new NameValueCollection
            {
                { "endUserID", "1234" },
                { "CarrID", "1" }
            };

            IService service = new Service();

            Mock<IDiagnostic> mock_trace = new Mock<IDiagnostic>();
            mock_trace.Setup(x => x.trace(It.IsAny<string>()));
            mock_trace.Setup(x => x.traceError(It.IsAny<string>()));

            Mock<ILogin> mock_login = new Mock<ILogin>();
            mock_login.Setup(x => x.GetTokenID()).Returns(TokenID);

            Mock<ICall> mock_call = new Mock<ICall>();

            var mock_appSettings = new Mock<IAppSettings>();
            mock_appSettings.Setup(x => x["Login_URL"]).Returns(Login_URL);
            mock_appSettings.Setup(x => x["PartnerID"]).Returns(PartnerID);
            mock_appSettings.Setup(x => x["Password"]).Returns(password);
            mock_appSettings.Setup(x => x["murp_URL"]).Returns(murp_URL);
            mock_appSettings.Setup(x => x["ActivationRequest_URL"]).Returns(ActivationRequest_URL);
            mock_appSettings.Setup(x => x["DeactivationRequest_URL"]).Returns(DeactivationRequest_URL);
            mock_appSettings.Setup(x => x["EndUserManagement_URL"]).Returns(EndUserManagement_URL);
            mock_appSettings.Setup(x => x["OTP_URL"]).Returns(OTP_URL);
            mock_appSettings.Setup(x => x["CheckSubs_URL"]).Returns(CheckSubs_URL);
            mock_appSettings.Setup(x => x["GetMSISDN_URL"]).Returns(GetMSISDN_URL);

            IEndUser endUser = new EndUser(mock_appSettings.Object, mock_call.Object, mock_trace.Object, mock_login.Object);

            var mock_request = new Mock<HttpRequestBase>();
            mock_request.Setup(x => x.HttpMethod).Returns("GET");
            mock_request.Setup(x => x.Url).Returns(new Uri("http://" + domain_url));
            mock_request.SetupGet(x => x.QueryString).Returns(queryString);
            mock_request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
                    //{"X-Requested-With", "XMLHttpRequest"}
                    {"X-Requested-With", "SAFE"}
                });

            HomeController controller = new HomeController(mock_trace.Object, endUser, service);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(mock_request.Object);

            controller.ControllerContext = new ControllerContext(context.Object, new System.Web.Routing.RouteData(), controller);
            #endregion

            #region Act
            var actual = controller.Index() as RedirectResult;
            #endregion

            #region Assert
            Assert.AreEqual(expected, actual.Url);
            #endregion
        }
    }
}
