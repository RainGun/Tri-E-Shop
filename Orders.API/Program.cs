var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Adds services from Swagger/OpenAPI documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Builder 
var app = builder.Build();

// Configure the HTTP request pipeline.

// Enables the swagger and UI.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Middleware CORS
app.UseHttpsRedirection();

// Defines controller paths for API use.
app.MapControllers();

app.Run();