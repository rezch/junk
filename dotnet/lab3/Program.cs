using lab3.Models;
using lab3.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ProductService>();

var app = builder.Build();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1") );

app.MapGet("/", () => "Hello World!");

// Получить все товары
app.MapGet("/api/products",
    (ProductService service) => service.GetAll());

// Получить товар по Id
app.MapGet("/api/products/{id:int}", (int id, ProductService service) =>
{
    var product = service.GetById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

// Создать товар
app.MapPost("/api/products", (Product product, ProductService service) =>
{
    var createdProduct = service.Create(product);
    return Results.Created($"/api/products/{createdProduct.Id}", createdProduct);
});

// Обновить товар
app.MapPut("/api/products/{id:int}", (int id, Product updatedProduct, ProductService service) =>
{
    if (id != updatedProduct.Id)
        return Results.BadRequest();

    var updated = service.Update(id, updatedProduct);

    return updated ? Results.NoContent() : Results.NotFound();
});

// Удалить товар
app.MapDelete("/api/products/{id:int}", (int id, ProductService service) =>
{
    var deleted = service.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.MapPatch("/api/products/{id:int}", (int id, Product patch, ProductService service) =>
{
    var product = service.GetById(id);
    if (product == null)
        return Results.NotFound();

    if (patch.Name is not null)
        product.Name = patch.Name;

    if (patch.Price != -1)
        product.Price = patch.Price;

    if (patch.Description is not null)
        product.Description = patch.Description;

    var updated = service.Update(id, product);
    return updated ? Results.Ok(product) : Results.NotFound();
});

app.Run();
