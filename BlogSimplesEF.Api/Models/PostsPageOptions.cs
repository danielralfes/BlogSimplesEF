namespace BlogSimplesEF.Api.Models;

public record PostsPageOptions : PageOptions
{
    public string? Term { get; set; }
    public string? Category { get; set; }
}
