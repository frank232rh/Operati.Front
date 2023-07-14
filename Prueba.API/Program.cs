using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prueba.Data;
using Prueba.Data.BL.Interfaces;
using Prueba.Data.BL.Services;
using Prueba.Data.Helpers;

var builder = WebApplication.CreateBuilder(args);

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(o => o.AddPolicy(MyAllowSpecificOrigins, builder =>
{
    //Se llaman a todas las paginas web que va a usar la API
    builder.WithOrigins("https://localhost:44494/")
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
}));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Interfaces and Services
builder.Services.AddTransient<IUser, UserService>();

//Inyeccion del DBContext
var builder2 = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
IConfiguration configuration = builder2.Build();
ContextConfiguration.ConexionString = configuration.GetValue<string>("ConnectionStrings:PruebaDBContext");

builder.Services.AddDbContext<PruebaDbContext>(options => options.UseSqlServer(ContextConfiguration.ConexionString));
builder.Services.AddDbContext<PruebaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PruebaDBContext"));
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
