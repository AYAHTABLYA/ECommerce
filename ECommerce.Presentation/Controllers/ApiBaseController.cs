using ECommerce.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandelProblem(result.Errors);
        }

        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return HandelProblem(result.Errors);
        }
        private ActionResult HandelProblem(IReadOnlyList<Error> errors)
        {
            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError,
                               title: "Internal Server Error",
                               detail: "Unexpected Error Occured");
            
            if(errors.All(E => E.Type == ErrorType.Validation))
                return HandelValidationProblem(errors);

            return HandelSingleErrorProblem(errors[0]);
        }

        private ActionResult HandelValidationProblem(IReadOnlyList<Error> errors)
        {
            // ModelState
            var ModelState = new ModelStateDictionary();
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Code, error.Desctiption);
            }
            return ValidationProblem(ModelState);
        }

        private ActionResult HandelSingleErrorProblem(Error error)
        {
            return Problem(title: error.Code,
                           detail: error.Desctiption,
                           type: error.Type.ToString(),
                           statusCode: MapErrorTypeToStatusCode(error.Type)
            );
        }

        private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        protected string GetEmailFromToken() => User.FindFirstValue(ClaimTypes.Email)!;
    }
}
