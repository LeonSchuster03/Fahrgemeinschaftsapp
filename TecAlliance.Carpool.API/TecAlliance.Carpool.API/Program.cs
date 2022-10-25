using Swashbuckle.AspNetCore.Filters;
using TecAlliance.Carpool.Business.SampleData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ExampleFilters();
});

builder.Services.AddSingleton<UserDtoProvider>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<UserDtoProvider>();

builder.Services.AddSingleton<CarpoolUnitDtoProvider>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<CarpoolUnitDtoProvider>();

builder.Services.AddSingleton<ShortUserInfoDtoProvider>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ShortUserInfoDtoProvider>();

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
