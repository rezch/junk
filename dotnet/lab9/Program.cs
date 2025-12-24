using GraphQL;
using GraphQL.Server;
using lab5.Database;
using lab5.Models;
using lab5.Queries;
using lab5.Schemas;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<SchoolContext>();
builder.Services.AddScoped<DatabaseHelper>();
builder.Services.AddScoped<StudentType>();
builder.Services.AddScoped<StudentQuery>();
builder.Services.AddScoped<GraphQL.Types.ISchema, AppSchema>();
builder.Services.AddGraphQL(b => b.AddSchema<AppSchema>().AddSystemTextJson());

var app = builder.Build();
// app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1") );

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost
});

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL(); // по умолчанию /graphql
});

app.Run();
