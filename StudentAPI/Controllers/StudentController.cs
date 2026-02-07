using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase 
    /*
     An instance of the Student Controller class will be created everytime we make a requenst into any
     of the web API methods
     */
    {
       private readonly APIDbContext _context; // web API methods use _context to interact with DB
       public StudentController(APIDbContext context) // Constructor
       // Note that we get the parameter through dependency injection
        {
            _context = context;
        }
       
       // web API methods:
       // GET: api/Student
       [HttpGet] // returns 200 (OK)
       public async  Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync(); // returns all the records in the Student table
        }
       
       // GET: api/Student/5
       [HttpGet("{id}")]
       public async Task<ActionResult<Student>> GetStudent(int id)
       {
           var student = await _context.Students.FindAsync(id);

           if (student == null)
           {
               return NotFound();
           }
           return student;
       }
       
       // PUT: api/Student/5
       [HttpPut("{id}")] // usually returns status 204 (No Content)
       public async Task<IActionResult> PutStudent(int id, Student student)
       {
           if (id != student.StudentId)
           {
               return BadRequest(); // 400 status code
           }
           _context.Entry(student).State = EntityState.Modified;

           try
           {
               await _context.SaveChangesAsync();
           }
           catch (DbUpdateConcurrencyException)
           // This code runs when there are two users, one trying to update a record, and one trying to
           // delete the same record at the same time
           {
               if (!StudentExists(id)) // Check if record is present or not
               {
                   return NotFound(); 
               }
               else
               {
                   throw;
               }
           }

           return NoContent(); // 204 No Content: Standard success response for PUT
       }
       
       // POST: api/Student
       [HttpPost] // usually returns status 201 (created)
       public async Task<IActionResult> PostStudent(Student student)
       {
           // The following two lines are what add a new record to the table
           _context.Students.Add(student);
           await _context.SaveChangesAsync();
           
           // Once record is added, the record's details will be inserted into "student"
           // URL to GET method is created because of the first two parameters "GetStudent" and "id"
           return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);
       }
       
       // DELETE: api/Student/5
       [HttpDelete("{id}")] // usually returns status 204 (No Content)
       public async Task<IActionResult> DeleteStudent(int id)
       {
           var student = await _context.Students.FindAsync(id); // retrieve corresponding record
           if (student == null)
           {
               return NotFound(); // 404 Student was not found
           }
           
           _context.Students.Remove(student); // Remove item from DB set "student"
           await _context.SaveChangesAsync(); // Save changes
           
           return NoContent();
       }

       private bool StudentExists(int id)
       {
           return _context.Students.Any(e => e.StudentId == id);
       }
    }
}

// Notes to self: The async keyword means we can execute many operations at the same time, so a function
// with the async keyword does not need to wait for other operations to finish running to execute.
// An asynchronous operation must be prefixed with the keyword await.

// Functions that take in a Student take in a JSON object
// Functions that take in an id take in a StudentID from the Student table (i.e. the student is searched
// by id)

// STATUS CODES:
// 1XX: Informational; 2XX: Success;
// 3XX: Redirection - Indicates that the client must take some additional action in order to complete their request
// 4XX: Client Error; 5XX: Server Error
// 200 (Success)
// 201 (Created): Resource successfully created on server
// 204  (No Content) Nothing important to be written
// 400 (Bad Request): Usually means the data passed to the server is not valid
// 401 (Un-authorized): Not allowed to access a resource (e.g. provided wrong password)
// 403 (Forbidden): We are trying to access a web API method outside of our permission
// 404 (Not Found): The resource we are trying to access is not available or DNE
// 500 (Internal Server Error): Error inside the web API implementation 