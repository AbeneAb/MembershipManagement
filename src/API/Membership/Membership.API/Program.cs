



var configuration = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);
Log.Logger = CreateSerilogLogger(configuration);

// Add services to the container.
try
{
    Log.Information("Configuring web host ...");
    var host = CreateHostBuilder(args).Build();
    Log.Information("Applying migrations ...");
    host?.MigrateDatabase<MembershipContext>((context, services) =>
    {
        var env = services.GetService<IWebHostEnvironment>();
        var logger = services.GetService<ILogger<MembershipContextSeed>>();
        MembershipContextSeed.SeedAsync(context, logger).Wait();
    });
    Log.Information("Starting host ...");
    host.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unexpected error");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder().
        SetBasePath(Directory.GetCurrentDirectory()).
        AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
        AddEnvironmentVariables();
    return builder.Build();
}
Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}
