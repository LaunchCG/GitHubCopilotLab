using Microsoft.EntityFrameworkCore;
using PriceService;
using PriceService.Database;
using PriceService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IPriceService, PricingService>();
builder.Services.AddDbContext<PriceContext>(optionsBuilder =>
{
string connString = string.Format("Server={0};Database={1};Port=5432;Username={2};Password={3};SSLMode=Require;Trust Server Certificate=true;Include Error Detail=true;",
    GlobalEnv.DBHOST, GlobalEnv.DBNAME, GlobalEnv.DBUSER, GlobalEnv.DBPASSWORD);

    optionsBuilder
        .UseNpgsql(connString)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
        .UseSnakeCaseNamingConvention();
}
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
