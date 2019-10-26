using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booksy.Models;
using Booksy.Services;
using Booksy.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IdentityRole = Microsoft.AspNetCore.Identity.MongoDB.IdentityRole;
using AutoMapper;
using Booksy.Mapper;
using Booksy.Data;

namespace Booksy
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

            services.AddIdentityWithMongoStoresUsingCustomTypes<ApplicationUser, IdentityRole>(Configuration.GetConnectionString("BooksyDb"))
                .AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                .Build());
            });
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(options =>
          {
              options.IncludeErrorDetails = true;
              //options.RequireHttpsMetadata = false;
              options.SaveToken = true;
              options.ClaimsIssuer = Configuration["Jwt:JwtIssuer"];
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = false,
                  ValidateIssuerSigningKey = true,
                  ValidateLifetime = true,
                  ValidateAudience = false,
                  ValidIssuer = Configuration["Jwt:JwtIssuer"],
                  //he audience "aud" claim in a JWT is meant to refer to the Resource Servers that should accept the token
                  ValidAudience = Configuration["Jwt:JwtAudience"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:JwtKey"])),
                  ClockSkew = TimeSpan.Zero // remove delay of token when expire
              };
          });
            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredLength = 3;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.SignIn.RequireConfirmedPhoneNumber = true;
            });

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddScoped<MongoDatabase>();
            services.Configure<SMSoptions>(Configuration);
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UsersProfile>();

            }, AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.EndsWith("Mapper")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeService(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want 
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
        void InitializeService(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var mongoDB = serviceProvider.GetService<MongoDatabase>();
                InitData.InitializeData(mongoDB);
            }

        }
    }
}
