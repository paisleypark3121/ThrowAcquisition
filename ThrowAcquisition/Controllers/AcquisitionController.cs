using Call;
using Diagnostic;
using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ThrowAPI;

namespace ThrowAcquisition.Controllers
{
    public class AcquisitionController : Controller
    {
        //const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0. 2272.118 Safari/537.36.";
        //const string ActivationURL = "";
        //const string BaseReturnUrl = @"http://throwacquisition.azurewebsites.net/acquisition/basereturn";
        //const string BaseCatalogueReturnUrl = "/acquisition/basecataloguereturn";

        //private IDiagnostic trace;
        //private IMobileUserRecognition mobileUserRecognition;
        //private IActivation activation;
        //private IService service;

        //public AcquisitionController(
        //    IDiagnostic _trace,
        //    IMobileUserRecognition _mobileUserRecognition,
        //    IActivation _activation,
        //    IService _service
        //    )
        //{
        //    trace = _trace;
        //    mobileUserRecognition = _mobileUserRecognition;
        //    activation = _activation;
        //    service = _service;
        //}

        //// GET: Acquisition
        //public ActionResult Index()
        //{   
        //    #region variables
        //    string request = null;

        //    #endregion
        //    #region try
        //    try
        //    {
        //        #region variables setup
        //        request = ServiceLayer.Utilities.RequestString(Request);
        //        var queryString = Request.QueryString;
        //        string url = Request.Url.Host;
        //        ServiceElement element = service.getByDomain(url);
        //        #endregion

        //        #region check AJAX call
        //        if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            throw new Exception("Ajax call");
        //        #endregion

        //        #region MobileUserRecognition
        //        string tid = DateTime.Now.Ticks.ToString();
        //        string id = element.ServiceID + "_test_" + tid;

        //        string redirectUrl = mobileUserRecognition.GetMobileRecognitonUrl(element.ServiceID.ToString(), @"http://"+url+BaseCatalogueReturnUrl + @"/" + id);
        //        #endregion

        //        #region precondition
        //        if (string.IsNullOrEmpty(redirectUrl))
        //            throw new Exception("Cannot redirect user");
        //        #endregion

        //        return Redirect(redirectUrl);
        //    }
        //    #endregion
        //    #region catch
        //    catch (Exception ex)
        //    {
        //        string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
        //        trace.traceError(error);
        //        ViewBag.message = error;
        //        return View("Error");
        //    }
        //    #endregion
        //    #region finally
        //    finally
        //    {
        //        //DateTime _responseTime = DateTime.Now;
        //        //LogEntry _logEntry = new LogEntry
        //        //{
        //        //    LogName = _LogName,
        //        //    internal_id = _internal_id,
        //        //    requestTime = _requestTime,
        //        //    responseTime = _responseTime,
        //        //    request = _request,
        //        //    response = _response,
        //        //    internal_parameters = JsonConvert.SerializeObject(_internal_parameters)
        //        //};
        //        //await log.trackAsync(_logEntry);

        //        //diagnostic = "internal_id: " + _internal_id + " - responseTime: " + _responseTime.ToString("HH:mm:ss.fff") + " - logEntry: " + QueenFramework.Utilities.Utilities.printObject(_logEntry);
        //        //trace.trace(diagnostic);
        //    }
        //    #endregion
        //}

        ///// <summary>
        ///// p.70
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult BaseReturn(string id)
        //{
        //    #region variables
        //    string request = null;

        //    #region lancio redirect parameters
        //    long EndUserID = -1;
        //    int CarrID = -1;
        //    int RetCode = -1;
        //    long TmpEndUserID = -1;
        //    #endregion

        //    #region 0100 redirect parameters
        //    int ServiceID = -1;
        //    string TransactionID = null;
        //    string UserIP = null;
        //    string TemplateID = "1234";
        //    #endregion

        //    #endregion
        //    #region try
        //    try
        //    {
        //        #region variables setup
        //        request = ServiceLayer.Utilities.RequestString(Request);
        //        var queryString = Request.QueryString;
        //        UserIP = Request.UserHostAddress;
        //        #endregion

        //        #region 0100 parameters
        //        if (string.IsNullOrEmpty(id))
        //        {
        //            ServiceID = 1234;
        //            TransactionID = DateTime.Now.Ticks.ToString();
        //            TemplateID = "1234";
        //        }
        //        else
        //        {
        //            string[] split = id.Split('_');
        //            ServiceID = int.Parse(split[0]);
        //            string ch = split[1].ToString();
        //            string tid = split[2].ToString();

        //            TransactionID = ch + "_" + tid;
        //        }
        //        #endregion

        //        #region preconditions
        //        if ((queryString == null) || (queryString.Count <= 0))
        //            throw new Exception("Missing queryString parameters for the request: " + request);
        //        #endregion

        //        #region querystring parameters
        //        if ((queryString != null) && (queryString.Count > 0))
        //        {
        //            #region lancio redirect parameters
        //            if (!string.IsNullOrEmpty(queryString["EndUserID"]))
        //                EndUserID = long.Parse(queryString["EndUserID"]);
        //            if (!string.IsNullOrEmpty(queryString["CarrID"]))
        //                CarrID = int.Parse(queryString["CarrID"]);
        //            if (!string.IsNullOrEmpty(queryString["RetCode"]))
        //                RetCode = int.Parse(queryString["RetCode"]);
        //            if (!string.IsNullOrEmpty(queryString["TmpEndUserID"]))
        //                TmpEndUserID = long.Parse(queryString["TmpEndUserID"]);
        //            #endregion
        //        }
        //        #endregion

