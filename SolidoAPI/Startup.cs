using Core;
using Core.Entity;
using Core.Helpers;
using Core.Interface;
using Core.Service;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Renci.SshNet;
using System;
using System.IO;

namespace SolidoAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<Infrastructure.Data.SolidoDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("sqlDb")));
            services.AddIdentity<Admin, IdentityRole>().AddEntityFrameworkStores<Infrastructure.Data.SolidoDbContext>().AddDefaultTokenProviders();
            services.Configure<JwtOptions>(Configuration.GetSection(nameof(JwtOptions)));
            services.AddUnitOfWork();
            services.AddScoped<IRoofRepository, RoofRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IMainRepository, MainRepository>();
            services.AddTransient<IMailService, MailService>();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddAutoMapper();
            services.AddJwtAuthentication(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Image")),
                RequestPath = "/Image"
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
