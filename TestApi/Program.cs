using Test.Business.HelperUtility;
using Test.Business.Service.ServiceAction;
using Test.Business.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserService, UserService>();
// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options=>
    options.SerializerSettings.Converters.Add(new SingleOrArrayListConverter(true)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
