using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MvcReprApp.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var secretKey = Encoding.ASCII.GetBytes("Секретный ключ длинной не менее 16 символовСекретный ключ длинной не менее 16 символов");

builder.Services.AddEndpointsApiExplorer();

// Adding authorization bearer token support in Swagger UI
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API with Jwt Authentication", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateLifetime = true
    };
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
    .AddPolicy("RequireSalesClaims", policy => policy.RequireClaim("Department", "Sales"));

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

string GenerateJwtToken(string username)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = secretKey;
    var role = username == "admin"
        ? "Admin"
        : "User";

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Ограничение по роли
app.MapGet("/admin-area", () => "Admin Content")
.RequireAuthorization("RequireAdminRole");

// Ограничение по политике
app.MapGet("/sales-reports", () => "Sales Reports")
.RequireAuthorization("RequireSalesClaims");

bool IsValidUser(string username, string password)
{
    return true;
}

app.MapPost("/login", async (HttpContext context, [FromBody] LoginModel model) =>
{
    if (IsValidUser(model.Username, model.Password))
    {
        var token = GenerateJwtToken(model.Username);
        return Results.Ok(new { Token = token });
    }

    return Results.Unauthorized();
});

app.MapGet("/profile", (ClaimsPrincipal user) =>
{
    // Доступ к данным пользователя через ClaimsPrincipal
    var userName = user.Identity.Name;

    return Results.Ok($"Пользователь: m2500237@edu.misis.ru");
})
.RequireAuthorization();

app.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok("Выход выполнен успешно");
});

app.Run();
