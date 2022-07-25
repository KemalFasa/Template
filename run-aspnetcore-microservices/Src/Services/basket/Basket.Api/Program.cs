using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Basket.Api.Repositories; 
using Basket.Api.GrpcServices;
using Discount.Grpc.Protos;
// using Microsoft.Exentensions.DepedencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =  builder.Configuration.GetValue<string>("cacheSettings:ConnectionString");  
    // builder.Configuration["cacheSettings"];
});


// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration =  "Localhost:6379";
// });

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
 // Grpc Configuration
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (o => o.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl"))); 
builder.Services.AddScoped<DiscountGrpcService>();


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
