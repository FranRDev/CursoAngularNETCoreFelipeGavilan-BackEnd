using AutoMapper;
using back_end.Filtros;
using back_end.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Text;

namespace back_end {

    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(provider => new MapperConfiguration(config => config.AddProfile(new PerfilesAutoMapper(provider.GetRequiredService<GeometryFactory>()))).CreateMapper());
            services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
            services.AddHttpContextAccessor();
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("CadenaConexionBDD"), sqlServer => sqlServer.UseNetTopologySuite());
            });
            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    builder
                        .WithOrigins(Configuration.GetValue<string>("FrontEndUrl"))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders(new string[] { "Total-Registros" });
                });
            });
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opciones => {
                    opciones.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddControllers(options => {
                options.Filters.Add(typeof(FiltroDeExcepcion));
            });
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Películas API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Películas API v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

    }

}