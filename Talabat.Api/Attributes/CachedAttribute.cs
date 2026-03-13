using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Core.ServicesContract;
using System.Text;

namespace Store.APIs.Attributes
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CachedAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.GetCacheKeyAsync(cacheKey);
            if(!string.IsNullOrEmpty(cacheResponse))
            {
                var ContentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "aplication/json",
                    StatusCode = 200
                };

                context.Result = ContentResult;
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult response)
            {
                await cacheService.SetCacheKeyAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_expireTime));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest httpRequest)
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append($"{httpRequest.Path}");
            foreach ( var (key , value) in httpRequest.Query.OrderBy(X => X.Key) )
            {
                cacheKey.Append($"|{key}-{value}");
            }

            return cacheKey.ToString();
        }
    }
}
