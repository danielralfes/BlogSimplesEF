using BlogSimplesEF.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace BlogSimplesEF.Web.Data.Blog
{
    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public BlogContext(DbContextOptions<BlogContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // registra todos os mapeamentos
            var assembly = typeof(BlogContext).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }
    }

}
