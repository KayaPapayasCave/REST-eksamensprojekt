using ClassLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<NoiseRepository>(new NoiseRepository());

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
app.UseCors("allowGetPut");

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
