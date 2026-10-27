using ECommerce.ServicesAbstarction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ECommerce.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int durationInMinute;
        public RedisCacheAttribute(int DurationInMinute = 5)
        {
            durationInMinute = DurationInMinute;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get Cashe Service
            var casheService = context.HttpContext.RequestServices.GetService<ICacheServices>();
            // Check Cashe if Existed
            var casheKey = CreateCasheKey(context.HttpContext.Request);
            // If Existed => Return Cashe Value and Do Not Execute the End Point
            var casheValue = await casheService.GetAsync(casheKey);
            if (casheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = casheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };

                return;
            }

            // If Not Existed => Continue to the End Point
            var executedContext = await next.Invoke();
            if (executedContext.Result is OkObjectResult result)
            {
                // Call SetAsync to Store the Result in Cashe
                await casheService.SetAsync(casheKey, result.Value!, TimeSpan.FromMinutes(5));
            }
        }


        private string CreateCasheKey(HttpRequest request)
        {
            //BaseUrl/api/Products
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path);

            foreach (var item in request.Query.OrderBy(X => X.Key))
            {
                Key.Append($"|{item.Key}-{item.Value}");
            }

            return Key.ToString();
        }
    }
}