using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThrowAcquisition.ServiceLayer.Catalogue
{
    public interface ICatalogueLayout
    {
        CatalogueLayoutElement get(string service);
    }
}