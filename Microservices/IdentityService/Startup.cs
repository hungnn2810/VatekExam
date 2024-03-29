﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using EntityFramework.Identity;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityModel.Client;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Stores;
using IdentityService.Commons.Constants;
using IdentityService.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace IdentityService
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

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Identity service"
                });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Identity.Service.xml"));
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });

            services
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(AppSettingConstants.ConnectionStrings.IdentityDb,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(AppSettingConstants.ConnectionStrings.IdentityDb,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddConfigurationStoreCache()
                .AddClientStoreCache<ClientStore>()
                .AddResourceStoreCache<ResourceStore>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();

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

            services
                .Configure<TokenClientOptions>(options =>
                {
                    options.Address = AppSettingConstants.IdentityServer.TokenEndpoint;
                    options.ClientId = AppSettingConstants.IdentityServer.ClientId;
                    options.ClientSecret = AppSettingConstants.IdentityServer.ClientSecret;
                })
                .AddTransient(sp => sp.GetRequiredService<IOptions<TokenClientOptions>>().Value)
                .AddHttpClient<TokenClient>();

            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(AppSettingConstants.ConnectionStrings.IdentityDb).EnableSensitiveDataLogging();
            });

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // this will do the initial DB population
            InitializeDatabase(app);

            // ref: https://github.com/IdentityServer/IdentityServer4/issues/1331#issuecomment-317049214
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };
            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardOptions);

            app.UseIdentityServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity service");
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMvc();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.ApiResources)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}

