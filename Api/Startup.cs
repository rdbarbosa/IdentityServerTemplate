using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace Api
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
            services.AddMvcCore(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
                config.Filters.Add(new AuthorizeFilter(policy)); //global authorize filter
            })
                .AddAuthorization()
                .AddApiExplorer();
                //.AddJsonFormatters();

            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = "http://localhost:5000";
            //        options.RequireHttpsMetadata = false;
            //        options.ApiName = "api1";
            //    });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    // auth server base endpoint (will use to search for disco doc)
                    options.Authority = "http://localhost:5000";
                    options.ApiName = "api1_swagger"; // required audience of access tokens
                    options.RequireHttpsMetadata = false; // dev only!
                });

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        //.WithOrigins("http://localhost:5100")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api1", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows 
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("http://localhost:5000/connect/authorize"),
                            Scopes = new Dictionary<string, string> {
                                { "api1", "api2" }
                            }
                        }
                    },
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Oauth2"
                            },
                            Scheme = "oauth2",
                            Name = "Oauth2",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                //c.OperationFilter<AuthorizeCheckOperationFilter>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.OAuthClientId("api1_swagger");
                c.OAuthAppName("My API Swagger");
            });

            app.UseRouting();
            app.UseCors("default");
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
