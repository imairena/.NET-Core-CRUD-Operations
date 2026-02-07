using Microsoft.EntityFrameworkCore;

namespace StudentAPI.Models;

// Decides what should be inside the actual physical DB after migration
// and it is also responsible for establishing a DB connection with the SQL Server DB
public class APIDbContext:DbContext
{
    public APIDbContext(DbContextOptions option):base(option) // Constructor for APIDbContext
    { }
    
    // To create a corresponding table for the Student model inside
    // the new DB we need an entity mapping to the model class
    public DbSet<Student> Students { get; set; } // Name of table will be Students
}

// Note: Migration is the process of converting entity framework core models into
// a corresponding SQL Server DB