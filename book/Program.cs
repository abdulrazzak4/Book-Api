using book.Data;
using book.Data.Models;
using book.Data.Services;
using book.Exceptions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Log = Serilog.Log;

var builder = WebApplication.CreateBuilder(args);
    try
    {
        // Use Serilog
        var configurationserilog = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        builder.Host.UseSerilog((hostContext, services, configuration) =>
        {
            configuration
                    .ReadFrom.Configuration(configurationserilog);
        });
    }
    finally
    {
        Log.CloseAndFlush();
    }
//DbContext configurations 
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseNameConnString"));
});
// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
//Services configuration
builder.Services.AddLogging();
builder.Services.AddTransient<BooksService>();
builder.Services.AddTransient<AuthorsService>();
builder.Services.AddTransient<PublishersService>();
builder.Services.AddTransient<LogsService>();

builder.Services.AddControllers();
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
//Exeption Handler
//app.ConfigureBuldInExceptionHandler();
app.MapControllers();
//Sead database
//AppDbInitializer.Seed(app);
app.Run();