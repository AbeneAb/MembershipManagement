using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace HealthData.API;

public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public virtual IServiceProvider ConfigureServices(IServiceCollection services) 
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));

        }) 
           .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Health API",
                Version = "v1"
            });
        });
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });


        services.AddSingleton<IRabbitMQPersistentConnection>(s => {
            var logger = s.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMq:Hostname"],
                DispatchConsumersAsync = true,
                UserName = _configuration["RabbitMq:UserName"],
                Password = _configuration["RabbitMq:Password"],
                Port = int.Parse(_configuration["RabbitMq:Port"])
            };
            var retryCount = 5;
            return new RabbitMQPersistentConnection(factory, logger, retryCount);
        });
        RegsiterEventBus(services);

        services.AddHostedService<HealthDataMockService>();
        var container = new ContainerBuilder();
        container.Populate(services);
        return new AutofacServiceProvider(container.Build());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
    {
        if (env.IsDevelopment())

        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Health API"));
        }
       
        app.UseRouting();
        app.UseCors("CorsPolicy");

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }
    private void RegsiterEventBus(IServiceCollection services) 
    {
        services.AddSingleton<IEventBus, RabbitMQEventBus>(S =>
        {
            var queueName = _configuration["Rabbitmq:SubscriptionClientName"];
            var persistanceConnection = S.GetRequiredService<IRabbitMQPersistentConnection>();
            var logger = S.GetRequiredService<ILogger<RabbitMQEventBus>>();
            var iLifetimeScope = S.GetRequiredService<ILifetimeScope>();
            var subscriptionManager = S.GetRequiredService<IEventBusSubscriptionsManager>();
            var retryCount = 5;
            return new RabbitMQEventBus(persistanceConnection, logger, subscriptionManager, iLifetimeScope, queueName, retryCount);

        });

        services.AddSingleton<IEventBusSubscriptionsManager, EventBusSubscriptionsManager>();
    }

}

