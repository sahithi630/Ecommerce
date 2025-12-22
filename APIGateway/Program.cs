using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load Configuration from ocelot.json (Docker or local)
var ocelotFile = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" && 
                 Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" 
                 ? "ocelot.docker.json" : "ocelot.json";
builder.Configuration.AddJsonFile(ocelotFile, optional: false, reloadOnChange: true);

// Add Controllers
builder.Services.AddControllers();

// Configure Swagger for API Gateway
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });
});

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure JWT Authentication for API Gateway
var jwtKey = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("JWT SecretKey is missing in configuration.");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("GatewayAuthentication", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };
    });

// Register Ocelot
builder.Services.AddOcelot();

var app = builder.Build();

// Enable CORS
app.UseCors("AllowAll");

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
        c.SwaggerEndpoint("https://localhost:5162/swagger/v1/swagger.json", "UserServices API v1");
        c.SwaggerEndpoint("https://localhost:5085/swagger/v1/swagger.json", "ProductService API v1");
        c.SwaggerEndpoint("https://localhost:5106/swagger/v1/swagger.json", "OrderService API v1");
    });
}

// Middleware Pipeline
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Start Ocelot
await app.UseOcelot();
app.MapControllers();

app.Run();