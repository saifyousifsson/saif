
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
         return NotFound();
        }
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
         return BadRequest(new ProblemDetails{Title="Bad Request Occurs"});
        }
        [HttpGet("unauthorized")]
        public ActionResult GetUnAuthorized()
        {
         return Unauthorized();
        }
        [HttpGet("validation-error")]
        public ActionResult GetValidationError()
        {
            ModelState.AddModelError("Error1","This is the first error");
            ModelState.AddModelError("Error2","This is the second error");

         return ValidationProblem();
        }
        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
         throw new Exception("This is server error");
        }
        
    }
}