using BlogSimplesEF.Web.Business;
using BlogSimplesEF.Web.Data.AspNetId;
using BlogSimplesEF.Web.Data.Blog;
using BlogSimplesEF.Web.Hub;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BlogSimplesEF.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            //Oppção se for usar o banco de dados
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<BlogContext>(options => options.UseSqlServer(connectionString));

            //Opção para usar o banco em memória
            //services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("DBMemory"));
            //services.AddDbContext<BlogContext>(options => options.UseInMemoryDatabase("DBMemory"));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages();

            //Registro dos serviços
            builder.Services.AddTransient<PostService>();

            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "BlogSimples",
                        Version = "v1",
                        Description = "API REST para manipulação de posts do blog",
                        Contact = new OpenApiContact
                        {
                            Name = "Daniel Ralfes",
                            Url = new Uri("https://github.com/danielralfes")
                        }
                    });
            });

            builder.Services.AddMvc(
                                    //options => { options.Filters.Add(new UnhandledExceptionLoggerFilter()); }
                                    )
                    .AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; });

            builder.Services.AddResponseCompression();
            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "[Ralfes v1] Api de Post do Blog");
            });

            app.UseAuthorization();
            app.MapRazorPages();

            // UseCors must be called before MapHub.
            //app.UseCors();
            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                //Usado para definir quais rotas podem acessar) - Deixei comentando para efeito de testes
                //.WithOrigins(
                //Está liberado tudo acima, mas em um cenário real de prdoução avaliar qual deverá ser liberado
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHub<PostsHub>("/PostsHub");
            });

            app.Run();
        }
    }
}
