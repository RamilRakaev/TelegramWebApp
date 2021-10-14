using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegramBotService;
using TelegramBotBusiness;
using GoogleCalendarService;
using GoogleCalendarBusiness;
using Infrastructure.Repositories;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Infrastructure.CQRS;
using TelegramWebApp.Pages.Account;
using Domain.Interfaces;

namespace TelegramWebApp
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
            services.AddDbContext<DataContext>(op => op.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                   op => op.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));
            services.AddIdentity<ApplicationUser, ApplicationUserRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

            services.AddRazorPages();
            services.AddHostedService<MigrationManager>();
            services.Configure<TelegramOptions>(Configuration.GetSection("TelegramOptions"));
            services.AddTransient<IRepository<TelegramOptions>, TelegramOptionsRepository>();
            services.AddTransient<IRepository<TelegramUser>, TelegramUserRepository>();

            services.AddTransient<AbstractTelegramHandlers, Handlers>();
            services.AddTransient<TelegramBot, TelegramBot>();
            services.AddTransient<ITelegramConfiguration, HandlerConfiguration>();

            services.AddTransient<UserProperties, UserProperties>();

            services.Configure<GoogleCalendarOptions>(Configuration.GetSection("GoogleCalendarOptions"));
            services.AddTransient<IGoogleCalendar, GoogleCalendar>();
            services.AddMediatR(MethodsAssembly.GetAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
