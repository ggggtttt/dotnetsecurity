using System.Text;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using mvcapp.Filters;
using Newtonsoft.Json;
using NLog;

namespace mvcapp.Handlers
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            base.Handle(context);

            var exceptionLogger = LogManager.GetLogger("ExceptionLogger");

            var generalErrorDto = new GeneralError();

            if (generalErrorDto.IsDebug)
            {
                generalErrorDto.Exception = context.Exception;
            }

            context.Result = new JsonResult<GeneralError>(
                generalErrorDto, 
                new JsonSerializerSettings(), 
                Encoding.UTF8,
                context.Request);

            exceptionLogger.Error(context.Exception);
        }
    }
}

