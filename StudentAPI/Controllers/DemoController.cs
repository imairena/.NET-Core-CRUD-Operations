using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        public string Greetings()
        {
            return "Hello, World!";
        }
        
        /*
        public IsActionResult Greetings()
        {
            return Ok("Hello, World!"); // Ok means that the operation returns status code 200
        }
        */
        
        /*
        public ActionResult<string> Greetings()
        {
        `if 
        `{
            return Ok("Hello, World!"); // Ok means that the operation returns status code 200
        `}
         else
         {
            return NoContent();
         }
        }
        */
        
        // CRUD
        // Create - a new record - POST
        // Read - retrieve a single list of record(s) - can be achieved with a web method of type GET
        // Update - modify an existing record - PUT
        // Delete - remove an existing record - DELETE

        // HTTP Verbs (a lot more but these are the most common ones)
        // GET
        // POST
        // PUT or PATCH
        // DELETE
        
        // Return types of a Web API:
        // Specific Type: int, string, new Class1()
        // IsActionResult: Used when we need to return multiple types of data from one method
        // ActionResult<T>: If we need to return either an action or an IActionResult
    }
}