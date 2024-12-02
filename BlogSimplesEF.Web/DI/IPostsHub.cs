namespace BlogSimplesEF.Web.DI
{
    public interface IPostsHub
    {
        Task SendMessage(string user, string message);
    }
}
