using Infrastructure.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var serverVersion = new MySqlServerVersion(new Version(8,1,0));
#pragma warning disable CS8600
builder.Services.AddDbContext<TiendaContext>(options => {

    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");; 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    
});
#pragma warning restore CS8600

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using(var scope = app.Services.CreateScope()){
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try{
        var context = services.GetRequiredService<TiendaContext>();
        await context.Database.MigrateAsync();
    }

    catch(Exception ex){
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
