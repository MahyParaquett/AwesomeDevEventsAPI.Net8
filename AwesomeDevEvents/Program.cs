using AwesomeDevEventsAPI.Mappers;
using AwesomeDevEventsAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AwesomeDevEvents.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Mahyara Paraquett",
            Email = "mahyaraparaquett@gmail.com",
            
        }
    });

    var xmlFile = "AwesomeDevEventsAPI.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
//Conexão com banco em memoria
//builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseInMemoryDatabase("DevEventsDb"));

//Conexão com um banco local
var connectionString = builder.Configuration.GetConnectionString("DevEvantsCs");
builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseSqlServer(connectionString));

//Contiguração pra ele achar todos os mappers
builder.Services.AddAutoMapper(typeof(DevEventProfile).Assembly);

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
