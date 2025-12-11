using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));

// ----- Endpoints

app.MapGet("/", () => "Hello World!")
    .WithSummary("hello_world")
    .WithDescription("Эта наша первая конечная точка для тестирования Minimal Api");


app.MapGet("/info", () => "Студент группы БИСТ-23-ПО-2 Крюзбан Д. И.")
    .WithSummary("Инфорфация");

app.MapGet("/variant",
        [EndpointSummary("variant")]
        [EndpointDescription("Получить вариант задания по id")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK, "text/plain")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        ([FromQuery(Name = "student id")] int id,
         [FromQuery(Name = "number of tasks")] int total) =>
{
    if (total == 0)
    {
         return Results.BadRequest("Деление на ноль невозможно.");
    }

    int variant = (id % total) + 1;
    string result = $"Ваш вариант: {variant}.";

    return Results.Text(result, "text/plain; charset=utf-8");
});

app.MapGet("/daysuntil",
        ([FromQuery] DateTime date) =>
{
    var today = DateTime.UtcNow.Date;
    var targetDate = date.Date;
    int daysLeft = (targetDate - today).Days;

    if (daysLeft < 0)
        return Results.BadRequest("Указанная дата уже прошла.");

    return Results.Ok(new { daysLeft });
})
.WithName("GetDaysUntil")
.WithSummary("get days until")
.WithDescription("Возвращает сколько дней осталось до указанной даты")
.Produces<object>(StatusCodes.Status200OK)
.Produces<string>(StatusCodes.Status400BadRequest);

app.Run();
