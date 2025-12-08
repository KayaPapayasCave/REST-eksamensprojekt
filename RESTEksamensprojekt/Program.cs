using ClassLibrary.Interfaces.DB;
using ClassLibrary.Interfaces.Local;
using ClassLibrary.Services.DB;
using ClassLibrary.Services.Local;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Local Repositories as Singletons for Dependency Injection
builder.Services.AddSingleton<INoiseRepository>(new NoiseRepository());
builder.Services.AddSingleton<IHumidityRepository>(new HumidityRepository());
builder.Services.AddSingleton<ITemperatureRepository>(new TemperatureRepository());
builder.Services.AddSingleton<ILightRepository>(new LightRepository());

// DB Repositories as Singletons for Dependency Injection
builder.Services.AddSingleton<INoiseRepositoryDB>(new NoiseRepositoryDB());
builder.Services.AddSingleton<IHumidityRepositoryDB>(new HumidityRepositoryDB());
builder.Services.AddSingleton<ITemperatureRepositoryDB>(new TemperatureRepositoryDB());
builder.Services.AddSingleton<ILightRepositoryDB>(new LightRepositoryDB());

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
    options.AddPolicy("allowAnything", // similar to * in Azure
        builder =>
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("allowAnything");

app.UseAuthorization();
app.MapControllers();

app.Run();
