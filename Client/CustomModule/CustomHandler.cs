using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Client.CustomModule
{
    public class CustomHandler : IHttpHandler
    {

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var fi = new FileInfo(context.Server.MapPath(context.Request.FilePath));
            if (fi.Exists)
            {
                var srcImage = Image.FromFile(fi.FullName);
                using (var b = new Bitmap(srcImage))
                {
                    b.Save(context.Response.OutputStream, ImageFormat.Png);
                }
                context.Response.ContentType = "image/png";
                context.Response.StatusCode = 200;
                context.Response.StatusDescription = "OK";
                context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                context.Response.StatusCode = 404;
                context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}