using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Recruit.Repository;
using RecruitApi;
using RecruitApi.Data;
using RecruitApi.Repository;



var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    // Add the below config for releasing new versions
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Recruit V1",
        Description = "API to manage Recruiting",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Recruit",
            Url = new Uri("https://example.com")
        },
        License = new OpenApiLicense
        {
            Name = "Recruit License",
            Url = new Uri("https://example.com/license")
        }
    });
});
// Swagger EOF


// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
builder.Services.AddScoped<IRecruitRepository, RecruitRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));


builder.Services.AddControllers();



var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

// Swagger 
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Add endpoints for new API Release
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Recruit API V1");
});
// Swagger EOF

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
