using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GreenShop.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace GreenShop
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
            services.AddScoped<SmtpClient>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>().GetSection("Email");
                return new SmtpClient()
                {
                    Host = config.GetValue<string>("Host"),
                    Port = config.GetValue<int>("Port"),
                    Credentials = new NetworkCredential()
                    {
                        UserName = config.GetValue<string>("Username"),
                        Password = config.GetValue<string>("Password")

                    }

                };
            });
            services.AddDbContext<ShopContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DevSql")));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
