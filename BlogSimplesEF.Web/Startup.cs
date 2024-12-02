//using BlogSimplesEF.Web.Data.AspNetId;
//using BlogSimplesEF.Web.Data.Blog;
//using BlogSimplesEF.Web.Business;
//using BlogSimplesEF.Web.Hub;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.OpenApi.Models;
//using Microsoft.AspNetCore.Hosting;


//namespace BlogSimplesEF.Web
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddControllersWithViews();  
//            services.AddControllers();
//            services.AddCors();

//            // Add services to the container.
//            var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//            //Oppção se for usar o banco de dados
//            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
//            services.AddDbContext<BlogContext>(options => options.UseSqlServer(connectionString));

//            //Opção para usar o banco em memória
//            //services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("DBMemory"));
//            //services.AddDbContext<BlogContext>(options => options.UseInMemoryDatabase("DBMemory"));

//            services.AddDatabaseDeveloperPageExceptionFilter();

//            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//                    .AddEntityFrameworkStores<ApplicationDbContext>();

//            services.AddRazorPages();

//            //Registro dos serviços
//            services.AddTransient<PostService>();

//            services.AddSwaggerGen(c => {

//                c.SwaggerDoc("v1",
//                    new OpenApiInfo
//                    {
//                        Title       = "BlogSimples",
//                        Version     = "v1",
//                        Description = "API REST para manipulação de posts do blog",
//                        Contact     = new OpenApiContact
//                        {
//                            Name = "Daniel Ralfes",
//                            Url  = new Uri("https://github.com/danielralfes")
//                        }
//                    });
//            });


//            services
//                    .AddMvc(
//                     //options => { options.Filters.Add(new UnhandledExceptionLoggerFilter()); }
//                     )
//                    .AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; });

//            services.AddResponseCompression();
//            services.AddSignalR();
//        }
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseMigrationsEndPoint();
//                app.UseDeveloperExceptionPage();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseSwagger();
//            app.UseSwaggerUI(c =>
//            {
//                c.SwaggerEndpoint("/swagger/v1/swagger.json", "[Ralfes v1] Api de Post do Blog");
//            });
//            app.UseAuthorization();

//            // UseCors must be called before MapHub.
//            //app.UseCors();
//            // Shows UseCors with CorsPolicyBuilder.
//            app.UseCors(builder =>
//            {
//                builder.AllowAnyOrigin()
//                       .AllowAnyMethod()
//                       .AllowAnyHeader();
//                //Usado para definir quais rotas podem acessar) - Deixei comentando para efeito de testes
//                //.WithOrigins(
//                //Está liberado tudo acima, mas em um cenário real de prdoução avaliar qual deverá ser liberado
//            });


//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapRazorPages(); 
//                endpoints.MapHub<PostsHub>("/PostsHub");
//                endpoints.MapControllers();
//            });

//        }
//    }
//}
