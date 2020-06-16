using Diagnostic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ThrowAPI;

namespace ThrowAcquisition.Controllers
{
    public class HomeController : Controller
    {
        const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0. 2272.118 Safari/537.36.";
        const string ActivationURL = "";
        const string BaseReturnUrl = "/BR";
        const string DisattivazioneBaseReturnUrl = "/DBR";
        const string OTPBaseReturnUrl = "/OTPBR";

        private IDiagnostic trace;
        private IEndUser endUser;
        private IService service;

        public HomeController(
            IDiagnostic _trace,
            IEndUser _endUser,
            IService _service
            )
        {
            trace = _trace;
            endUser = _endUser;
            service = _service;
        }

        public ActionResult Index()
        {
            #region variables
            string request = null;
            string redirectUrl = null;
            #endregion
            #region try
            try
            {
                #region variables setup
                request = ServiceLayer.Utilities.RequestString(Request);
                var queryString = Request.QueryString;
                string Host = Request.Url.Host;
                string RawUrl = Request.RawUrl;
                ViewBag.Host = Host;
                ViewBag.RawUrl = RawUrl;
                string _host = Host;
                if (RawUrl != "/")
                    _host = _host += RawUrl;

                ServiceElement element = service.getByDomain(_host);
                if (element == null)
                    return View();
                #endregion

                #region check AJAX call
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    throw new Exception("Ajax call");
                #endregion

                #region MobileUserRecognition
                string tid = DateTime.Now.Ticks.ToString();
                string id = element.ServiceID + "_test_" + tid;

                redirectUrl = endUser.GetMobileRecognitonUrl(element.ServiceID.ToString(), @"http://" + Host + BaseReturnUrl + @"/" + id);
                #endregion

                #region precondition
                if (string.IsNullOrEmpty(redirectUrl))
                    throw new Exception("Cannot redirect user");
                #endregion

                return Redirect(redirectUrl);
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                trace.traceError(error);
                ViewBag.message = error;
                return View("Error");
            }
            #endregion
            #region finally
            finally
            {
                //DateTime _responseTime = DateTime.Now;
                //LogEntry _logEntry = new LogEntry
                //{
                //    LogName = _LogName,
                //    internal_id = _internal_id,
                //    requestTime = _requestTime,
                //    responseTime = _responseTime,
                //    request = _request,
                //    response = _response,
                //    internal_parameters = JsonConvert.SerializeObject(_internal_parameters)
                //};
                //await log.trackAsync(_logEntry);

                JObject _jobject = new JObject();
                _jobject.Add(new JProperty("header", "Index"));
                _jobject.Add(new JProperty("request", request));
                _jobject.Add(new JProperty("redirectUrl", redirectUrl));
                string message = _jobject.ToString(Formatting.None);
                trace.trace(message);
            }
            #endregion
        }

