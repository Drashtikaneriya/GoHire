using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using FluentValidation;
using YourProjectName.Models;
using RecruitmentsystemAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Role>();
builder.Services.AddValidatorsFromAssemblyContaining<User>();
builder.Services.AddValidatorsFromAssemblyContaining<JobPosition>();
builder.Services.AddValidatorsFromAssemblyContaining<Candidate>();
builder.Services.AddValidatorsFromAssemblyContaining<Application>();
builder.Services.AddValidatorsFromAssemblyContaining<Interview>();
builder.Services.AddValidatorsFromAssemblyContaining<Company>();


// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

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
