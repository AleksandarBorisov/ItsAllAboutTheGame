using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Services.Data;
using ItsAllAboutTheGame.Data.Models;
using Microsoft.AspNetCore.Mvc;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.External.Contracts;
using ItsAllAboutTheGame.Services.External;
using ItsAllAboutTheGame.Services.Data.ForeignExchangeApiService;
using ItsAllAboutTheGame.Services.Data.Contracts.ForeignExchangeApiService;
using ItsAllAboutTheGame.Extensions;
using ItsAllAboutTheGame.Services;
using ItsAllAboutTheGame.Services.Data.Services;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.Services.Game.Contracts.GameOne;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Services.Core.Game;

namespace ItsAllAboutTheGame
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
            services.AddDbContext<ItsAllAboutTheGameDbContext>(options =>

                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            this.RegisterAuthentication(services);
            this.RegisterServices(services);
            this.RegisterMainComponents(services);
        }

        private void RegisterMainComponents(IServiceCollection services)
        {
            //services.AddMvc();

            services.AddHttpClient();

            services.AddResponseCaching();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddMvc(options =>
            //{
            //    options.CacheProfiles.Add("Default",
            //    new CacheProfile()
            //    {
            //        Duration = 3600
            //    });
            //});
            services.AddMemoryCache();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IForeignExchangeService, ForeignExchangeService>();
            services.AddScoped<IJsonDeserializer, JsonDeserializer>();
            services.AddScoped<IForeignExchangeApiCaller, ForeignExchangeApiCaller>();
            services.AddSingleton<IGame, Game>();
            services.AddSingleton<IGameRandomizer, GameRandomizer>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        }

        private void RegisterAuthentication(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ItsAllAboutTheGameDbContext>()
                .AddDefaultTokenProviders();
            //services.AddIdentity<User, IdentityRole<string>>()
            //        .AddUserStore<UserStore<User, IdentityRole<string>, ItsAllAboutTheGameDbContext, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityUserToken<int>, IdentityRoleClaim<int>>>()
            //        .AddRoleStore<RoleStore<IdentityRole<string>, ItsAllAboutTheGameDbContext, int, UserRole, IdentityRoleClaim<int>>>()
            //        .AddDefaultTokenProviders();

            services.AddAuthentication();
            services.AddAuthorization();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            UserManager<User> userManager,
            ItsAllAboutTheGameDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Index");
            }

            app.SeedAdmins(userManager, context);

            app.UseNotFoundExceptionHandler();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseResponseCaching();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "notfound",
                    template: "404",
                    defaults: new { controller = "Error", action = "PageNotFound" });

                routes.MapRoute(
                    name: "ForeignApiError",
                    template: "ForeignApiError",
                    defaults: new { controller = "Error", action = "ForeignApiError" });

                routes.MapRoute(
                    name: "Administration",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
