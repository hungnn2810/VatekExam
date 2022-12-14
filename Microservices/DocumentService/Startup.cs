using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using DocumentService.Commons.Constants;
using DocumentService.Commons.Enums;
using DocumentService.Helpers;
using DocumentService.Helpers.Implements;
using DocumentService.Services;
using DocumentService.Services.Hubs;
using DocumentService.Services.Implements;
using EntityFramework.Document;
using EntityFramework.Identity;
using IdentityModel.AspNetCore.OAuth2Introspection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DocumentService
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
            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Document service"
                });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Document.Service.xml"));
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });

            services
                .AddAuthentication(AppSettingConstants.IdentityServer.DefaultScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = AppSettingConstants.IdentityServer.Authority;
                    options.ApiName = AppSettingConstants.IdentityServer.ApiName;
                    options.ApiSecret = AppSettingConstants.IdentityServer.ApiSecret;
                    options.RequireHttpsMetadata = false;
                    options.TokenRetriever = new Func<HttpRequest, string>(req =>
                    {
                        var fromHeader = TokenRetrieval.FromAuthorizationHeader();
                        var fromQuery = TokenRetrieval.FromQueryString();
                        return fromHeader(req) ?? fromQuery(req);
                    });
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(OAuthConstants.ClaimTypes.UserType,
                        UserTypeEnum.Admin.ToString("d"));
                });

                options.AddPolicy("UserOnly", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(OAuthConstants.ClaimTypes.UserType,
                        UserTypeEnum.User.ToString("d"));
                });
            });

            services.AddDbContext<DocumentDbContext>(options =>
            {
                options.UseSqlServer(AppSettingConstants.ConnectionStrings.DocumentDb);
            });

            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(AppSettingConstants.ConnectionStrings.IdentityDb);
            });

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services
                .AddHttpContextAccessor();

            // Singleton services
            services
                .AddSingleton<IS3Service, S3Service>();

            // Scoped services
            services
                .AddScoped<DocumentHub>()
                .AddScoped<ICurrentHttpContext, CurrentHttpContext>()
                .AddScoped<INotificationService, NotificationService>();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document service");
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DocumentHub>("/notifyhub/document");
            });

            app.UseMvc();
        }
    }
}

