using FluentValidation;
using Microsoft.OpenApi.Models;
using Serilog;
using Transacciones.API.Endpoints.Cuentas;
using Transacciones.API.Mapper;
using Transacciones.API.Middleware;
using Transacciones.API.Validators;
using Transacciones.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMemoryCache();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
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
builder.Services.AddScoped<IValidator<Transacciones.API.Endpoints.Transacciones.AbonoRequest>, AbonoRequestValidator>();
builder.Services.AddScoped<IValidator<Transacciones.API.Endpoints.Transacciones.RetiroRequest>, RetiroRequestValidator>();



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
