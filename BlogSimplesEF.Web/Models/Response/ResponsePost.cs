namespace BlogSimplesEF.Web.Models.Response
{
    public class ResponsePost
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string UserId { get; set; }
        public string? UserBlog { get; set; }
    }
}
