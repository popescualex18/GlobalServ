using Microsoft.EntityFrameworkCore;
using GlobalServ.DataModels;
using GlobalServ.BusinessLogic.Implementations;
using GlobalServ.BusinessLogic.Interfaces;
using GlobalServ.DataAccessLayer.Interfaces;
using GlobalServ.DataAccessLayer.Implementations;
using GlobalServ.Core.Interfaces;
using GlobalServ.Core.Implementations;
using GlobalServ.Core.Helpers;
using GlobalServ.Api.Helpers;
using Microsoft.OpenApi.Models;
using Swashbuckle.Examples;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json",
        false,
        true
    );
// Add services to the container.
var configuration = builder.Configuration;
var appSettingsSection = builder.Configuration.GetSection("ApiOptions");
var appSettings = appSettingsSection.Get<ApiOptions>()!;
builder.Services.Configure<ApiOptions>(appSettingsSection);
builder.Services.AddControllers();
builder.Services.AddAuthorizationAndAuthentication(appSettings);
builder.Services.AddDbContext<GlobalServDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DatabaseConnectionString");
    options.UseSqlServer(connectionString);
});
builder.Services.AddControllers();
builder.Services.AddScoped<IUserBusinessService, UserBusinessService>();
builder.Services.AddScoped<IServiceBusinessService, ServiceBusinessService>();
builder.Services.AddScoped<IGeneratePasswordHelper, GeneratePasswordHelper>();
builder.Services.AddScoped<ITokenHelper, TokenHelper>();
builder.Services.AddScoped(typeof(IBaseBusinessService<>), typeof(BaseBusinessService<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    
    // Configure Swagger to use the Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the Bearer token in the input box below.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    // Use the Bearer token for operations that require authorization
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
