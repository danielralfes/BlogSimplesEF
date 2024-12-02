using System;
using BlogSimplesEF.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlogSimplesEF.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors();

            services.AddDbContext<BlogContext>(options =>
                                                    options.UseInMemoryDatabase("DBMemory")); //UseSQlServer(connectionString)


            services.AddMvc(options => { options.Filters.Add(new UnhandledExceptionLoggerFilter()); })
                    .AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; });

            services.AddResponseCompression();

            services.AddScoped<BlogContext>();
            //services.AddTransient<EventosService>();
            //services.AddTransient<SalasService>();

            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "BlogSimplesEF.Api",
                        Version = "v1",
                        Description = "API REST para manipulação do blog",
                        Contact = new OpenApiContact
                        {
                            Name = "Daniel Ralfes",
                            Url = new Uri("https://github.com/danielralfes")
                        }
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "[Ralfes v1] Api do BlogSimplesEF");
            });

            app.UseHttpsRedirection();

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                //Usado para definir quais rotas podem acessar) - Deixei comentando para efeito de testes
                //.WithOrigins(
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
