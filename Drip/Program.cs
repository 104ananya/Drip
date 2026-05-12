using Dapper;
using Drip.Data;
using Drip.Services;
using Microsoft.EntityFrameworkCore;

// Tell Dapper how to handle DateOnly (PostgreSQL DATE type)
SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IExpenseService, ExpenseService>(); // one instance per HTTP request

// This tells .NET's Dependency Injection: "When any class asks for IExpenseService, give it an ExpenseService instance." The Controller never creates the service itself — DI hands it over automatically.


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
