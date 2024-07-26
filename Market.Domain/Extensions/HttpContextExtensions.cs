using Microsoft.AspNetCore.Http;

namespace Market.Domain.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetValueByClaimType(this HttpContext httpContext, string type)
        {
            var value = httpContext.User.Claims.FirstOrDefault(claim =>
                claim.Type == type)!.Value;

            return value;
        }
    }
}
