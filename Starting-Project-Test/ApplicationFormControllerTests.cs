using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Starting_Project.Controllers;
using Starting_Project.Models;
using Starting_Project.Models.CosmosDb;

namespace Starting_Project_Test
{
    public class ApplicationFormControllerTests
    {
        private readonly ApplicationFormContext _context;
        private readonly ApplicationFormController _controller;

        public ApplicationFormControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationFormContext>()
             .UseInMemoryDatabase(databaseName: "programDb")  // Create a new database instance for each test method
             .Options;

            _context = new ApplicationFormContext(options);
            _controller = new ApplicationFormController(_context);
        }
        [Fact]
        public async Task CreateApplicationForm_ReturnsCreatedAtActionResult_WithValidData()
        {
            // Arrange
            var formDto = new ApplicationFormDto
            {
                ProgramName = "New Program",
                Questions = new List<QuestionDto>() {
            new QuestionDto { QuestionText = "Sample Question", Type = QuestionType.Paragraph }
        }
            };

            // Act
            var result = await _controller.CreateApplicationForm(formDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedApplication = createdAtResult.Value as ProgramApplication;
            Assert.NotNull(returnedApplication);
            Assert.Equal("New Program", returnedApplication.ProgramName);
        }
        [Fact]
        public async Task GetApplicationForm_ReturnsOkObjectResult_WithExistingId()
        {
            // Arrange
            var newApplication = new ProgramApplication
            {
                ProgramName = "Existing Program",
                Questions = new List<Question> { new Question { QuestionText = "What is your name?", Type = QuestionType.Paragraph } }
            };
            _context?.Applications?.Add(newApplication);
            await _context?.SaveChangesAsync()!;

            // Act
            var result = await _controller.GetApplicationForm(newApplication.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var application = okResult.Value as ProgramApplication;
            Assert.NotNull(application);
            Assert.Equal("Existing Program", application.ProgramName);
        }
        [Fact]
        public async Task DeleteApplicationForm_ReturnsNoContentResult_WhenApplicationExists()
        {
            // Arrange
            var existingApplication = new ProgramApplication
            {
                ProgramName = "Existing Program",
                Questions = new List<Question> { new Question { QuestionText = "Sample Question", Type = QuestionType.Paragraph } }
            };
            await _context.Applications!.AddAsync(existingApplication);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteApplicationForm(existingApplication.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify it's deleted
            var deletedApplication = await _context.Applications.FindAsync(existingApplication.Id);
            Assert.Null(deletedApplication);
        }

        [Fact]
        public async Task DeleteApplicationForm_ReturnsNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await _controller.DeleteApplicationForm(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task UpdateApplicationForm_ReturnsNoContentResult_WhenDataIsValid()
        {
            // Arrange
            var existingApplication = new ProgramApplication
            {
                ProgramName = "Original Program",
                Questions = new List<Question> { new Question { QuestionText = "Original Question", Type = QuestionType.Paragraph } }
            };
            await _context.Applications!.AddAsync(existingApplication);
            await _context.SaveChangesAsync();

            var updatedDto = new ApplicationFormDto
            {
                ProgramName = "Updated Program",
                Questions = new List<QuestionDto> {
            new QuestionDto { QuestionText = "Updated Question", Type = QuestionType.Paragraph }
        }
            };

            // Act
            var result = await _controller.UpdateApplicationForm(existingApplication.Id, updatedDto);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Retrieve to verify update
            var updatedApplication = await _context.Applications.FindAsync(existingApplication.Id);
            Assert.Equal("Updated Program", updatedApplication!.ProgramName);
            Assert.Single(updatedApplication.Questions!);
            Assert.Equal("Updated Question", updatedApplication?.Questions?[0].QuestionText);
        }

        [Fact]
        public async Task UpdateApplicationForm_ReturnsNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var applicationDto = new ApplicationFormDto { ProgramName = "Some Program" };

            // Act
            var result = await _controller.UpdateApplicationForm(nonExistingId, applicationDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}