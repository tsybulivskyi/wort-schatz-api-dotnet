public class WordDto
{
    public int Id { get; set; }
    public required string Original { get; set; }
    public required string Translation { get; set; }
    public required string[] Tags { get; set; } = new string[0];
}
