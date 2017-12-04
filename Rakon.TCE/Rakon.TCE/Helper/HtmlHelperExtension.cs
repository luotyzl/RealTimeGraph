using System.Web;
using System.Web.Mvc;

namespace Rakon.TCE.Helper
{
    public static class HtmlHelperExtension
    {
        // Returns the website base URL, e.g. http://tt920.trunet.local/staging/ or http://localhost:52059/
        public static string GetBaseUrl(this HtmlHelper helper)
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/") appUrl += "/";

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }
    }
}
