using System.Web.Http;
using System.Web.Mvc;
using mvcapp.Models;

namespace mvcapp.Controllers
{
    public class XssTestController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/xsstest/get")]
        public JsonResult Get(string value)
        {
            return new JsonResult {Data = value};
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/xsstest/post")]
        public JsonResult Post(XssTestModel model)
        {
            return new JsonResult { Data = model.Value };
        }
    }
}