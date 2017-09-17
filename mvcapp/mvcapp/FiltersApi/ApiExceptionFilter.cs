using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using mvcapp.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog;

namespace mvcapp.FiltersApi
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            // throw new Exception("Throw exception to call ExceptionHandler");
            var exceptionLogger = LogManager.GetLogger("ExceptionLogger");

            var generalErrorDto = new GeneralError();

            if (generalErrorDto.IsDebug)
            {
                generalErrorDto.Exception = context.Exception;
            }

            var jsonMediaTypeFormatter = GetJsonMediaTypeFormatter();

            context.Response = context.Request.
                CreateResponse(HttpStatusCode.BadRequest, generalErrorDto, jsonMediaTypeFormatter);

            exceptionLogger.Error(context.Exception);
        }

        private static JsonMediaTypeFormatter GetJsonMediaTypeFormatter()
        {
            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                }
            };

            jsonMediaTypeFormatter.SerializerSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
            return jsonMediaTypeFormatter;
        }
    }
}