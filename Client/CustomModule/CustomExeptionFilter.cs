using System.Web.Mvc;

namespace Client.CustomModule
{
    public class CustomExeptionFilter : FilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
            filterContext.ExceptionHandled = true;
        }
        
    }
}