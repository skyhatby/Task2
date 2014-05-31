using System.Web.Mvc;
using Client.CustomModule;

namespace Client
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomExeptionFilter());
        }
    }
}
