
using Microsoft.EntityFrameworkCore;
using BlogSimplesEF.Api.Models.Entities;

namespace BlogSimplesEF.Api.Infrastructure.Data
{
    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        public BlogContext(DbContextOptions<BlogContext> dbContextOptions)
            : base(dbContextOptions)
        {
            SeedData();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // registra todos os mapeamentos
            var assembly = typeof(BlogContext).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData()
        {
            //Obs:
            //Seed apenas para testes em memória
            //Em ambiente normal não usar.
            //Seed usado apenas para inicializar os dados em quando é usado codefirst e apenas uma vez

            if (Categories.Count() == 0)
            {
                //Inicialização do banco de dados para realizar testes
                //Post
                //Comment
                //Category

                //Eventos.Add(new Evento { SalaId = 3, EventoId = Guid.NewGuid(), DataInicial = DateTime.Now.AddDays(10), DataFinal = DateTime.Now.AddDays(11), Responsavel = "José Teste" });

                SaveChanges();
            }
        }
    }
}
