using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4;
using System.Security.Claims;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddControllers();
            services
                .AddIdentityServer()
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(Config.Users)
                .AddDeveloperSigningCredential();
            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("OnlyWeirdScope", builder =>
                    {
                        builder.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", "WeirdScope");
                    });
                    options.AddPolicy("OnlyTestGroup", builder =>
                    {
                        builder.RequireClaim("groups", "7f8e7276-822c-4678-b5f8-2fc6337ba90b");
                    });
                })
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:6001";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                })
                .AddJwtBearer("aad-2", "Azure AD JWT", options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.Authority = "https://login.microsoftonline.com/common";
                    options.TokenValidationParameters =
                            new TokenValidationParameters { ValidateAudience = false, ValidateLifetime = false, ValidateIssuerSigningKey = false, ValidateTokenReplay = false, ValidateActor = false, ValidateIssuer = false };
                })
                .AddOpenIdConnect("aad-1", "Azure AD", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.Authority = "https://login.microsoftonline.com/common";
                    options.Scope.Add("openid");
                    options.RequireHttpsMetadata = true;
                    options.ResponseType = "code id_token";
                    options.TokenValidationParameters =
                            new TokenValidationParameters { ValidateIssuer = false };
                    options.ClientId = "c8762132-e275-4aaa-bc07-d19bf317a448";
                    options.ClientSecret = "V-C54sc2PM~5sC_J928aA3wjsh5s-MM6.t";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.CallbackPath = "/signin-oidc";
                })
                .AddOpenIdConnect("aad-3", "Azure AD B2C", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.Authority = "https://azsoftwaredev.b2clogin.com/tfp/azsoftwaredev.onmicrosoft.com/B2C_1_signupandlogin/v2.0";
                    options.RequireHttpsMetadata = true;
                    options.Scope.Add("6af9f363-8b4b-4c1d-a648-51065f72aaaa");
                    options.ResponseType = "code id_token token";
                    options.TokenValidationParameters =
                            new TokenValidationParameters { ValidateIssuer = false };
                    options.ClientId = "6af9f363-8b4b-4c1d-a648-51065f72aaaa";
                    options.ClientSecret = "eAk5Z92Fl4fy3-jH9_714QnaI9yIO~12x.";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.CallbackPath = "/signin-oidc-b2c";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseIdentityServer()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
