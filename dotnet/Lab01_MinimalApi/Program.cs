using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ----- API

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.MapGet("/", () => "Hello World!")
    .WithSummary("Привет мир!")
    .WithDescription("Эта наша первая конечная точка для тестирования Minimal Api");


app.MapGet("/info", () => "Студент группы БИСТ-23-ПО-2 Крюзбан Д. И.")
    .WithSummary("Инфорфация");

app.MapGet("/variant",
        [EndpointSummary("Вариант")]
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

    int variant = id % total + 1;
    string result = $"Ваш вариант: {variant}.";

    return Results.Text(result, "text/plain; charset=utf-8");
});

// holidays
var holidays = new Dictionary<DateOnly, string>
{
    { new DateOnly(DateTime.Now.Year, 1, 1), "Новый год" },
    { new DateOnly(DateTime.Now.Year, 1, 7), "Рождество" },
    { new DateOnly(DateTime.Now.Year, 2, 14), "День святого Валентина" },
    { new DateOnly(DateTime.Now.Year, 2, 23), "День защитника Отечества" },
    { new DateOnly(DateTime.Now.Year, 3, 8), "Международный женский день" },
    { new DateOnly(DateTime.Now.Year, 5, 1), "День труда" },
    { new DateOnly(DateTime.Now.Year, 5, 9), "День Победы" },
    { new DateOnly(DateTime.Now.Year, 6, 12), "День России" },
    { new DateOnly(DateTime.Now.Year, 11, 4), "День народного единства" },
    { new DateOnly(DateTime.Now.Year, 12, 31), "Новый год" }
};


app.MapGet("/isholidays",
        [EndpointSummary("Праздник")]
        [EndpointDescription("Возвращает праздник по дате")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK, "text/plain")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        ([FromQuery] DateTime date) =>
{
    DateOnly dateOnly;
    try {
        dateOnly = DateOnly.FromDateTime(date);
    } catch (Exception) {
         return Results.BadRequest("Не верно указана дата.");
    }

    string result = $"Сегодня не праздник.";

    if (holidays.TryGetValue(dateOnly, out var holidayName))
    {
        result = $"Сегодня: {holidayName}";
    }

    return Results.Text(result, "text/plain; charset=utf-8");
});


app.Run();
