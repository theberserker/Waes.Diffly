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

        public void SetResponseIfDomainException(ExceptionContext context)
        {
            var diffDomainException = context.Exception as DiffDomainException;
            if (diffDomainException != null)
            {
                var errorDto = new ErrorDto { Message = diffDomainException.Message };
                var errorResult = new JsonResult(errorDto) { StatusCode = (int)HttpStatusCode.BadRequest };
                context.Result = errorResult;
            }
        }
    }
}
