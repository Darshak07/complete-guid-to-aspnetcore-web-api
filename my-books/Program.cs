using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Exceptions;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//=============Logger==============================================

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();

});
//builder.Host.UseSerilog();


 //Log.Logger = new LoggerConfiguration()
 //                   .MinimumLevel.Debug() 
 //                   .MinimumLevel.Information()
 //                   .WriteTo.Console()
 //                   .WriteTo.File("Logs/log.txt",rollingInterval:RollingInterval.Day).CreateLogger();

Log.Information("Log is working");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v2",new OpenApiInfo { Title ="my Books2",Version ="v2"}));

string? connstrings = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connstrings));

//Configure services
builder.Services.AddTransient<BooksService>();
builder.Services.AddTransient<AuthorService>();
builder.Services.AddTransient<PublisherService>();
builder.Services.AddApiVersioning(config => 
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;

    //config.ApiVersionReader = new HeaderApiVersionReader("custome-header-version");
    //config.ApiVersionReader = new MediaTypeApiVersionReader();
});
//=============JWT Based Authetication=========
//Token validation parameters
var tokenvalidationParameters=new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"])),
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:ValidAudience"],
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddSingleton(tokenvalidationParameters);
//add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
//add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
//add JWT Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;//if enabled only will work with https if false it works with http
    options.TokenValidationParameters = tokenvalidationParameters;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>c.SwaggerEndpoint("/swagger/v2/swagger.json","my Books2"));
   
}

app.UseHttpsRedirection();
//Authentication && Authorisation
app.UseAuthentication();
app.UseAuthorization();

app.ConfigureBuildinExceptionHandler();
//app.ConfigureCustomeExceptionHandle();
app.MapControllers();
//AppDbInitializer.Seed(app);
AppDbInitializer.SeedRoles(app).Wait();
app.Run();
