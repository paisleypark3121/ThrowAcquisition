using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ThrowAcquisition.ServiceLayer.Catalogue
{
    public class CatalogueLayout : ICatalogueLayout
    {
        private List<CatalogueLayoutElement> CatalogueLayouts = null;

        public CatalogueLayout()
        {
            if (CatalogueLayouts == null)
            {
                CatalogueLayouts = new List<CatalogueLayoutElement>()
                {
                    new CatalogueLayoutElement() {
                        service="GLAMSPOT",
                        style="../../Content/glamspot/glamspot.css",
                        logo="../Content/glamspot/logo-it.png",
                        btn_login="../Content/glamspot/btn-login-it.png",
                        login_box="background-image: url(../Content/glamspot/bg-box.png)",
                        page_wrapper="background-image: url(../Content/glamspot/bg-wrapper.png)",
                        container="",
                        body=""
                    },
                    new CatalogueLayoutElement() {
                        service="CLUB4FUN",
                        style="../../Content/club4fun/club4fun.css",
                        logo="../Content/club4fun/logo.png",
                        //btn_login="background-image: url(../Content/club4fun/entra-btn.png)",
                        btn_login="../Content/club4fun/entra-btn.png",
                        login_box="",
                        page_wrapper="../Content/club4fun/bg.jpg",
                        container="",
                        body=""
                    },
                    new CatalogueLayoutElement() {
                        service="ASTROTODAY",
                        style="../../Content/astrotoday/astroToday.css",
                        logo="../Content/astrotoday/logo-it.png",
                        btn_login="../Content/astrotoday/btn-login-it.png",
                        body="background-image: url(../Content/astrotoday/bg-body.jpg)",
                        page_wrapper="../Content/astrotoday/bg-wrapper.png",
                        container=""
                    },

                    new CatalogueLayoutElement() {
                        service="ILIKEMETEO",
                        style="../../Content/ilikemeteo/iLikeMeteo.css",
                        logo="../Content/ilikemeteo/logo-it.png",
                        btn_login="../Content/ilikemeteo/btn-login-it.png",
                        body="background-image: url(../Content/ilikemeteo/bg-body.png)",
                        page_wrapper="",
                        container="background-image: url(../Content/ilikemeteo/bg-container.png)"
                    }
                };
            }
        }


        public CatalogueLayoutElement get(string service)
        {
            #region try
            try
            {
                #region preconditions
                if (CatalogueLayouts == null)
                    throw new Exception("Missing layouts");
                if (string.IsNullOrEmpty(service))
                    throw new Exception("Missing mandatory parameter service");
                #endregion

                return CatalogueLayouts.Where(x => ((x.service == service))).FirstOrDefault();
            }
            #endregion
            #region catch
            catch (Exception ex)
            {
                string error = "Error in function " + MethodBase.GetCurrentMethod().Name + " - " + ex.Message;
                System.Diagnostics.Trace.TraceError(error);
                return null;
            }
            #endregion
        }

    }
}