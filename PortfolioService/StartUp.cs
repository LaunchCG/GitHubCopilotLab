using System.Reflection;
using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SALearning.Services;
using SALearning.Database;

[assembly: FunctionsStartup(typeof(SALearning.Startup))]

namespace SALearning
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSwaggerGen();
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IPortfolioService, PortfolioService>();
            builder.Services.AddScoped<IPricingService, PricingService>();

            builder.Services.AddDbContext<PortfolioContext>(optionsBuilder => 
            {
                string connString = string.Format("Server={0};Database={1};Port=5432;Username={2};Password={3};SSLMode=Require;Trust Server Certificate=true",
                    GlobalEnv.DBHOST, GlobalEnv.DBNAME, GlobalEnv.DBUSER, GlobalEnv.DBPASSWORD);

                optionsBuilder
                    // Feature:Logging
                    // .UseLoggerFactory(loggerFactory)
                    .UseNpgsql(connString)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .UseSnakeCaseNamingConvention();
            });
            builder.Services.AddAutoMapper(Assembly.GetAssembly(this.GetType()));
            builder.Services.AddEndpointsApiExplorer();
        }
    }
}