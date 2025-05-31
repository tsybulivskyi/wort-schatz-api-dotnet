namespace WordTranslationApp.Models;

public record Word
{
    public int Id { get; set; } // EF Core primary key
    public string Original { get; set; }
    public string Translation { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public static Word FromDto(WordDto dto)
    {
        return new Word
        {
            Id = dto.Id,
            Original = dto.Original,
            Translation = dto.Translation,
            Tags = dto.Tags.Select(tagName => new Tag { Name = tagName, Color = string.Empty }).ToList()
        };
    }
}

public record Tag
{
    public int Id { get; set; } // EF Core primary key
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}