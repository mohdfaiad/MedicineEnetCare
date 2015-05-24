using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ENetCareMVC.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = HttpContext.Current.Server.GetLastError();

            //Handler HTTP errors
            if (ex.GetType() == typeof(HttpException))
            {
                var httpCode = ((HttpException)ex).GetHttpCode();

                // Do not log http 404 & 403 exception and let customErrors section handle it
                if (httpCode == 404 || httpCode == 403)
                    return;
            }

            // Get inner exception if there is one
            if (ex is HttpUnhandledException && ex.InnerException != null)
                ex = ex.InnerException;

            var loggedInUser = "NONE";
            if (User.Identity.IsAuthenticated)
                loggedInUser = User.Identity.Name;

            StringBuilder errorMsg = new StringBuilder();
            errorMsg.AppendFormat("{0}: Unhandled exception occurred: {1}, URL: {2}, Username: {3}",
                DateTime.Now,
                ex.Message,
                Request.RawUrl, loggedInUser);
            errorMsg.AppendLine();
            errorMsg.AppendFormat("Type: {0}", ex.GetType());
            errorMsg.AppendLine();
            errorMsg.AppendFormat("Source: {0}", ex.Source);
            errorMsg.AppendLine();
            errorMsg.AppendFormat("Stack Trace: {0}", ex.StackTrace);
            errorMsg.AppendLine();

            string errorLogPath = Server.MapPath("~/Log/error.log");

            File.AppendAllText(errorLogPath, errorMsg.ToString());
        }
    }
}
