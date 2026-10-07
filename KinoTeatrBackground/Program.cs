using Microsoft.EntityFrameworkCore;
using KinoTeatrBackground.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.WebHost.UseUrls("http://0.0.0");
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();   // Разрешаем запросы откуда угодно
        policy.AllowAnyMethod();   // Разрешаем любые методы (GET, POST...)
        policy.AllowAnyHeader();  // Разрешаем любые заголовки
    });
});

var app = builder.Build();
app.UseCors();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
