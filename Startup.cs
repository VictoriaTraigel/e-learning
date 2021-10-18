using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LeshBrain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace LeshBrain
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ContextDB>(options=> options.UseMySql(
                "server=localhost;user=root;password=root;database=leshbrain;",
                new MySqlServerVersion(new Version(8, 0, 21))
            ));
            services.AddIdentity<UserEntity, IdentityRole<int>>(opts=>
            {
                opts.Password.RequireDigit = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ContextDB>();

            services.AddControllersWithViews();

            //services.Configure<FormOptions>(options =>
            //{
            //    options.MultipartBodyLengthLimit = 100000000;
            //});
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("isAdmin",
            //         policy => policy.RequireRole("super admin"));
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
