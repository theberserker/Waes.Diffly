using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;

namespace Waes.Diffly.UnitTest
{
    public class ConstraintsTestHelper
    {
        public static bool TestConstraint(IRouteConstraint constraint, object value, Action<IRouter> routeConfig = null)
        {
            var context = new Mock<HttpContext>();

            var route = new RouteCollection();

            if (routeConfig != null)
            {
                routeConfig(route);
            }

            var parameterName = "fake";
            var values = new RouteValueDictionary() { { parameterName, value } };
            var routeDirection = RouteDirection.IncomingRequest;
            return constraint.Match(context.Object, route, parameterName, values, routeDirection);
        }
    }
}
