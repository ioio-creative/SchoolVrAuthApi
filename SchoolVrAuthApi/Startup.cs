using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolVrAuthApi.Models;


namespace SchoolVrAuthApi
{
    public class Startup
    {
        private const string DefaultConnectionStringKey = "SchoolVrAuthConnectionString";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connectionString = Configuration.GetValue<string>(DefaultConnectionStringKey);
            services.AddDbContext<AuthContext>(opt => opt.UseLazyLoadingProxies().UseSqlServer(connectionString));

            // https://stackoverflow.com/questions/51116403/how-to-get-client-ip-address-in-asp-net-core-2-1/51245326
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                
                app.UseExceptionHandler("/api/error");
            }
            else
            {
                app.UseHsts();

                app.UseExceptionHandler("/api/error");                
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
