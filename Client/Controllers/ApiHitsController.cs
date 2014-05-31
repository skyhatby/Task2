using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web.Http;

namespace Client.Controllers
{
    public class ApiHitsController : ApiController
    {
        public ConcurrentDictionary<string, int> Get()
        {
            var c = new CustomModule.CustModule().AllHits;
            return c;
        }
    }
}
