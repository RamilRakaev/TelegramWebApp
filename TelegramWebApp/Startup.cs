using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegramBotService;
using TelegramBotBusiness;
using TelegramBotBusiness.Services;
using GoogleCalendarService;
using GoogleCalendarBusiness;
using Infrastructure.Repositories;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Infrastructure.CQRS;
using TelegramWebApp.Pages.Account;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Domain.ModelValidators;

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
            services.AddIdentity<ApplicationUser, ApplicationUserRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddTransient<IRepository<Option>, OptionRepository>();
            services.AddTransient<IRepository<TelegramUser>, TelegramUserRepository>();
            services.AddHostedService<MigrationService>();
            services.AddHostedService<UserService>();

            services.AddTransient<IGoogleCalendar, GoogleCalendar>();
            services.AddTransient<ITelegramHandlerConfiguration, HandlerConfiguration>();
            services.AddTransient<AbstractTelegramHandlers, TelegramHandlers>();

            //services.Configure<TelegramOptions>(Configuration.GetSection("TelegramOptions"));
            services.AddTransient<AbstractTelegramBot, TelegramBot>();

            //Настройка телеграм бота для перехвата запросов
            //services.AddHostedService<ConfigureWebhookService>();

            services.AddTransient<UserProperties, UserProperties>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddValidatorsFromAssembly(MethodsAssembly.GetAssembly());
            services.AddMediatR(MethodsAssembly.GetAssembly());

            services.AddTransient<IValidator<WebAppOptions>, CalendarOptionsValidator>();
            services.AddTransient<IValidator<ApplicationUser>, ApplicationUserValidator>();
            services.AddTransient<IValidator<TelegramUser>, TelegramUserValidator>();

            services.AddRazorPages().AddFluentValidation();
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

            app.UseAuthentication();    // подключение аутентификации
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
