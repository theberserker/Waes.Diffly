using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Waes.Diffly.Core.Exceptions;
using System.Net;
using Waes.Diffly.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Waes.Diffly.Api.Infrastructure
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            SetResponseIfDomainException(context);
            base.OnException(context);
        }

        /// <summary>
        /// Handles the appropriate response status and message for the <see cref="DiffDomainException"/>. 
        /// In case the there was a <see cref="DiffDataIncompleteException"/>, there is no data to compare for at least one of the sides.
        /// In case it was any other DiffDomainException exception this was a client mistake and we report is as a BadRequest.
        /// </summary>
        /// <param name="context"></param>
        public void SetResponseIfDomainException(ExceptionContext context)
        {
            var diffDomainException = context.Exception as DiffDomainException;
            if (diffDomainException != null)
            {
                if (diffDomainException is DiffDataIncompleteException)
                {
                    context.Result = new NotFoundResult();
                }
                else
                {
                    var errorDto = new ErrorDto { Message = diffDomainException.Message };
                    var errorResult = new JsonResult(errorDto) { StatusCode = (int)HttpStatusCode.BadRequest };
                    context.Result = errorResult;
                }
            }

        }
    }
}
