using IdentityManager2.AspNetIdentity;
using IdentityManager2.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace IdentityManager
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<IdentityDbContext>(opt => opt.UseInMemoryDatabase("test"));
            services.AddDbContext<IdentityDbContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                options.UseSqlServer(@"Server=(localdb)\\mssqllocaldb; Database=AspIdentity; Trusted_Connection=True; MultipleActiveResultSets=true"));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityManager()
                .AddIdentityMangerService<AspNetCoreIdentityManagerService<IdentityUser, string, IdentityRole, string>>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseIdentityManager();
        }
    }
}
