using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThrowAcquisition.ServiceLayer
{
    public class Utilities
    {   
        public static string RequestString(HttpRequestBase Request)
        {
            string requestString = "Url: " + Request.Url;
            requestString += " - UserHostAddress: " + Request.UserHostAddress;
            requestString += " - UserAgent: " + Request.UserAgent;
            if (Request.UrlReferrer != null) requestString += " - UrlReferrer: " + Request.UrlReferrer;
            if (Request.QueryString != null) requestString += " - QueryString: " + Request.QueryString;
            return requestString;
        }

        public static string extractValue(string _response, string placeholder_start, string placeholder_finish = null)
        {
            #region preconditions
            if (string.IsNullOrEmpty(_response))
                return null;
            if (string.IsNullOrEmpty(placeholder_start))
                return null;
            if (!_response.Contains(placeholder_start))
                return null;
            #endregion

            int index = _response.IndexOf(placeholder_start);
            string value = _response.Substring(index + placeholder_start.Length);

            if (placeholder_finish == null)
                return value.Trim();

            if (!value.Contains(placeholder_finish))
                return null;
            index = value.IndexOf(placeholder_finish);
            value = value.Substring(0, index);

            return value;
        }

        public static Dictionary<string, object> mergeDictionaries(Dictionary<string, object> d1, Dictionary<string, object> d2)
        {
            #region preconditions
            if ((d1 == null) || (d1.Count <= 0))
                return d2;
            if ((d2 == null) || (d2.Count <= 0))
                return d1;
            #endregion

            return d1.Concat(d2).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
        }

        public static Dictionary<string, string> mergeDictionaries(Dictionary<string, string> d1, Dictionary<string, string> d2)
        {
            #region preconditions
            if ((d1 == null) || (d1.Count <= 0))
                return d2;
            if ((d2 == null) || (d2.Count <= 0))
                return d1;
            #endregion

            return d1.Concat(d2).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
        }
    }
}