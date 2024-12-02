using BlogSimplesEF.Web.Data.Blog;
using BlogSimplesEF.Web.DI;
using BlogSimplesEF.Web.Models.Entities;
using BlogSimplesEF.Web.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace BlogSimplesEF.Web.Business
{
    public class PostService : IPostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly BlogContext _db;

        public PostService(ILogger<PostService> logger, BlogContext db)
        {
            _logger = logger;
            _db = db;
        }

        //Obs: Método de exemplo de listagem, em um cenário real listar os registros paginando
        //Por questão de tempo não foi implementado uma listagem simplificada
        public async Task<List<ResponsePost>> ListarTodosPosts()
        {
            _logger.LogInformation("[EventosService] Listar todos os registros");

            var retornaLista = await _db.Posts
                                        .Include(i => i.UserBlog)
                                        .Select(s => new ResponsePost()
                                        {
                                            Id = s.Id,
                                            Title = s.Title,
                                            Summary = s.Summary,
                                            Content = s.Content,
                                            UserId = s.UserId,
                                            UserBlog = s.UserBlog.UserName,
                                            PublishedOn = s.PublishedOn
                                        })
                                        .OrderByDescending(t => t.PublishedOn)
                                        .AsNoTracking()
                                        .ToListAsync();

            return retornaLista;
        }

        public async Task<Resultado> CriarPostDoBlog(Post Post)
        {
            _logger.LogInformation("[PostService] Criar Post: {0}", Post.Title);

            var resultado = new Resultado()
            {
                Acao = "Criação de Post"
            };

            _db.Posts.Add(Post);

            var resultDb = await _db.SaveChangesAsync();

            if (resultDb < 1)
                resultado.Inconsistencias.Add("Registro não criado");

            return resultado;
        }

        public async Task<Resultado> AtualizarPostDoBlog(Post Post)
        {
            _logger.LogInformation("[PostsService] Atualizar Post", Post.Title);

            var resultado = new Resultado()
            {
                Acao = "Atualização de Post"
            };

            _db.Posts.Update(Post).State = EntityState.Modified;

            var resultDb = await _db.SaveChangesAsync();

            if (resultDb < 1)
                resultado.Inconsistencias.Add("Registro não atualizado");

            return resultado;
        }

        public async Task<Resultado> ExcluirPostDoBlog(int PostId)
        {
            _logger.LogInformation("[PostService] Exclusão PostId", PostId);

            var resultado = new Resultado()
            {
                Acao = "Exclusão de Post"
            };

            _db.Posts.Remove(new Post() { Id = PostId });

            var resultDb = await _db.SaveChangesAsync();

            if (resultDb < 1)
                resultado.Inconsistencias.Add("Não foi possível excluir o Post");

            return resultado;
        }

        public async Task<Post?> GetPostById(int? postId)
        {
            return await _db.Posts
                            .FirstOrDefaultAsync(x => x.Id == postId);
        }
    }
}
