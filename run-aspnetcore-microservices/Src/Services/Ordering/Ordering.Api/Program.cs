using MediatR;
using Ordering.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

              

 builder.Services.AddApplicationServices();
 builder.Services.AddInfrastructureServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(Program));
//  builder.Services.AddDbContext<OrderContext>();
 builder.Services.AddAutoMapper(typeof(Program));

 builder.Services.AddControllers();
var app = builder.Build();
//   app.MigrateDatabase<OrderContext>((context, services) =>
//                     {
//                         var logger = services.GetService<ILogger<OrderContextSeed>>();
//                         OrderContextSeed
//                             .SeedAsync(context, logger)
//                             .Wait();
//                     })
//                 .Run();


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
