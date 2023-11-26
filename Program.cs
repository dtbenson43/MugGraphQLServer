using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Mug.Extensions;
using Mug.Query;
using Mug.Services.CosmosDb;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services
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

app.UseHttpsRedirection();

// Add routing
app.MapGraphQL();
app.MapControllers();

app.Run();
