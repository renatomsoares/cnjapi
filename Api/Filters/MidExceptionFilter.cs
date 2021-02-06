using System.Linq;
using Application.Filters.Notifications;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Helpers.Exceptions;

namespace Application.Filters {
    public class MidExceptionFilter : ActionFilterAttribute, IExceptionFilter {

        private readonly ILogger _logger;

       public MidExceptionFilter (ILoggerFactory logger) {
            if (logger == null) {
                throw new System.ArgumentNullException (nameof (logger));
            }
            this._logger = logger.CreateLogger ("Global Exception Filter");
        }
        
        private Notification ReturnValidation(ExceptionContext context)
        {
            Notification errorResponse = new Notification();

            if (!((ValidationException) context.Exception).Errors.Any())
            {
                var description = new ErrorDescription(context.Exception.Message);
                errorResponse.Add(description.ToString());
            }

            foreach (var validationsfailures in ((ValidationException) context.Exception).Errors)
            {
               var description = new ErrorDescription(validationsfailures.ErrorMessage);
                errorResponse.Add(description.ToString());
            }

            return errorResponse;
        }

        public void OnException (ExceptionContext context) {
            var result = new ContentResult {ContentType = "application/json"};

            string content = null;

            //only handle ValidationExceptions
            if ((context.Exception as ValidationException) != null)
            {
                //Trata Result baseado no FluentValidation
                var erros = ReturnValidation(context);
                result.Content = JsonConvert.SerializeObject(erros,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                context.HttpContext.Response.StatusCode = 400;
                context.ExceptionHandled = true;
                context.Result = result;
            }
            else if ((context.Exception as NotFoundException) != null)
            {
                context.HttpContext.Response.StatusCode = 404;
                context.ExceptionHandled = true;
                context.Result = null;

                this._logger.LogError(context.Exception, context.ActionDescriptor.DisplayName);
            }
            else
            {
                context.HttpContext.Response.StatusCode = 500;
                context.ExceptionHandled = true;
                context.Result = null;
                
                //this._logger.LogError ("GlobalExceptionFilter", context.Exception);
                this._logger.LogError(context.Exception,context.ActionDescriptor.DisplayName);
            }


        }
  }
}