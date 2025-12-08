using ClassLibrary.Interfaces.DB;
using ClassLibrary.Interfaces.Local;
using ClassLibrary.Services.DB;
using ClassLibrary.Services.Local;

/// <summary>
/// Entry point for the ASP.NET Core REST API application.
/// Responsible for configuring services, middleware, dependency injection, 
/// JSON serialization and CORS policies for sensor data.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------
// Add services to the container
// ------------------------------------------------------------

/// <summary>
/// Adds MVC controllers and JSON converters for DateOnly and TimeOnly
/// which are not natively supported by System.Text.Json.
/// </summary>
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

/// <summary>
/// Swagger/OpenAPI configuration for API documentation.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/// <summary>
/// Registers Local repositories as Singletons, enabling them to be injected in controllers.
/// </summary>
builder.Services.AddSingleton<INoiseRepository>(new NoiseRepository());
builder.Services.AddSingleton<IHumidityRepository>(new HumidityRepository());
builder.Services.AddSingleton<ITemperatureRepository>(new TemperatureRepository());
builder.Services.AddSingleton<ILightRepository>(new LightRepository());


/// <summary>
/// Registers DB repositories as Singletons, enabling them to be injected in controllers.
/// </summary>
builder.Services.AddSingleton<NoiseRepository>(new NoiseRepository());
builder.Services.AddSingleton<HumidityRepository>(new HumidityRepository());
builder.Services.AddSingleton<TemperatureRepository>(new TemperatureRepository());
builder.Services.AddSingleton<LightRepository>(new LightRepository());

/// <summary>
/// Configures multiple CORS policies, including a policy that allows requests from anywhere.
/// </summary>
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAnythingFromZealand",
                builder =>
                    builder.WithOrigins("http://zealand.dk")
                        .AllowAnyHeader()
                        .AllowAnyMethod());

    options.AddPolicy("allowGetPut",
                    builder =>
                        builder.AllowAnyOrigin()
                        .WithMethods("GET", "PUT")
                        .AllowAnyHeader());

    options.AddPolicy("allowAnything",
        builder =>
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
});

var app = builder.Build();

// ------------------------------------------------------------
// Configure the HTTP request pipeline
// ------------------------------------------------------------

/// <summary>
/// Enables Swagger UI for API exploration and endpoint testing.
/// Swagger is always active regardless of environment.
/// </summary>
app.UseSwagger();
app.UseSwaggerUI();

/// <summary>
/// Forces HTTPS redirection, improving connection security.
/// </summary>
app.UseHttpsRedirection();

/// <summary>
/// Enables the configured CORS policy allowing all origins, methods and headers.
/// </summary>
app.UseCors("allowAnything");

/// <summary>
/// Adds authorization middleware (even though authentication is not implemented in this project).
/// </summary>
app.UseAuthorization();

/// <summary>
/// Maps controllers so incoming HTTP requests are routed to endpoints.
/// </summary>
app.MapControllers();

/// <summary>
/// Starts the web application.
/// </summary>
app.Run();
