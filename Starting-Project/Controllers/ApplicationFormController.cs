using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Starting_Project.Models.CosmosDb;
using Starting_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Starting_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationFormController : ControllerBase
    {
        private readonly ApplicationFormContext _context;

        public ApplicationFormController(ApplicationFormContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplicationForm([FromBody] ApplicationFormDto formDto)
        {
            var application = new ProgramApplication
            {
                ProgramName = formDto.ProgramName,
                Questions = formDto.Questions?.ConvertAll(q => new Question
                {
                    QuestionText = q.QuestionText,
                    Type = q.Type,
                    Options = q.Options
                })
            };

            _context?.Applications?.Add(application);
            await _context?.SaveChangesAsync()!;

            return CreatedAtAction(nameof(GetApplicationForm), new { id = application.Id }, application);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationForm(Guid id)
        {
            var application = await _context.Applications?
                .Include(a => a.Questions)
                .FirstOrDefaultAsync(a => a.Id == id)!;

            if (application == null)
            {
                return NotFound();
            }

            return Ok(application);
        }
        [HttpGet("questions/{type}")]
        public async Task<IActionResult> GetQuestionsByType(QuestionType type)
        {
            var questions = await _context.Questions
                                          .Where(q => q.Type == type)
                                          .ToListAsync();

            if (questions == null || questions.Count == 0)
            {
                return NotFound("No questions found for the specified type.");
            }

            return Ok(questions);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplicationForm(Guid id, [FromBody] ApplicationFormDto formDto)
        {
            var application = await _context.Applications?.Include(a => a.Questions).FirstOrDefaultAsync(a => a.Id == id)!;
            if (application == null)
            {
                return NotFound();
            }

            application.ProgramName = formDto.ProgramName;
            application.Questions = formDto.Questions?.ConvertAll(q => new Question
            {
                Id = q.Id, // Ensure to pass Ids in DTO to map correctly
                QuestionText = q.QuestionText,
                Type = q.Type,
                Options = q.Options
            });

            _context.Applications.Update(application);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content is standard for a successful PUT request
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationForm(Guid id)
        {
            var application = await _context.Applications!.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content is standard for a successful DELETE request
        }
    }
}
