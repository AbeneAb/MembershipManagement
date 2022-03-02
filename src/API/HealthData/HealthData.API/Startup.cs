namespace HealthData.API;

public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services) 
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

        RegsiterEventBus(services);

        services.AddSingleton<IRabbitMQPersistentConnection>(s => {
            var logger = s.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["Rabbitmq:Hostname"],
                DispatchConsumersAsync = true,
                UserName = _configuration["Rabbitmq:UserName"],
                Password = _configuration["Rabbitmq:Password"],
                Port = int.Parse(_configuration["Port"])
            };
            var retryCount = 5;
            return new RabbitMQPersistentConnection(factory, logger, retryCount);
        });
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
            var queueName = _configuration["Rabbitmq:QueueName"];
            var persistanceConnection = S.GetRequiredService<IRabbitMQPersistentConnection>();
            var logger = S.GetRequiredService<ILogger<RabbitMQEventBus>>();
            var subscriptionManager = S.GetRequiredService<IEventBusSubscriptionsManager>();
            var retryCount = 5;
            return new RabbitMQEventBus(persistanceConnection, logger, subscriptionManager, queueName, retryCount);

        });

        services.AddSingleton<IEventBusSubscriptionsManager, EventBusSubscriptionsManager>();
    }
    private void ConfigureEventBus(IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    }

}