        public ActionResult BR(string id)
        {
            #region variables
            string request = null;
            Dictionary<string, object> pageRequest = null;
            Dictionary<string, object> response = null;

            #region lancio redirect parameters
            long EndUserID = -1;
            int CarrID = -1;
            int RetCode = -1;
            long TmpEndUserID = -1;
            #endregion

            #region 0100 redirect parameters
            int ServiceID = -1;
            string TransactionID = null;
            string UserIP = null;
            string TemplateID = "1234";
            #endregion

            #endregion
            #region try
            try
            {
                #region variables setup
                request = ServiceLayer.Utilities.RequestString(Request);
                var queryString = Request.QueryString;
                UserIP = Request.UserHostAddress;
                #endregion

                #region 0100 parameters
                if (string.IsNullOrEmpty(id))
                {
                    ServiceID = 1234;
                    TransactionID = DateTime.Now.Ticks.ToString();
                    TemplateID = "1234";
                }
                else
                {
                    string[] split = id.Split('_');
                    ServiceID = int.Parse(split[0]);
                    string ch = split[1].ToString();
                    string tid = split[2].ToString();

                    TransactionID = ch + "_" + tid;
                }
                #endregion

                #region preconditions
                if ((queryString == null) || (queryString.Count <= 0))
                    throw new Exception("Missing queryString parameters for the request: " + request);
                #endregion

                #region querystring parameters
                if ((queryString != null) && (queryString.Count > 0))
                {
                    #region lancio redirect parameters
                    if (!string.IsNullOrEmpty(queryString["EndUserID"]))
                        EndUserID = long.Parse(queryString["EndUserID"]);
                    if (!string.IsNullOrEmpty(queryString["CarrID"]))
                        CarrID = int.Parse(queryString["CarrID"]);
                    if (!string.IsNullOrEmpty(queryString["RetCode"]))
                        RetCode = int.Parse(queryString["RetCode"]);
                    if (!string.IsNullOrEmpty(queryString["TmpEndUserID"]))
                        TmpEndUserID = long.Parse(queryString["TmpEndUserID"]);
                    #endregion
                }
                #endregion

                #region check AJAX call
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    throw new Exception("Ajax call");
                #endregion

                #region excape
                if (((RetCode < 0) || (RetCode > 1100))
                    || ((CarrID != Carrier.Wind.GetHashCode()) && (CarrID != Carrier.H3G.GetHashCode())))
                {
                    // force behavior
                    ServiceElement _serviceElement = service.get(ServiceID, Carrier.H3G.GetHashCode());
                    return Redirect(_serviceElement.DigitalGOCatalogue);
                    //throw new Exception("Error: wrong RetCode - " + RetCode);
                }
                #endregion

                #region acquisition request
                ServiceElement serviceElement = service.get(ServiceID, CarrID);
                TemplateID = serviceElement.templateID;
                pageRequest = new Dictionary<string, object>
                {
                    {"ActivationURL",serviceElement.activationUrl},
                    {"FailURLPostfix","?id="+TransactionID},
                    {"ServiceID", ServiceID },
                    {"SuccessURLPostfix","?id="+TransactionID},
                    {"TemplateID",TemplateID},
                    {"TmpEndUserID",TmpEndUserID},
                    {"TransactionID",TransactionID },
                    {"UserAgent",UserAgent},
                    {"UserID",EndUserID },
                    {"UserIP",UserIP},
                };
                response = endUser.ActivationRequest(pageRequest);
                #endregion

                #region parse response
                if ((response == null) || (response.Count < 3))
                    throw new Exception("Invalid response for request: " + JsonConvert.SerializeObject(pageRequest, Formatting.None));
                if (response["RetCode"].ToString() == "RequestExecutedSuccessfullyRedirectUser")
                    return Redirect(response["RedirectUrl"].ToString());
                else
                {
                    string error = "Invalid response: " + JsonConvert.SerializeObject(response, Formatting.None);
                    error += " for request: " + JsonConvert.SerializeObject(pageRequest, Formatting.None);
                    throw new Exception(error);
                }
                #endregion
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                trace.traceError(error);
                ViewBag.message = error;
                return View("Error");
            }
            #endregion
            #region finally
            finally
            {
                //DateTime _responseTime = DateTime.Now;
                //LogEntry _logEntry = new LogEntry
                //{
                //    LogName = _LogName,
                //    internal_id = _internal_id,
                //    requestTime = _requestTime,
                //    responseTime = _responseTime,
                //    request = _request,
                //    response = _response,
                //    internal_parameters = JsonConvert.SerializeObject(_internal_parameters)
                //};
                //await log.trackAsync(_logEntry);

                JObject _jobject = new JObject();
                _jobject.Add(new JProperty("header", "BR"));
                _jobject.Add(new JProperty("request", request));
                if ((pageRequest != null) && (pageRequest.Count > 0))
                    _jobject.Add(new JProperty("pageRequest", JsonConvert.SerializeObject(pageRequest, Formatting.None)));
                if ((response!=null) && (response.Count>0))
                    _jobject.Add(new JProperty("response", JsonConvert.SerializeObject(response, Formatting.None)));
                string message = _jobject.ToString(Formatting.None);
                trace.trace(message);
            }
            #endregion
        }

