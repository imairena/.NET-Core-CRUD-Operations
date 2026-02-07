using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Models;

public class Student
{
    [Key]
    public int StudentId { get; set; }

    [Required] // Makes the data field required
    [Column(TypeName = "nvarchar(250)")] // Best practice is to specify type
    public string Name { get; set; } = " "; // default value is empty string
    
    public int ContactNumber { get; set; }
    
    public int Age { get; set; }
    
}