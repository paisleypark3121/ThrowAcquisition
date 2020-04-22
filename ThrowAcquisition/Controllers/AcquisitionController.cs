using Call;
using Diagnostic;
using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LancioIntegration.Controllers
{
    public class AcquisitionController : Controller
    {
        private ICall call;
        private ILog log;
        private ITrace trace;

        public AcquisitionController(
            ICall _call,
            ILog _log,
            ITrace _trace)
        {
            call = _call;
            log = _log;
            trace = _trace;
        }

        // GET: Acquisition
        public ActionResult Index()
        {
            return View();
        }
    }
}