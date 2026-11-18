using AuraWealth.PortfolioTrading.Application;
using AuraWealth.PortfolioTrading.Infrastructure;
using AuraWealth.WebAPI.Utilities;
using AuraWealth.MarketData.Application;
using AuraWealth.MarketData.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
//exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//---->* end of presentaion layer deps


// 2. Wire up the Infrastructure Layer (Database, Interceptors, Dapper)
builder.Services.AddPortfolioTradingInfrastructureServices(builder.Configuration);
// 3. Wire up the Application Layer (MediatR, FluentValidation)
builder.Services.AddPortfolioTradingApplicationServices();
//---->* end of PortfolioTrading domain deps


//wire infrastructure and application layer dependencies of MarketData domain
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddMarketDataApplicationServices();
builder.Services.AddMarketDataInfrastructureServices(builder.Configuration);
//---->* end of MarketData domain deps


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
