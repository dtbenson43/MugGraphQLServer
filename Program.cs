using HotChocolate.AspNetCore;
using Mug.Extensions;
using Mug.Query;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services
builder.Services.AddAzureSqlDbIdentityService(config);
builder.Services.AddIdentityServices(config);
builder.Services.AddCosmosDbService(config);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMongoDbFiltering("cosmos")
    .AddMongoDbSorting("cosmos");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

// Add routing
app.MapGraphQL().WithOptions(new GraphQLServerOptions
{
    Tool = {
        Enable = app.Environment.IsDevelopment()
    }
});
app.MapControllers();

app.Run();
