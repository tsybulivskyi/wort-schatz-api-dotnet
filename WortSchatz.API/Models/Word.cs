namespace WordTranslationApp.Models;

public record Word
{
    public int Id { get; set; } // EF Core primary key
    public required string Original { get; set; }
    public required string Translation { get; set; }
    public required ICollection<Tag> Tags { get; set; } = [];

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

    public WordDto ToDto()
    {
        return new WordDto
        {
            Id = Id,
            Original = Original,
            Translation = Translation,
            Tags = Tags.Select(t => t.Name).ToArray()
        };
    }
}

public record Tag
{
    public int Id { get; set; } // EF Core primary key
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}