        //        #region check AJAX call
        //        if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            throw new Exception("Ajax call");
        //        #endregion

        //        #region excape
        //        if ((RetCode < 0) || (RetCode > 1100))
        //            throw new Exception("Error: wrong RetCode - "+RetCode);
        //        #endregion

        //        #region acquisition request
        //        ServiceElement serviceElement = service.get(ServiceID, CarrID);
        //        TemplateID = serviceElement.templateID;
        //        Dictionary<string, object> pageRequest = new Dictionary<string, object>
        //        {
        //            {"ServiceID", ServiceID },
        //            {"TransactionID",TransactionID },
        //            {"UserID",EndUserID },
        //            {"TmpEndUserID",TmpEndUserID},
        //            {"UserAgent",UserAgent},
        //            {"ActivationURL",ActivationURL},
        //            {"UserIP",UserIP},
        //            {"SuccessURLPostfix","id="+TransactionID},
        //            {"FailURLPostfix","id="+TransactionID},
        //            {"TemplateID",TemplateID},
        //        };
        //        Dictionary<string, object> response = activation.PageRequest(pageRequest);
        //        #endregion

        //        #region parse response
        //        if ((response == null) || (response.Count < 3))
        //            throw new Exception("Invalid response");
        //        if (response["RetCode"].ToString() == "1010")
        //            return Redirect(response["RedirectUrl"].ToString());
        //        else
        //            throw new Exception("Invalid returnUrl");
        //        #endregion
        //    }
        //    #endregion
        //    #region catch
        //    catch (Exception ex)
        //    {
        //        string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
        //        trace.traceError(error);
        //        ViewBag.message = error;
        //        return View("Error");
        //    }
        //    #endregion
        //    #region finally
        //    finally
        //    {
        //        //DateTime _responseTime = DateTime.Now;
        //        //LogEntry _logEntry = new LogEntry
        //        //{
        //        //    LogName = _LogName,
        //        //    internal_id = _internal_id,
        //        //    requestTime = _requestTime,
        //        //    responseTime = _responseTime,
        //        //    request = _request,
        //        //    response = _response,
        //        //    internal_parameters = JsonConvert.SerializeObject(_internal_parameters)
        //        //};
        //        //await log.trackAsync(_logEntry);

        //        //diagnostic = "internal_id: " + _internal_id + " - responseTime: " + _responseTime.ToString("HH:mm:ss.fff") + " - logEntry: " + QueenFramework.Utilities.Utilities.printObject(_logEntry);
        //        //trace.trace(diagnostic);
        //    }
        //    #endregion
        //}

        ///// <summary>
        ///// p.14
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult EntryPoint()
        //{
        //    #region variables
        //    string request = null;
        //    int sid = -1;
        //    string ch = null;
        //    string tid = null;
        //    string id = null;
        //    #endregion
        //    #region try
        //    try
        //    {
        //        #region variables setup
        //        request = ServiceLayer.Utilities.RequestString(Request);
        //        var queryString = Request.QueryString;
        //        #endregion

        //        #region querystring parameters
        //        if ((queryString != null) && (queryString.Count > 0))
        //        {
        //            if (!string.IsNullOrEmpty(queryString["sid"]))
        //                sid = int.Parse(queryString["sid"]);
        //            if (!string.IsNullOrEmpty(queryString["ch"]))
        //                ch = queryString["ch"];
        //            if (!string.IsNullOrEmpty(queryString["tid"]))
        //                tid = queryString["tid"];
        //        }
        //        if (sid == -1)
        //            sid = 1234;
        //        if (string.IsNullOrEmpty(ch))
        //            ch = "test";
        //        if (string.IsNullOrEmpty(tid))
        //            tid = DateTime.Now.Ticks.ToString();
        //        id = sid + "_" + ch + "_" + tid;
        //        #endregion

        //        #region check AJAX call
        //        if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //            throw new Exception("Ajax call");
        //        #endregion

        //        #region MobileUserRecognition
        //        string redirectUrl = mobileUserRecognition.GetMobileRecognitonUrl(sid.ToString(), BaseReturnUrl + @"/" + id);
        //        #endregion

        //        #region precondition
        //        if (string.IsNullOrEmpty(redirectUrl))
        //            throw new Exception("Cannot redirect user");
        //        #endregion

        //        return Redirect(redirectUrl);
        //    }
        //    #endregion
        //    #region catch
        //    catch (Exception ex)
        //    {
        //        string error = "Error in function " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
        //        trace.traceError(error);
        //        ViewBag.message = error;
        //        return View("Error");
        //    }
        //    #endregion
        //    #region finally
        //    finally
        //    {
        //        //DateTime _responseTime = DateTime.Now;
        //        //LogEntry _logEntry = new LogEntry
        //        //{
        //        //    LogName = _LogName,
        //        //    internal_id = _internal_id,
        //        //    requestTime = _requestTime,
        //        //    responseTime = _responseTime,
        //        //    request = _request,
        //        //    response = _response,
        //        //    internal_parameters = JsonConvert.SerializeObject(_internal_parameters)
        //        //};
        //        //await log.trackAsync(_logEntry);

        //        //diagnostic = "internal_id: " + _internal_id + " - responseTime: " + _responseTime.ToString("HH:mm:ss.fff") + " - logEntry: " + QueenFramework.Utilities.Utilities.printObject(_logEntry);
        //        //trace.trace(diagnostic);
        //    }
        //    #endregion
        //}
    }
}