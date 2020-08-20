using System.IdentityModel.Tokens.Jwt;
using IdentityManager2.AspNetIdentity;
using IdentityManager2.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityManager
{
    public class StartupWithIdentityServer
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                options.UseSqlServer(@"Server=(localdb)\\mssqllocaldb; Database=AspIdentity; Trusted_Connection=True; MultipleActiveResultSets=true"));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityManager(opt =>
                    opt.SecurityConfiguration =
                        new SecurityConfiguration
                        {
                            HostAuthenticationType = "cookie",
                            HostChallengeType = "oidc"
                        })
                .AddIdentityMangerService<AspNetCoreIdentityManagerService<IdentityUser, string, IdentityRole, string>>();

            // To make role claim type == "role"
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication()
                .AddCookie("cookie")
                .AddOpenIdConnect("oidc", opt =>
                {
                    opt.Authority = "http://localhost:5000";
                    opt.ClientId = "identitymanager2";
                    //opt.ResponseType = "id_token token";

                    // default: openid & profile
                    opt.Scope.Add("roles");

                    opt.RequireHttpsMetadata = false; // dev only
                    opt.SignInScheme = "cookie";
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseIdentityManager();
        }
    }
}
