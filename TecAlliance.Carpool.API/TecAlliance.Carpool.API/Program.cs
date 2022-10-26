using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
using TecAlliance.Carpool.Business.SampleData;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TecAlliance.Carpool.Business.Services;
using TecAlliance.Carpool.Data.Service;
using TecAlliance.Carpool.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ExampleFilters();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Carpool API",
        Description = "An ASP.NET Core Web API for managing Carpools",
        //TermsOfService = new Uri("https://example.com/terms"),
        //Contact = new OpenApiContact
        //{
        //    Name = "Example Contact",
        //    Url = new Uri("https://example.com/contact")
        //},
        //License = new OpenApiLicense
        //{
        //    Name = "Example License",
        //    Url = new Uri("https://example.com/license")
        //}
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSingleton<UserDtoProvider>();
builder.Services.AddSingleton<List<UserDtoProvider>>();
builder.Services.AddSingleton<CarpoolUnitDtoProvider>();
builder.Services.AddSingleton<List<CarpoolUnitDtoProvider>>();
builder.Services.AddSingleton<ShortUserInfoDtoProvider>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ShortUserInfoDtoProvider>();

builder.Services.AddScoped<IUserBusinessServices, UserBusinessServices>();
builder.Services.AddScoped<IUserDataServices, UserDataServices>();

builder.Services.AddScoped<ICarpoolUnitBusinessServices, CarpoolUnitBusinessServices>();
builder.Services.AddScoped<ICarpoolUnitDataServices, CarpoolUnitDataServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI(options =>
        {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
