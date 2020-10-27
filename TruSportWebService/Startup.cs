using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.Owin;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;
using OnTrackWebService.Repository;
//using Owin;

//[assembly: OwinStartup(typeof(OnTrackWebService.Startup))]
namespace OnTrackWebService
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
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();
            //services.AddMvc().AddJsonOptions(options => {
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //});

            services.AddMvc().AddMvcOptions(options =>
            {
                options.EnableEndpointRouting = false;
            });
            services.AddMvc().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            //#if DEBUG
            services.AddDbContext<OnTrackContext>
                (op => op.UseSqlServer(Configuration["ConnectionString:OnTrackDBTest"]));

            ////#else
            //services.AddDbContext<OnTrackContext>
            //    (op => op.UseSqlServer(Configuration["ConnectionString:OnTrackDB"]));

            //#endif

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false
                };
            });

            services.AddScoped<IOnTrackRepository<ContactTrace>, ContactTraceRepository>();
            services.AddScoped<IOnTrackRepository<Coach>, CoachRepository>();
            services.AddScoped<IEmailRepository<string>, EmailRepository>();
            services.AddScoped<INewsRepository<RssFeedItem>, NewsRepository>();
            services.AddScoped<IOnTrackRepository<Field>, FieldRepository>();
            services.AddScoped<IOnTrackRepository<Fixture>, FixtureRepository>();
            services.AddScoped<IOnTrackRepository<Flyer>, FlyerRepository>();
            services.AddScoped<IOnTrackRepository<League>, LeagueRepository>();
            services.AddScoped<IOnTrackRepository<LeagueStat>, LeagueStatRepository>();
            services.AddScoped<IOnTrackRepository<LTable>, LeagueTableRepository>();
            services.AddScoped<IOnTrackRepository<Inventory>, InventoryRepository>();
            services.AddScoped<IOnTrackRepository<FixtureProduct>, FixtureProductRepository>();
            services.AddScoped<IOnTrackRepository<Transfer>, TransferRepository>();
            services.AddScoped<IOnTrackRepository<Match>, MatchRepository>();
            services.AddScoped<IOnTrackRepository<MatchTicket>, MatchTicketRepository>();
            services.AddScoped<IOnTrackRepository<MatchInning>, MatchInningRepository>();
            services.AddScoped<IOnTrackRepository<MatchRoster>, MatchRosterRepository>();
            services.AddScoped<IOnTrackRepository<MatchStat>, MatchStatRepository>();
            services.AddScoped<IOnTrackRepository<MatchType>, MatchTypeRepository>();
            services.AddScoped<IOnTrackRepository<Order>, OrderRepository>();
            services.AddScoped<IOnTrackRepository<Player>, PlayerRepository>();
            services.AddScoped<IOnTrackRepository<Award>, AwardRepository>();
            services.AddScoped<IOnTrackRepository<PlayerSeason>, PlayerSeasonRepository>();
            services.AddScoped<IOnTrackRepository<Team>, TeamRepository>();
            services.AddScoped<IOnTrackRepository<TicketConfiguration>, TicketConfigurationRepository>();
            services.AddScoped<IOnTrackRepository<Role>, RoleRepository>();
            services.AddScoped<ISettingRepository<Setting>, SettingRepository>();
            services.AddScoped<IOnTrackRepository<Season>, SeasonRepository>();
            services.AddScoped<IOnTrackRepository<Sport>, SportRepository>();
            //services.AddScoped<IDisposable, UserRepository>();
            services.AddScoped<IDisposable, AuthenticationRepository>();
            services.AddScoped<IPushNotificationRepository<Push>, PushNotificationRepository>();
            services.AddScoped<IOnTrackRepository<UserType>, UserTypeRepository>();
            //services.AddSingleton<BackgroundWorker>();

            //string domain = $"https://{Configuration["Auth0:Domain"]}/";
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            //}).AddJwtBearer(options =>
            //{
            //    options.Authority = domain;
            //    options.Audience = Configuration["Auth0:ApiIdentifier"];
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("read:players", policy => policy.Requirements.Add(new HasScopeRequirement("read:players", domain)));
            //});

            //services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseHsts();
            //}

            //IAppBuilder apps = new AppBuilder();
            //apps.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
            //{
            //    AllowInsecureHttp = true,

            //    TokenEndpointPath = new PathString("/token"),

            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

            //    Provider = new AuthorizationServerProvider()
            //};

            
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //apps.UseOAuthAuthorizationServer(options);
            //apps.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            ////HttpConfiguration config = new HttpConfiguration();
            ////WebApiConfig.Register(config);

            app.UseStaticFiles();

            app.UseAuthentication();
            
            //app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //public void Configuration(IAppBuilder app)
        //{
        //    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        //    app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

        //    OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
        //    {
        //        AllowInsecureHttp = true,

        //        TokenEndpointPath = new PathString("/token"),

        //        AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

        //        Provider = new AuthorizationServerProvider()
        //    };

        //    app.UseOAuthAuthorizationServer(options);

        //    app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        //    //HttpConfiguration config = new HttpConfiguration();
        //    //ApiConfig.Register(config);
        //}
    }
}
