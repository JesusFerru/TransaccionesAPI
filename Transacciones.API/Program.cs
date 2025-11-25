using FluentValidation;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Transacciones.API.Endpoints.Cuentas;
using Transacciones.API.Endpoints.Transacciones;
using Transacciones.API.Filters;
using Transacciones.API.Mapper;
using Transacciones.API.Middleware;
using Transacciones.API.Validators;
using Transacciones.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMemoryCache();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});

builder.Services.AddProblemDetails(option =>
{
    option.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
    };
});
builder.Services.AddExceptionHandler<HandleException>();

builder.Services.AddScoped<IValidator<CreateCuentaRequest>, CreateCuentaValidator>();
builder.Services.AddScoped<IValidator<GetCuentaByIdRequest>, GetCuentaByIdValidator>();
builder.Services.AddScoped<IValidator<AbonoRequest>, AbonoRequestValidator>();
builder.Services.AddScoped<IValidator<RetiroRequest>, RetiroRequestValidator>();

builder.Services.AddScoped<ApiKeyAuthorizationFilter>();

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// OpenAPI Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transacciones API",
        Version = "v1",
        Description = "API para gestionar cuentas y transacciones bancarias."
    });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "API Key en el header con formato: Bearer {ApiKey}",
        Scheme = "ApiKeyScheme",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKeyAuth"
        }
    };

    options.AddSecurityDefinition("ApiKeyAuth", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            Array.Empty<string>()
        }
    };

    options.AddSecurityRequirement(securityRequirement);
    // XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.EnableAnnotations();
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
