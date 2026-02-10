using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Controllers;
using StudentAPI.Models;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace StudentAPI.Tests
{
    public class StudentControllerTests
    {
        // Helper method to create a new InMemory Database Context for each test
        // This ensures tests do not interfere with each other by using a unique database name
        private APIDbContext GetDatabaseContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<APIDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            
            var databaseContext = new APIDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        // Test checking if GetStudents returns all students from the database
        [Fact]
        public async Task GetStudents_ReturnsAllStudents()
        {
            // Arrange: Setup the in-memory database with test data
            var dbName = Guid.NewGuid().ToString();
            using (var context = GetDatabaseContext(dbName))
            {
                context.Students.Add(new Student { StudentId = 1, Name = "John Doe", Age = 20, ContactNumber = 12345 });
                context.Students.Add(new Student { StudentId = 2, Name = "Jane Doe", Age = 22, ContactNumber = 67890 });
                await context.SaveChangesAsync();
            }

            // Act: Execute the controller method
            using (var context = GetDatabaseContext(dbName))
            {
                var controller = new StudentController(context);
                var result = await controller.GetStudents();

                // Assert: Verify the results
                var actionResult = Assert.IsType<ActionResult<IEnumerable<Student>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Student>>(actionResult.Value);
                Assert.Equal(2, model.Count());
            }
        }

        // Test checking if GetStudent returns a specific student when they exist
        [Fact]
        public async Task GetStudent_ReturnsStudent_WhenStudentExists()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using (var context = GetDatabaseContext(dbName))
            {
                context.Students.Add(new Student { StudentId = 1, Name = "John Doe", Age = 20, ContactNumber = 12345 });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = GetDatabaseContext(dbName))
            {
                var controller = new StudentController(context);
                var result = await controller.GetStudent(1);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Student>>(result);
                var model = Assert.IsType<Student>(actionResult.Value);
                Assert.Equal(1, model.StudentId);
                Assert.Equal("John Doe", model.Name);
            }
        }

        // Test checking if GetStudent returns NotFound when the student ID does not exist
        [Fact]
        public async Task GetStudent_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using (var context = GetDatabaseContext(dbName))
            {
                // Database is empty
            }

            // Act
            using (var context = GetDatabaseContext(dbName))
            {
                var controller = new StudentController(context);
                var result = await controller.GetStudent(99);

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        // Test checking if PostStudent correctly adds a new student to the database
        [Fact]
        public async Task PostStudent_AddsStudent()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var newStudent = new Student { StudentId = 3, Name = "New Student", Age = 25, ContactNumber = 11111 };

            // Act
            using (var context = GetDatabaseContext(dbName))
            {
                var controller = new StudentController(context);
                var result = await controller.PostStudent(newStudent);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                var model = Assert.IsType<Student>(createdAtActionResult.Value);
                Assert.Equal("New Student", model.Name);
                Assert.Equal(3, model.StudentId);
            }

            // Verify it was actually saved to DB
            using (var context = GetDatabaseContext(dbName))
            {
                var savedStudent = await context.Students.FindAsync(3);
                Assert.NotNull(savedStudent);
                Assert.Equal("New Student", savedStudent.Name);
            }
        }

        // Test checking if PutStudent updates an existing student
        [Fact]
        public async Task PutStudent_UpdatesStudent()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using (var context = GetDatabaseContext(dbName))
            {
                context.Students.Add(new Student { StudentId = 1, Name = "Original Name", Age = 20, ContactNumber = 12345 });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = GetDatabaseContext(dbName))
            {
                var controller = new StudentController(context);
                var updatedStudent = new Student { StudentId = 1, Name = "Updated Name", Age = 21, ContactNumber = 12345 };
                
                var result = await controller.PutStudent(1, updatedStudent);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            // Verify update
            using (var context = GetDatabaseContext(dbName))
            {
                var student = await context.Students.FindAsync(1);
                Assert.Equal("Updated Name", student.Name);
                Assert.Equal(21, student.Age);
            }
        }

        // Test checking if DeleteStudent removes a student from the database
        [Fact]
        public async Task DeleteStudent_RemovesStudent()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using (var context = GetDatabaseContext(dbName))
            {
                context.Students.Add(new Student { StudentId = 1, Name = "To Be Deleted", Age = 20, ContactNumber = 12345 });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = GetDatabaseContext(dbName))
            {
                var controller = new StudentController(context);
                var result = await controller.DeleteStudent(1);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            // Verify deletion
            using (var context = GetDatabaseContext(dbName))
            {
                var student = await context.Students.FindAsync(1);
                Assert.Null(student);
            }
        }
    }
}
