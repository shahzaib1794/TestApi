using Test.Business.HelperUtility;
using Test.Business.Service.ServiceAction;
using Test.Business.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using TestApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserService, UserService>();
//JsonConvert.DefaultSettings = () =>
//{
//    var settings = new JsonSerializerSettings();
//    settings.Converters.Add(new SingleOrArrayListConverter(true));
//    //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
//    return settings;
//};
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
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
