using System.Web;
using System.Web.Mvc;

namespace SarahTizzard_WEBD3000_MyTunes_MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
