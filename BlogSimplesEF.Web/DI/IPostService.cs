using BlogSimplesEF.Web.Models.Entities;
using BlogSimplesEF.Web.Models.Response;

namespace BlogSimplesEF.Web.DI
{
    public interface IPostService
    {
        Task<Resultado> AtualizarPostDoBlog(Post Post);
        Task<Resultado> CriarPostDoBlog(Post Post);
        Task<Resultado> ExcluirPostDoBlog(int PostId);
        Task<List<ResponsePost>> ListarTodosPosts();
        Task<Post?> GetPostById(int? postId);
    }
}