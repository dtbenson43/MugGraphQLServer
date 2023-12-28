using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Mug.Extensions;
using Mug.Mutation;
using Mug.Query;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services
builder.Services.AddIdentityServices(config);
builder.Services.AddCosmosDbService(config);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddMutationConventions(applyToAllMutations: true)
    .AddMutationType<Mutation>()
    .AddQueryType<Query>()
    .AddMongoDbFiltering("cosmos")
    .AddMongoDbSorting("cosmos");

if (!builder.Environment.IsDevelopment())
{
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
}

builder.Services.AddCors(options =>
{
    // Development CORS Policy
    options.AddPolicy("DevelopmentCorsPolicy",
        builder => builder.WithOrigins("http://localhost:5173","http://127.0.0.1:5173","http://127.0.0.1:4280", "http://localhost:4280")
                          .AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyMethod());

    // Production CORS Policy
    options.AddPolicy("ProductionCorsPolicy",
        builder => builder.WithOrigins("https://www.novustoria.com", "https://novustoria.com")
                          .AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevelopmentCorsPolicy");
}
else
{
    app.UseCors("ProductionCorsPolicy");
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

// Add routing
app.MapGraphQL().WithOptions(new GraphQLServerOptions
{
    Tool = {
        Enable = app.Environment.IsDevelopment()
    }
});
app.MapControllers();

app.Run();
