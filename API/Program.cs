using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PurchaseTransactionAPI.API.Middlewares;
using PurchaseTransactionAPI.API.Validators;
using PurchaseTransactionAPI.Application.ExternalServices;
using PurchaseTransactionAPI.Application.ExternalServices.Treasury;
using PurchaseTransactionAPI.Application.ExternalServices.Treasury.Options;
using PurchaseTransactionAPI.Application.Interfaces;
using PurchaseTransactionAPI.Application.Persistence;
using PurchaseTransactionAPI.Application.Repositories;
using PurchaseTransactionAPI.Application.Services;
using PurchaseTransactionAPI.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTransactionRequestValidator>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IExchangeRateClient, ExchangeRateClient>();
builder.Services.AddScoped<IHttpClientFactoryCustom, HttpClientFactoryCustom>();

builder.Services.Configure<ExchangeRateClientOptions>(
    builder.Configuration.GetSection("ExchangeRateClient"));


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();