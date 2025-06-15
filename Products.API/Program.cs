// Web application Builder.
var builder = WebApplication.CreateBuilder(args);

// Services necessary for the controllers to function.
builder.Services.AddControllers();
 
// Adds services from Swagger/OpenAPI documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Initializes the HttpClient to connect with the Platform.sh API.

builder.Services.AddHttpClient("PlatformSH", client =>
{
    client.BaseAddress = new Uri("https://api.platform.sh/api/projects/pj2g557uk2z2g/");
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

// Middleware CORS
app.UseHttpsRedirection();

// Defines controller paths for API use.
app.MapControllers();

app.Run();