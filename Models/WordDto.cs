public class WordDto
{
    public int Id { get; set; }
    public string Original { get; set; }
    public string Translation { get; set; }
    public string[] Tags { get; set; } = new string[0];
}
