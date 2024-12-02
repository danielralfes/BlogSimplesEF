using BlogSimplesEF.Web.Business;
using BlogSimplesEF.Web.Models.Entities;
using BlogSimplesEF.Web.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BlogSimplesEF.Web.Hub;
using Microsoft.AspNetCore.SignalR;

namespace BlogSimplesEF.Web.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly PostService _service;
        ClaimsPrincipal currentUser;
        private readonly IHubContext<PostsHub> _hubContext;

        public PostsController(ILogger<PostsController> logger, PostService service, IHubContext<PostsHub> hubContext)
        {
            _logger     = logger;
            _service    = service;
            _hubContext = hubContext;
        }

        private string RetornarUserId()
        {
            if (this.User != null && this.User.Identity.IsAuthenticated)
            {
                currentUser = this.User;
                return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            return null;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePost), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ResponsePost>> ListarTodosPosts()
        {
            _logger.LogInformation($"[PostsController.ListarTodosPosts] Listagem de todos os posts");

            try
            {
                var resultado = await _service.ListarTodosPosts();

                if (!resultado.Any())
                    return NotFound("Não existe registros de posts");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PostsController.ListarTodosPosts]  Erro na chamada do 'Get'");

                return BadRequest("Erro ao executar chamada");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Resultado>> CriarPostDoBlog(RequestPost Post)
        {
            _logger.LogInformation($"[BlogSimplesEF.Web.Posts] Criar Posts");

            try
            {
                var postDb = new Post()
                {
                    Content = Post.Content,
                    PublishedOn = DateTime.Now,
                    Summary = Post.Summary,
                    Title = Post.Title,
                    UserId = RetornarUserId()
                };

                if(postDb.UserId == null)
                    return BadRequest(new Resultado()
                    {
                        Acao = "Criação de Post",
                        Inconsistencias = { "Usuário não autenticado" }
                    });

                var resultado = await _service.CriarPostDoBlog(postDb);

                if (resultado.Inconsistencias.Count > 0)
                {
                    _logger.LogError("[BlogSimplesEF.Web.Posts] " + Util.GetJSONResultado(resultado));
                    return BadRequest(resultado);
                }
                else
                {
                    _logger.LogInformation("[BlogSimplesEF.Web.Posts] Inclusão efetuada com sucesso");
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", User?.Identity?.Name ?? "Anonimo", postDb.Title);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BlogSimplesEF.Web.Posts] Erro na chamada do 'Post'");

                return BadRequest(new Resultado()
                {
                    Acao = "Criação de Post",
                    Inconsistencias = { "Exceção ao efetuar a inclusão do Post" }
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Resultado>> AtualizarPostDoBlog(RequestPost post)
        {
            _logger.LogInformation($"[BlogSimplesEF.Web.Posts] Atualizar PostId: {post.Id}");

            try
            {
                var resultado = new Resultado();
                var usrId     = RetornarUserId();
                Post? postDb  = await _service.GetPostById(post.Id);

                if (postDb == null)
                    resultado.Inconsistencias.Add("Registro não encontrado");
                else  if (usrId == null)
                    return BadRequest(new Resultado()
                    {
                        Acao            = "Usuário de Post",
                        Inconsistencias = { "Usuário não autenticado" }
                    });
                else if (postDb.UserId != RetornarUserId())
                    resultado.Inconsistencias.Add("Registro não atualizado - Sem Permissão");

                var PostDb = new Post()
                {
                    Id = post.Id.GetValueOrDefault(),
                    Content = post.Content,
                    PublishedOn = DateTime.Now,
                    Summary = post.Summary,
                    Title = post.Title,
                    UserId = RetornarUserId()
                };

                resultado = await _service.AtualizarPostDoBlog(PostDb);

                if (resultado.Inconsistencias.Count > 0)
                {
                    _logger.LogError(Util.GetJSONResultado(resultado));
                    return BadRequest(resultado);
                }
                else
                    _logger.LogInformation("[BlogSimplesEF.Web.Posts] Alteracao efetuada com sucesso");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BlogSimplesEF.Web.Posts] Erro na chamada do 'Put'");

                return BadRequest(new Resultado()
                {
                    Acao = "Atualização de Post",
                    Inconsistencias = { "Exceção ao efeturar a inclusão do Post" }
                });
            }
        }

        [HttpDelete("{PostId}")]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Resultado>> ExcluirPostDoBlog(int postId)
        {

            _logger.LogInformation($"[BlogSimplesEF.Web.Posts] Excluir PostId: {postId}");

            try
            {
                if (RetornarUserId() == null)
                    return BadRequest(new Resultado()
                    {
                        Acao = "Criação de Post",
                        Inconsistencias = { "Usuário não autenticado" }
                    });

                var resultado = await _service.ExcluirPostDoBlog(postId);
                if (resultado.Inconsistencias.Count > 0)
                {
                    _logger.LogError(Util.GetJSONResultado(resultado));
                    return BadRequest(resultado);
                }
                else
                    _logger.LogInformation("[BlogSimplesEF.Web.Posts] Exclusão efetuada com sucesso");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[BlogSimplesEF.Web.Posts] Erro na chamada do 'Delete'");

                return BadRequest(new Resultado()
                {
                    Acao = "Criação de Post",
                    Inconsistencias = { "Exceção ao efetuar a deleção do Post" }
                });
            }
        }
        
    }
}
