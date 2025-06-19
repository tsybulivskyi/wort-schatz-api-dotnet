using FirebaseAdmin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WordTranslationApp;
using WordTranslationApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapGet("/words", async (WordRepository repo) =>
{
    var words = await repo.GetAllAsync();
    var wordDtos = words.Select(w => new WordDto
    {
        Id = w.Id,
        Original = w.Original,
        Translation = w.Translation,
        Tags = w.Tags.Select(t => t.Name).ToArray()
    });
    return Results.Ok(wordDtos);
}).RequireAuthorization();

app.MapPost("/words", async (WordDto wordDto, WordRepository repo) =>
{
    var word = Word.FromDto(wordDto);

    await repo.AddAsync(word);
    return Results.Created($"/words/{word.Id}", word);
}).RequireAuthorization();

app.MapDelete("/words", async (WordRepository repo) =>
{
    await repo.DeleteAllAsync();
    return Results.NoContent();
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

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