        public ActionResult Disattivazione()
        {
            #region variables
            string request = null;
            string redirectUrl = null;
            #endregion
            #region try
            try
            {
                #region variables setup
                request = ServiceLayer.Utilities.RequestString(Request);
                var queryString = Request.QueryString;
                string Host = Request.Url.Host;
                ServiceElement element = service.getByDomain(Host);
                #endregion

                #region check AJAX call
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    throw new Exception("Ajax call");
                #endregion

                redirectUrl = endUser.GetMobileRecognitonUrl(element.ServiceID.ToString(), @"http://" + Host + DisattivazioneBaseReturnUrl);

                #region precondition
                if (string.IsNullOrEmpty(redirectUrl))
                    throw new Exception("Cannot redirect user");
                #endregion

                return Redirect(redirectUrl);
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                trace.traceError(error);
                ViewBag.message = error;
                return View("Error");
            }
            #endregion
            #region finally
            finally
            {
                //DateTime _responseTime = DateTime.Now;
                //LogEntry _logEntry = new LogEntry
                //{
                //    LogName = _LogName,
                //    internal_id = _internal_id,
                //    requestTime = _requestTime,
                //    responseTime = _responseTime,
                //    request = _request,
                //    response = _response,
                //    internal_parameters = JsonConvert.SerializeObject(_internal_parameters)
                //};
                //await log.trackAsync(_logEntry);

                JObject _jobject = new JObject();
                _jobject.Add(new JProperty("header", "Disattivazione"));
                _jobject.Add(new JProperty("request", request));
                _jobject.Add(new JProperty("redirectUrl", redirectUrl));
                string message = _jobject.ToString(Formatting.None);
                trace.trace(message);
            }
            #endregion
        }

        public ActionResult DBR()
        {
            #region variables
            string request = null;
            string redirectUrl = null;

            #region lancio redirect parameters
            long EndUserID = -1;
            int CarrID = -1;
            int RetCode = -1;
            string TmpEndUserID=null;
            #endregion

            #endregion
            #region try
            try
            {
                #region variables setup
                request = ServiceLayer.Utilities.RequestString(Request);
                var queryString = Request.QueryString;
                string Host = Request.Url.Host;
                #endregion

                #region preconditions
                if ((queryString == null) || (queryString.Count <= 0))
                    throw new Exception("Missing queryString parameters for the request: " + request);
                #endregion

                #region querystring parameters
                if ((queryString != null) && (queryString.Count > 0))
                {
                    #region lancio redirect parameters
                    if (!string.IsNullOrEmpty(queryString["EndUserID"]))
                        EndUserID = long.Parse(queryString["EndUserID"]);
                    if (!string.IsNullOrEmpty(queryString["CarrID"]))
                        CarrID = int.Parse(queryString["CarrID"]);
                    if (!string.IsNullOrEmpty(queryString["RetCode"]))
                        RetCode = int.Parse(queryString["RetCode"]);
                    if (!string.IsNullOrEmpty(queryString["TmpEndUserID"]))
                        TmpEndUserID = queryString["TmpEndUserID"];
                    #endregion
                }
                #endregion

                #region check AJAX call
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    throw new Exception("Ajax call");
                #endregion

                #region excape
                if (RetCode == 1000)
                {
                    redirectUrl = endUser.manageOTP(TmpEndUserID, @"http://" + Host + OTPBaseReturnUrl);
                    return Redirect(redirectUrl);
                }
                else if (RetCode == 1100)
                {
                    throw new NotImplementedException();
                }
                else
                    throw new NotImplementedException();
                #endregion
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                trace.traceError(error);
                ViewBag.message = error;
                return View("Error");
            }
            #endregion
            #region finally
            finally
            {
                //DateTime _responseTime = DateTime.Now;
                //LogEntry _logEntry = new LogEntry
                //{
                //    LogName = _LogName,
                //    internal_id = _internal_id,
                //    requestTime = _requestTime,
                //    responseTime = _responseTime,
                //    request = _request,
                //    response = _response,
                //    internal_parameters = JsonConvert.SerializeObject(_internal_parameters)
                //};
                //await log.trackAsync(_logEntry);

                JObject _jobject = new JObject();
                _jobject.Add(new JProperty("header", "DBR"));
                _jobject.Add(new JProperty("request", request));
                _jobject.Add(new JProperty("redirectUrl", redirectUrl));
                string message = _jobject.ToString(Formatting.None);
                trace.trace(message);
            }
            #endregion
        }

