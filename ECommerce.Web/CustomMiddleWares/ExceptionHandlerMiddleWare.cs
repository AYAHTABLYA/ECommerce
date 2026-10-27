using ECommerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.Web.CustomMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate Next, ILogger<ExceptionHandlerMiddleWare> Logger)
        {
            _next = Next;
            _logger = Logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
			try
            {
                await _next.Invoke(context);

                //404 Not Found
                await HandelNotFoundEndPointAsync(context);
            }
            catch (Exception ex)
			{
                _logger.LogError(ex, "Something went wrong!");

                Console.WriteLine(ex.Message);

                var Problem = new ProblemDetails()
                {
                    Title = "An Unexpected error!",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    Status = ex switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    }
                };

                context.Response.StatusCode = Problem.Status.Value;
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }

        private static async Task HandelNotFoundEndPointAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound && !context.Response.HasStarted)
            {
                var Problem = new ProblemDetails()
                {
                    Title = "The resource you are looking for is not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"The resource at {context.Request.Path} is not found",
                    Instance = context.Request.Path
                };
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }
    }
}
