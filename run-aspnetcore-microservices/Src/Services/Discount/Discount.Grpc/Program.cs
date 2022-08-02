using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Runtime.InteropServices;
using Discount.Grpc.Services;
using Discount.Grpc.Repositories.Interfaces;
using Discount.Grpc.Repositories;

var builder = WebApplication.CreateBuilder(args);
const int Port = 5003;

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.WebHost.ConfigureKestrel(options =>
{
  // Setup a HTTP/2 endpoint without TLS.
  options.ListenLocalhost(Port, o => o.Protocols =
      HttpProtocols.Http2);
});

// Add services to the container.
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();

// Enable reflection in Debug mode.
if (app.Environment.IsDevelopment())
{
  app.MapGrpcReflectionService();
}
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


app.Run();












// using Discount.Grpc.Services;

// var builder = WebApplication.CreateBuilder(args);

// // Additional configuration is required to successfully run gRPC on macOS.
// // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// // Add services to the container.
// builder.Services.AddGrpc();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// app.MapGrpcService<DiscountService>();
// app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// app.Run();
