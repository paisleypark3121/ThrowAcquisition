using Diagnostic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public ActionResult CheckAge()
        {
            return View();
        }

        public ActionResult Index()
        {
            #region variables
            string request = null;
            string redirectUrl = null;
            bool test = false;
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
                    _host += RawUrl;
                string fullBaseReturnUrl = @"http://" + Host + BaseReturnUrl + @"/";
                #endregion

                #region ALWAYS ON
                if ((!string.IsNullOrEmpty(Request.UserAgent)) &&
                    (Request.UserAgent.ToLower().Contains("alwayson")))
                {
                    redirectUrl = "Always On: " + Request.UserAgent;
                    return View();
                }
                #endregion
                #region GOOGLE BOT
                if (Request.UserHostAddress.StartsWith("66."))
                {
                    redirectUrl = "Google BOT: "+ Request.UserHostAddress;
                    return View();
                }
                #endregion

                #region check AJAX call
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    throw new Exception("Ajax call");
                #endregion

                #region ServiceElement
                ServiceElement element = Utilities.GetServiceByDomain(service, Host, RawUrl);
                #endregion

                ////////////////////////////// FORCE TEST //////////////////////////////
                if ((queryString != null) && (queryString["sid"] != null))
                {
                    element = service.get(int.Parse(queryString["sid"].ToString()));
                    test = true;
                }
                ////////////////////////////// FORCE TEST //////////////////////////////
                
                ////////////////////////////// FORCE //////////////////////////////
                //if (!test)
                //    return Redirect(element.DigitalGOCatalogue);
                ////////////////////////////// FORCE //////////////////////////////

                #region murp
                string tid = DateTime.Now.Ticks.ToString();
                string id = element.ServiceID + "_index_" + tid;
                return Redirect(redirectUrl = endUser.GetMobileRecognitonUrl(element.ServiceID.ToString(), fullBaseReturnUrl + id));
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
            string redirectUrl = null;
            #endregion

            #endregion
            #region try
            try
            {
                #region variables setup
                request = ServiceLayer.Utilities.RequestString(Request);
                var queryString = Request.QueryString;
                string Host = Request.Url.Host;
                string RawUrl = Request.RawUrl;
                RawUrl = RawUrl.Substring(0, RawUrl.IndexOf("/BR"));
                string _host = Host;
                if (RawUrl != "/")
                    _host += RawUrl;
                UserIP = Request.UserHostAddress;
                #endregion

                #region 0100 parameters
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Cannot retrieve navigation information");
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

                #region ServiceElement
                ServiceElement element = Utilities.GetServiceByDomain(service, Host, RawUrl);
                #endregion

                #region check CarrID for DigitalGo redirect
                if (CarrID > 0)
                {
                    element = service.getByDomain(element.domainName, CarrID);
                    ServiceID = element.ServiceID;

                    if (!element.migrated)
                        return Redirect(element.DigitalGOCatalogue);

                    #region valid user
                    if ((RetCode == 1000) || (RetCode == 1100))
                    {
                        if (element.moderation)
                            return Redirect(redirectUrl = "../CheckAge/" + id + "?" + queryString);

                        #region acquisition request
                        TemplateID = element.templateID;
                        pageRequest = new Dictionary<string, object>
                        {
                            {"ActivationURL",element.activationUrl},
                            {"FailURLPostfix","?id="+TransactionID},
                            {"ServiceID", ServiceID },
                            {"SuccessURLPostfix","?tid="+TransactionID+"&CarrID="+CarrID},
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
                            return Redirect(redirectUrl = response["RedirectUrl"].ToString());
                        else
                        {
                            string error = "Invalid response: " + JsonConvert.SerializeObject(response, Formatting.None);
                            error += " for request: " + JsonConvert.SerializeObject(pageRequest, Formatting.None);
                            throw new Exception(error);
                        }
                        #endregion
                    }
                    #endregion
                    #region invalid user
                    else
                    {
                        //ViewBag.message = "Error " + RetCode;
                        //return View("Error");
                        return View("wifi");
                    }
                    #endregion
                }
                #endregion
                #region missing CarrID
                else
                {
                    //ViewBag.message = "Error " + RetCode;
                    //return View("Error");
                    return View("wifi");
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
                _jobject.Add(new JProperty("redirectUrl", redirectUrl));
                string message = _jobject.ToString(Formatting.None);
                trace.trace(message);
            }
            #endregion
        }

        public ActionResult yes(string id)
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
            string redirectUrl = null;
            #endregion

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
                    _host += RawUrl;
                UserIP = Request.UserHostAddress;
                #endregion

                #region 0100 parameters
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Cannot retrieve navigation information");
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

                #region 4g navigation
                if ((RetCode == 1000) || (RetCode == 1100))
                {
                    ServiceElement serviceElement = service.get(ServiceID, CarrID);

                    if (!serviceElement.migrated)
                        return Redirect(serviceElement.DigitalGOCatalogue);

                    #region acquisition request
                    TemplateID = serviceElement.templateID;
                    pageRequest = new Dictionary<string, object>
                    {
                        {"ActivationURL",serviceElement.activationUrl},
                        {"FailURLPostfix","?id="+TransactionID},
                        {"ServiceID", ServiceID },
                        {"SuccessURLPostfix","?tid="+TransactionID+"&CarrID="+CarrID},
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
                        return Redirect(redirectUrl = response["RedirectUrl"].ToString());
                    else
                    {
                        string error = "Invalid response: " + JsonConvert.SerializeObject(response, Formatting.None);
                        error += " for request: " + JsonConvert.SerializeObject(pageRequest, Formatting.None);
                        throw new Exception(error);
                    }
                    #endregion
                }
                #endregion
                #region ELSE
                else //if (RetCode == 4070)
                {
                    //ServiceElement serviceElement = service.get(ServiceID, 4);

                    //if (!serviceElement.migrated)
                    //    return Redirect(serviceElement.DigitalGOCatalogue);

                    //string url = null;
                    //if (Host.StartsWith("new"))
                    //    url= serviceElement.catalogue;
                    //else
                    //    url=serviceElement.DigitalGOCatalogue;

                    //return Redirect(url);

                    ViewBag.message = "Error " + RetCode;
                    return View("Error");
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
                if ((response != null) && (response.Count > 0))
                    _jobject.Add(new JProperty("response", JsonConvert.SerializeObject(response, Formatting.None)));
                _jobject.Add(new JProperty("redirectUrl", redirectUrl));
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
                string RawUrl = Request.RawUrl;
                RawUrl = RawUrl.Replace("/disattivazione", "");
                ViewBag.Host = Host;
                ViewBag.RawUrl = RawUrl;
                string _host = Host;
                if (RawUrl != "/")
                    _host += RawUrl;
                string fullBaseReturnUrl = @"http://" + Host + DisattivazioneBaseReturnUrl;
                #endregion

                #region check AJAX call
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    throw new Exception("Ajax call");
                #endregion

                #region ServiceElement
                ServiceElement element = Utilities.GetServiceByDomain(service, Host, RawUrl);
                #endregion

                redirectUrl = endUser.GetMobileRecognitonUrl(element.ServiceID.ToString(), fullBaseReturnUrl);

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

                //#region excape
                //if (RetCode == 1000)
                //{
                    redirectUrl = endUser.manageOTP(TmpEndUserID, @"http://" + Host + OTPBaseReturnUrl);
                    return Redirect(redirectUrl);
                //}
                //else if (RetCode == 1100)
                //{
                //    throw new NotImplementedException();
                //}
                //else
                //    throw new NotImplementedException();
                //#endregion
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
            Dictionary<string, object> DeactivationRequest = null;
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
                string RawUrl = Request.RawUrl;
                RawUrl = RawUrl.Replace("/OTPBR", "");
                if (RawUrl.Contains("?"))
                    RawUrl = RawUrl.Substring(0, RawUrl.IndexOf("?"));
                ViewBag.Host = Host;
                ViewBag.RawUrl = RawUrl;
                string _host = Host;
                if (RawUrl != "/")
                    _host += RawUrl;
                #endregion

                #region check AJAX call
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    throw new Exception("Ajax call");
                #endregion

                #region ServiceElement
                ServiceElement element = Utilities.GetServiceByDomain(service, Host, RawUrl);
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

                #region Request Successfull
                if ((RetCode == 1000) || (RetCode == 1100))
                {
                    #region checksub request
                    ServiceElement serviceElement = service.get(element.ServiceID, CarrID);
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

                    string SubscriberID = null;
                    foreach (var _element in response)
                    {
                        string key = _element.Key;
                        object value = _element.Value;
                        Dictionary<string, object> _value = (Dictionary<string, object>)value;
                        if ((RetCode == 1000) || (RetCode == 1030) || (RetCode == 1050))
                        {
                            SubscriberID = key;
                            break;
                        }
                    }
                    #endregion

                    #region user subscribed
                    if (!string.IsNullOrEmpty(SubscriberID))
                    {
                        DeactivationRequest = new Dictionary<string, object>
                        {
                            {"DeactivationMethodID", 1 },
                            {"SubscriberID",SubscriberID },
                        };
                        response = endUser.DeactivateRequest(DeactivationRequest);

                        if (response == null)
                            throw new Exception("Invalid response for request: " + JsonConvert.SerializeObject(DeactivationRequest, Formatting.None));
                        RetCode = int.Parse(response["RetCode"].ToString());

                        #region successfull request
                        if ((RetCode == 1001) || (RetCode == 1020))
                        {
                            ViewBag.message = "Disattivazione avvenuta correttamente";
                            return View("Information");
                        }
                        #endregion
                        #region ERROR
                        else
                        {
                            throw new Exception("Error in performing the request - DeactivationRequest: " + RetCode);
                        }
                        #endregion
                    }
                    #endregion
                    #region user already unsubscribed
                    else if (RetCode == 1010)
                    {
                        ViewBag.message = "Disattivazione avvenuta correttamente";
                        return View("Information");
                    }
                    #endregion
                    #region ERROR
                    else
                    {
                        throw new Exception("Error in performing the request - CheckSubs: " + RetCode);
                    }
                    #endregion
                }
                #endregion
                #region ERROR
                else
                {
                    throw new Exception("Error in performing the request: "+RetCode);
                }
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
                if ((DeactivationRequest != null) && (DeactivationRequest.Count > 0))
                    _jobject.Add(new JProperty("DeactivationRequest", JsonConvert.SerializeObject(DeactivationRequest, Formatting.None)));
                if ((response != null) && (response.Count > 0))
                    _jobject.Add(new JProperty("response", JsonConvert.SerializeObject(response, Formatting.None)));
                string message = _jobject.ToString(Formatting.None);
                trace.trace(message);
            }
            #endregion
        }

        public ActionResult BR_error()
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
            string ServiceID = null;
            string TransactionID = null;
            string redirectUrl = null;
            #endregion

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
                    _host += RawUrl;
                #endregion

                #region preconditions
                if ((queryString == null) || (queryString.Count <= 0))
                    throw new Exception("Missing queryString parameters for the request: " + request);
                #endregion

                #region querystring parameters
                if ((queryString != null) && (queryString.Count > 0))
                {
                    #region lancio redirect parameters
                    if (!string.IsNullOrEmpty(queryString["ServiceID"]))
                        ServiceID = queryString["ServiceID"];
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

                ServiceElement serviceElement = service.get(int.Parse(ServiceID), CarrID);
                if (RetCode == 4030)
                    return Redirect(redirectUrl = serviceElement.catalogue + "?EndUserID=" + EndUserID + "&CarrID=" + CarrID);
                else
                    return Redirect(redirectUrl = serviceElement.errorPage);
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
                if ((response != null) && (response.Count > 0))
                    _jobject.Add(new JProperty("response", JsonConvert.SerializeObject(response, Formatting.None)));
                _jobject.Add(new JProperty("redirectUrl", redirectUrl));
                string message = _jobject.ToString(Formatting.None);
                trace.trace(message);
            }
            #endregion
        }

        public ActionResult test()
        {
            return View("wifi");
        }
    }
}