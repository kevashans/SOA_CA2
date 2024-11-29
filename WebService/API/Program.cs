using API.Extensions;
using Data.Auth;
using Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application and infrastructure services
builder.Services.AddApplicationServices();// business logic (Service Layer)
builder.Services.AddInfrastructureServices(builder.Configuration);// data Layer (Repositories, DbContext)

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<User>();

app.Run();
