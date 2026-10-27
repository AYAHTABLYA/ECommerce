using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationresponse(ActionContext actionContext)
        {
            var Errors = actionContext.ModelState.Where(X => X.Value.Errors.Count > 0)
                                     .ToDictionary(X => X.Key, X => X.Value.Errors
                                     .Select(X => X.ErrorMessage).ToArray());

            var Problem = new ProblemDetails
            {
                Title = "Validation Error",
                Detail = "One or more validation errors occurred",
                Status = StatusCodes.Status400BadRequest,
                Extensions =
                        {
                            { "errors", Errors }
                        }
            };
            return new BadRequestObjectResult(Problem);

        }
    }
}
