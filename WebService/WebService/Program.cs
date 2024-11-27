using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebService.Data;
using WebService.Extensions;
using WebService.Models.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication()
	.AddCookie(IdentityConstants.ApplicationScheme)
	.AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddApiEndpoints();

builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
	var connectionString = builder.Configuration.GetConnectionString("DbConnection");
	x.UseSqlServer(connectionString);
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<User>();

app.Run();
