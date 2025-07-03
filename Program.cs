using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WordTranslationApp;
using WordTranslationApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();


builder.Services.AddDbContext<WortSchatzDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<WordRepository>();
builder.Services.AddScoped<TagRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["jwt:Issuer"];
        options.Audience = builder.Configuration["jwt:Audience"];
    });

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks()
    .AddCheck<SampleHealthCheck>("Sample");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

//services.AddHealthChecksUI();
app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });


// Seed test data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WortSchatzDbContext>();
    if (!db.Words.Any())
    {
        var tag1 = new Tag { Name = "Noun", Color = "blue" };
        var tag2 = new Tag { Name = "Verb", Color = "red" };
        var word1 = new Word { Original = "Haus", Translation = "House", Tags = new List<Tag> { tag1 } };
        var word2 = new Word { Original = "gehen", Translation = "to go", Tags = new List<Tag> { tag2 } };
        db.Tags.AddRange(tag1, tag2);
        db.Words.AddRange(word1, word2);
        db.SaveChanges();
    }
}

app.Run();

