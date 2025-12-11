using lab3.Models;
using lab3.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ProductService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1") );

// ----- Endpoints

app.MapGet("/", () => "Hello World!");

// Получить все товары
app.MapGet("/api/products",
    (ProductService service) => service.GetAll()
)
.WithName("GetAllProducts")
.WithSummary("Получить все продукты")
.WithDescription("Возвращает список всех продуктов.")
.Produces<List<Product>>(StatusCodes.Status200OK);

// Получить товар по Id
app.MapGet("/api/products/{id:int}", (int id, ProductService service) =>
{
    var product = service.GetById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById")
.WithSummary("Получить продукт по Id")
.WithDescription("Возвращает продукт с указанным идентификатором.")
.Produces<Product>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// Создать товар
app.MapPost("/api/products", (Product product, ProductService service) =>
{
    var createdProduct = service.Create(product);
    return Results.Created($"/api/products/{createdProduct.Id}", createdProduct);
})
.WithName("CreateProduct")
.WithSummary("Добавить новый продукт")
.WithDescription("Создаёт новый продукт и возвращает его с присвоенным идентификатором.")
.Produces<Product>(StatusCodes.Status201Created);

// Обновить товар
app.MapPut("/api/products/{id:int}", (int id, Product updatedProduct, ProductService service) =>
{
    if (id != updatedProduct.Id)
        return Results.BadRequest();

    var updated = service.Update(id, updatedProduct);

    return updated ? Results.NoContent() : Results.NotFound();
})
.WithName("UpdateProduct")
.WithSummary("Обновить продукт по Id")
.WithDescription("Обновляет существующий продукт.")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

// Удалить товар
app.MapDelete("/api/products/{id:int}", (int id, ProductService service) =>
{
    var deleted = service.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteProduct")
.WithSummary("Удалить продукт по Id")
.WithDescription("Удаляет продукт с заданным идентификатором.")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

// Частичное обновление (patch) продукта
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
})
.WithName("PatchProduct")
.WithSummary("Частично обновить продукт по Id")
.WithDescription("Позволяет обновить часть свойств продукта.")
.Produces<Product>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.Run();
