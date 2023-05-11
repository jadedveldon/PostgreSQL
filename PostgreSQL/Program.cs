using Microsoft.EntityFrameworkCore;
using PostgreSQL.DataEntities.Models;
using PostgreSQL.DataLayer.Implementations;
using PostgreSQL.DataLayer.Interfaces;
using PostgreSQL.Repository.Implementations;
using PostgreSQL.Repository.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency Injection
builder.Services.AddScoped<IPostgreSQLRepository, PostgreSQLRepository>();
builder.Services.AddScoped<IPostgreSQLDataLayer, PostgreSQLDataLayer>();

//Service to connect to db
builder.Services.AddDbContext<TestContext>(options => options.UseNpgsql("Host=demo-sapan.postgres.database.azure.com;Database=postgres;Username=sapan@demo-sapan;Password=Sachit2000"));

//automapper setup
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
