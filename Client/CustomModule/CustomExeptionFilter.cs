using System.Web.Mvc;

namespace Client.CustomModule
{
    public class CustomExeptionFilter : FilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.RedirectToRoute("~/");
        }
        
    }
}