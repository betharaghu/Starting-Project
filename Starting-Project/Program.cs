using Microsoft.EntityFrameworkCore;
using Starting_Project.Models.CosmosDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationFormContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var cosmosDbConfig = configuration.GetSection("CosmosDb");
    options.UseCosmos(
        cosmosDbConfig["AccountEndpoint"]!,
        cosmosDbConfig["AccountKey"]!,
        cosmosDbConfig["DatabaseName"]!
    );
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationFormContext>();
    await dbContext.Database.EnsureCreatedAsync(); // Asynchronously ensure the database and container are created
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
