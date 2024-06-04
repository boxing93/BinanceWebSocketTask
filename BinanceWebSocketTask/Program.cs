using BinanceWebSocketTask.API.Common.Filters;
using BinanceWebSocketTask.Application;
using BinanceWebSocketTask.Application.Common.Interfaces;
using BinanceWebSocketTask.Domain.Entities;
using BinanceWebSocketTask.Infrastructure;
using BinanceWebSocketTask.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    //options.AddPolicy("")
    options.AddPolicy("_allowsAny",
        builder =>
        {
            builder
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
})
.AddXmlSerializerFormatters();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = (DatabaseContext)scope.ServiceProvider.GetService(typeof(DatabaseContext));
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.AddAsync(new CryptoSymbol { Name = "BTCUSDT" });
        await dbContext.AddAsync(new CryptoSymbol { Name = "ADAUSDT" });
        await dbContext.AddAsync(new CryptoSymbol { Name = "ETHUSDT" });
        await dbContext.SaveChangesAsync();
    }
    catch (Exception ex)
    {

    }
}
var service = (IBinanceService)builder.Services.BuildServiceProvider().GetService(typeof(IBinanceService));
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseResponseCaching();
app.MapControllers();

app.Run();

