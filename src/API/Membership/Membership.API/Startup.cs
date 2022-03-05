using Autofac;
using Autofac.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Membership.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructureServices(Configuration);
            services.AddControllers();
            services.AddControllers(option =>
            {
                option.Filters.Add(typeof(HttpGlobalExceptionFilter));
                option.Filters.Add(typeof(ValidateModelStateFilter));
            })
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true)
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Membership API",
                    Version = "v1"
                });
            });
            services.AddSingleton<IRabbitMQPersistentConnection>(s => {
                var logger = s.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["RabbitMq:Hostname"],
                    DispatchConsumersAsync = true,
                    UserName = Configuration["RabbitMq:UserName"],
                    Password = Configuration["RabbitMq:Password"],
                    Port = int.Parse(Configuration["RabbitMq:Port"])
                };
                var retryCount = 5;
                return new RabbitMQPersistentConnection(factory, logger, retryCount);
            });
            RegisterEventBus(services);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            var container = new ContainerBuilder();
            container.Populate(services);

            //container.RegisterModule(new ApplicationModule(Configuration["ConnectionString"]));

            return new AutofacServiceProvider(container.Build());
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())

            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Membership API v1"));
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ConfigureEventBus(app);
        }

        private void RegisterEventBus(IServiceCollection services) 
        {
            services.AddSingleton<IEventBus,RabbitMQEventBus>(sp =>
            {
                var subscriptionClientName = Configuration["RabbitMq:SubscriptionClientName"];
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var ilifeTimeScope = sp.GetRequiredService<ILifetimeScope>();
                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new RabbitMQEventBus(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager,ilifeTimeScope, subscriptionClientName, retryCount);
            });
            services.AddSingleton<IEventBusSubscriptionsManager, EventBusSubscriptionsManager>();
            services.AddTransient<IIntegrationEventHandler<NewPulseDataPosted>, NewPulseDataHandler>();

        }
        private void ConfigureEventBus(IApplicationBuilder app) 
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<NewPulseDataPosted, IIntegrationEventHandler<NewPulseDataPosted>>();


        }
    }
}
