using Presentacion.Mvc.App.Utilidades;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Presentacion.Mvc.App.Handler
{
    public class EmerErrHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            //base.Handle(context);
            const string errorMessage = "An unexpected error occured";
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new
                {
                    Message = errorMessage
                });
            response.Headers.Add("X-Error", errorMessage);
            context.Result = new ResponseMessageResult(response);
            if (context.Exception != null)
                new Task(() => { Utils.RegistrarLogWindows("EmerErrHandler_Handle", context.Exception); }).Start();
        }

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            if (context.Exception != null)
                new Task(() => { Utils.RegistrarLogWindows("EmerErrHandler_ShouldHandle", context.Exception); }).Start();

            return base.ShouldHandle(context);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.

        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            //return base.HandleAsync(context, cancellationToken);
            // Access Exception using context.Exception;
            const string errorMessage = "An unexpected error occured";
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new
                {
                    Message = errorMessage
                });
            response.Headers.Add("X-Error", errorMessage);
            context.Result = new ResponseMessageResult(response);

            if (context.Exception != null)
                new Task(() => { Utils.RegistrarLogWindows("EmerErrHandler_HandleAsync", context.Exception); }).Start();
        }
    }
}