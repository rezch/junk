using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Путь перенаправления неаутентифицированных пользователей
        options.LoginPath = "/login";
        // Путь перенаправления при отказе в доступе (код ответа 403 Forbidden)
        options.AccessDeniedPath = "/denied";
        // Настройка параметров cookie
        options.Cookie.Name = "Lab11Cookie";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireSalesClaims", policy => policy.RequireClaim("Department", "Sales"));
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Ограничение по роли
app.MapGet("/admin-area", () => "Admin Content").RequireAuthorization("RequireAdminRole");

// Ограничение по политике
app.MapGet("/sales-reports", () => "Sales Reports").RequireAuthorization("RequireSalesClaims");

app.MapPost("/login", async (HttpContext context, string username, string password) =>
{
    // Здесь размещается логика проверки логина/пароля
    var success = true;
    if (success)
    {
        var role = username == "admin"
            ? "Admin"
            : "User";

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, username),
            new (ClaimTypes.Role, role),
        };
        if (password == "123")
            claims.Add(new Claim("Department", "Sales"));

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true, // Сохранение куки после закрытия браузера
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Срок истечения
        };

        // Создание и добавление аутентификационного куки в ответ
        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Results.Ok("Вход выполнен успешно");
    }

    return Results.Unauthorized();
});

app.MapGet("/profile", (ClaimsPrincipal user) =>
{
    // Доступ к данным пользователя через ClaimsPrincipal
    var userName = user.Identity.Name;

    return Results.Ok($"Пользователь: m2500237@edu.misis.ru");
}).RequireAuthorization();

app.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok("Выход выполнен успешно");
});

app.Run();
