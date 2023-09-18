using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using my_books.Data;
using my_books.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v2",new OpenApiInfo { Title ="my Books2",Version ="v2"}));
string? connstrings = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connstrings));

//Configure services
builder.Services.AddTransient<BooksService>();

//======================
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>c.SwaggerEndpoint("/swagger/v2/swagger.json","my Books2"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
AppDbInitializer.Seed(app);
app.Run();
