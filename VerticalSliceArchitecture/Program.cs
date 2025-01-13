using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture;
using VerticalSliceArchitecture.DataBase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var asssembly = typeof(Program).Assembly;

builder.Services.AddCarter();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(asssembly));
builder.Services.AddValidatorsFromAssembly(asssembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.MapCarter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