        public ActionResult OTPBR()
        {
            #region variables
            string request = null;
            Dictionary<string, object> CheckSubsRequest = null;
            Dictionary<string, object> response = null;

            #region lancio redirect parameters
            long EndUserID = -1;
            int CarrID = -1;
            int RetCode = -1;
            string TmpEndUserID = null;
            #endregion

            #endregion
            #region try
            try
            {
                #region variables setup
                request = ServiceLayer.Utilities.RequestString(Request);
                var queryString = Request.QueryString;
                string Host = Request.Url.Host;
                #endregion

                #region preconditions
                if ((queryString == null) || (queryString.Count <= 0))
                    throw new Exception("Missing queryString parameters for the request: " + request);
                #endregion

                #region querystring parameters
                if ((queryString != null) && (queryString.Count > 0))
                {
                    #region lancio redirect parameters
                    if (!string.IsNullOrEmpty(queryString["EndUserID"]))
                        EndUserID = long.Parse(queryString["EndUserID"]);
                    if (!string.IsNullOrEmpty(queryString["CarrID"]))
                        CarrID = int.Parse(queryString["CarrID"]);
                    if (!string.IsNullOrEmpty(queryString["RetCode"]))
                        RetCode = int.Parse(queryString["RetCode"]);
                    if (!string.IsNullOrEmpty(queryString["TmpEndUserID"]))
                        TmpEndUserID = queryString["TmpEndUserID"];
                    #endregion
                }
                #endregion

                #region check AJAX call
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    throw new Exception("Ajax call");
                #endregion

                #region excape
                if (RetCode == 1000)
                {
                    #region acquisition request
                    ServiceElement serviceElement = service.getByDomain(Host);
                    CheckSubsRequest = new Dictionary<string, object>
                    {
                        {"ServiceID", serviceElement.ServiceID },
                        {"UserID",EndUserID },
                    };
                    response = endUser.CheckSubs(CheckSubsRequest);
                    #endregion

                    #region parse response
                    if (response == null)
                        throw new Exception("Invalid response for request: " + JsonConvert.SerializeObject(CheckSubsRequest, Formatting.None));
                    if (response["RetCode"].ToString() != "1000")
                    {
                        // user already de-activated
                        throw new NotImplementedException();
                    }
                    else
                    {
                        string subscriberID = response["RetCode"].ToString();
                    }
                    #endregion
                }
                else if (RetCode == 1100)
                {
                    throw new NotImplementedException();
                }
                else
                    throw new NotImplementedException();
                #endregion

                throw new NotImplementedException();
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                trace.traceError(error);
                ViewBag.message = error;
                return View("Error");
            }
            #endregion
            #region finally
            finally
            {
                //DateTime _responseTime = DateTime.Now;
                //LogEntry _logEntry = new LogEntry
                //{
                //    LogName = _LogName,
                //    internal_id = _internal_id,
                //    requestTime = _requestTime,
                //    responseTime = _responseTime,
                //    request = _request,
                //    response = _response,
                //    internal_parameters = JsonConvert.SerializeObject(_internal_parameters)
                //};
                //await log.trackAsync(_logEntry);

                JObject _jobject = new JObject();
                _jobject.Add(new JProperty("header", "OTPBR"));
                _jobject.Add(new JProperty("request", request));
                if ((CheckSubsRequest != null) && (CheckSubsRequest.Count > 0))
                    _jobject.Add(new JProperty("CheckSubsRequest", JsonConvert.SerializeObject(CheckSubsRequest, Formatting.None)));
                if ((response != null) && (response.Count > 0))
                    _jobject.Add(new JProperty("response", JsonConvert.SerializeObject(response, Formatting.None)));
                string message = _jobject.ToString(Formatting.None);
                trace.trace(message);
            }
            #endregion
        }

        public ActionResult test()
        {
            return Redirect("http://landings.mobileservice.mobi/hostedpage/load/232/?env=1");
        }
    }
}