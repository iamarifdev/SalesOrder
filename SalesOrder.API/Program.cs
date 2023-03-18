using Microsoft.EntityFrameworkCore;
using SalesOrder.API.Helpers;
using SalesOrder.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterAutoMappingProfiles();
builder.Services.RegisterDependencies();

if (builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlite(
            builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)),
            x => x.MigrationsAssembly("SalesOrder.Data")
        )
    );
}

var app = builder.Build();

MigrateDatabase();

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

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

void MigrateDatabase()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var dbContextOptions = serviceScope.ServiceProvider.GetService<DbContextOptions<ApplicationDbContext>>();
    if (dbContextOptions == null || !dbContextOptions.IsRelational()) return;
    
    using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    context.Database.Migrate();
}