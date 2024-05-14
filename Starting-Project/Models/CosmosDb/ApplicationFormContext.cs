
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Starting_Project.Models.CosmosDb
{
    public class ApplicationFormContext : DbContext
    {
        public DbSet<ProgramApplication>? Applications { get; set; }
        public DbSet<Question> Questions { get; set; }

        public ApplicationFormContext(DbContextOptions<ApplicationFormContext> options)
          : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Optionally add further configuration that's not provided via DI
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseCosmos(
                //    "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                //    "programDb"
                //);
            }
        }
    }
}
