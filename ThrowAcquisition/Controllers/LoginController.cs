using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThrowAcquisition.ServiceLayer.Catalogue;
using ThrowAPI;

namespace ThrowAcquisition.Controllers
{
    public class LoginController : Controller
    {
        //private IOTP otp;
        //private ICatalogueLayout catalogueLayout;

        public LoginController(
            //IOTP _otp,
            //ICatalogueLayout _catalogueLayout
            )
        {
            //otp = _otp;
            //catalogueLayout = _catalogueLayout;
        }

        // GET: Login
        public ActionResult Index()
        {
            //string host = Request.Url.Host;

            //CatalogueLayoutElement catalogueLayoutElement = catalogueLayout.get(host.ToUpper());
            //if (catalogueLayoutElement==null)
            //    catalogueLayoutElement = catalogueLayout.get("CLUB4FUN");

            //ViewBag.style = catalogueLayoutElement.style;
            //ViewBag.title = catalogueLayoutElement.service;
            //ViewBag.logo = catalogueLayoutElement.logo;
            //ViewBag.page_wrapper = catalogueLayoutElement.page_wrapper;
            //ViewBag.login_box = catalogueLayoutElement.login_box;
            //ViewBag.Btn_Login = catalogueLayoutElement.btn_login;
            //ViewBag.body = catalogueLayoutElement.body;
            //ViewBag.page_container = catalogueLayoutElement.container;

            //#region FIRST ACCESS
            //if (Request.HttpMethod == "GET")
            //{
            //    ViewBag.phase = 1;
            //    ViewBag.welcome = "Per accedere inserisci il tuo numero di cellulare.";
            //    ViewBag.label = "CELLULARE";
            //    ViewBag.req = "Inserisci il tuo cellulare";
            //}
            //#endregion
            //#region SEND PINCODE
            //else if ((Request.HttpMethod == "POST") && Request["phase"]=="1")
            //{
            //    string msisdn = Request["Txt_First"];


            //    int pin=otp.sendOTP(msisdn);
            //    if (pin < 0)
            //        throw new Exception("Error in sending pin code");
 
            //    ViewBag.phase = 2;
            //    ViewBag.welcome = "Per accedere inserisci il pin code ricevuto";
            //    ViewBag.label = "PIN CODE";
            //    ViewBag.req = "Inserisci il pin che hai ricevuto";
            //    ViewBag.msisdn = msisdn;
            //}
            //#endregion
            //#region VALIDATE PIN CODE
            //else if ((Request.HttpMethod == "POST") && Request["phase"] == "2")
            //{
            //    string msisdn = Request["msisdn"];
            //    string pin = Request["Txt_First"];

            //    if (otp.validateOTP(msisdn,pin))
            //    {
            //        // redirect to catalogue
            //    }
            //    else
            //    {
            //        ViewBag.phase = 1;
            //        ViewBag.welcome = "Per accedere inserisci il tuo numero di cellulare.";
            //        ViewBag.label = "CELLULARE";
            //        ViewBag.req = "Inserisci il tuo cellulare";
            //    }
            //}
            //#endregion

            return View();
        }
    }
}