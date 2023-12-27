using FluentValidation.AspNetCore;
using ProniaOnion.Application.ServiceRegistration;
using ProniaOnion.Persistence.ServerRegistration;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.GetApplicationService();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddControllers()/*.AddFluentValidation(c=>c.RegisterValidatorsFromAssemblyContaining<CreateCategoryDto>)*/;
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
