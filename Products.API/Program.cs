var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
 
// Adds services from Swagger/OpenAPI documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Initializes the HttpClient to connect with the Platform.sh API.

builder.Services.AddHttpClient("PlatformSH", client =>
{
    // Reads the URL from the appsettings.json file
    var baseUrl = builder.Configuration["ExternalApis:PlatformSH:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);
});

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Builder 
var app = builder.Build();

// HTTP request pipeline.
// Enables the swagger and UI.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirects HTTP requests,
app.UseHttpsRedirection();

// Defines controller paths for API use.
app.MapControllers();

app.Run();