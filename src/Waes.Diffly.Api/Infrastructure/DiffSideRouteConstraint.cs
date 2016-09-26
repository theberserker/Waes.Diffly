using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Waes.Diffly.Core.Domain.Enums;

namespace Waes.Diffly.Api.Infrastructure
{
    /// <summary>
    /// Implements route matching by the DiffSide enum values.
    /// </summary>
    public class DiffSideRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var response = Enum.GetNames(typeof(DiffSide))
                .Any(s => s.ToLowerInvariant() == values[routeKey].ToString().ToLowerInvariant());

            return response;
        }
    }
}
