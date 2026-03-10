using Asp.Versioning;
using Directo.Wari.API.Extensions;
using Directo.Wari.API.Middleware;
using Directo.Wari.Application;
using Directo.Wari.Application.Common.Interfaces;
using Directo.Wari.Application.Features.HttpUtil.Interfaces;
using Directo.Wari.Infrastructure;
using Directo.Wari.Infrastructure.Services.FacturaActiva;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ===== SERILOG =====
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

// ===== CAPAS DE CLEAN ARCHITECTURE =====
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHttpClient<IHttpUtilRepository, HttpUtilRepository>();

// ===== CONTROLLERS =====
builder.Services.AddControllers()
    //PERMITIR EL USO DE PASCALCASE MENO IMPACTO PARA EL FRONT ACTUAL
    .AddJsonOptions(option =>
    {
        option.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// ===== DESACTIVAR VALIDACIÓN AUTOMÁTICA DE ASP.NET =====
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// ===== API VERSIONING =====
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
    options.AssumeDefaultVersionWhenUnspecified = true;

});

// ===== SWAGGER =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v2", new()
    {
        Title = "WariDirecto-Modulo-Factura Activa API",
        Version = "v2"
    });

    options.CustomSchemaIds(type => type.FullName);
    options.IgnoreObsoleteActions();
    options.IgnoreObsoleteProperties();
});

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ===== HEALTH CHECKS =====
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!)
    .AddRedis(builder.Configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379");

var app = builder.Build();

// ===== MIDDLEWARE PIPELINE =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "WariDirecto API V2");
    });
}

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseCors("AllowDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");


app.Run();
