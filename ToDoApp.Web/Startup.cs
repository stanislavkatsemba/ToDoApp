using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApp.Data;
using ToDoApp.Data.Repositories;
using ToDoApp.Domain.Shared.Common.Events;
using ToDoApp.Domain.ToDoItems;
using ToDoApp.Domain.ToDoItems.Events;
using ToDoApp.Domain.ToDoItems.ReadModel;
using ToDoApp.Domain.Users;
using ToDoApp.Web.Common.Authentication;
using ToDoApp.Web.Hubs.ToDoItems;

namespace ToDoApp.Web
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
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            //Database
            services.AddDbContext<ToDoAppContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            //Repositories
            services.AddScoped<IToDoItemRepository, ToDoItemSqlRepository>();
            services.AddScoped<IToDoItemReadRepository, ToDoItemReadSqlRepository>();
            services.AddScoped<IUserRepository, UserSqlRepository>();

            //Authentication
            services.AddScoped<JwtService>();
            services.AddAuthentication("Authentication")
                .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Authentication", null);

            //Application services
            services.AddScoped<UserService>();
            services.AddScoped<ToDoItemService>();

            //Domain events
            services.AddSingleton<IDomainEventPublisher, InMemoryEventPublisher>();
            services.AddScoped<IDomainEventHandler<ToDoItemChangedEvent>, ToDoItemEventHandler>();
            services.AddScoped<IDomainEventHandler<ToDoItemRemovedEvent>, ToDoItemEventHandler>();

            //SignalR
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ToDoAppContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            dbContext.Database.EnsureCreated();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ToDoItemHub>("/hubs/toDoItems");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private class InMemoryEventPublisher : IDomainEventPublisher
        {
            private readonly IServiceProvider _serviceProvider;

            public InMemoryEventPublisher(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public async Task Publish<T>(T domainEvent) where T : IDomainEvent
            {
                using var scope = _serviceProvider.CreateScope();
                var handlers = scope.ServiceProvider.GetServices<IDomainEventHandler<T>>();
                foreach (var handler in handlers)
                {
                    await handler.Handle(domainEvent);
                }
            }
        }
    }
}
