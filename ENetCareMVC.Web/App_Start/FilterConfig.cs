using System.Web;
using System.Web.Mvc;

namespace ENetCareMVC.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            // By default, all actions require a logged in user
            filters.Add(new AuthorizeAttribute());

        }
    }
}
