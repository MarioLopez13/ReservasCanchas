using BackendReservas.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddScoped<PromocionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔽 CORS: permite peticiones desde Angular (localhost:4200)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// 🔽 Usa la política de CORS antes de Authorization
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
