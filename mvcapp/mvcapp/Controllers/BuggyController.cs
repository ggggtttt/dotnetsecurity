using System;
using System.Web.Http;

namespace mvcapp.Controllers
{
    public class BuggyController : ApiController
    {
        [HttpGet]
        [Route("api/buggy")]
        public string GetBuggy( )
        {
            throw new Exception("Intentionally throw exception.");
        }
    }
}