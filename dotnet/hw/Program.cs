using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

using BulletinBoard.Models;
using BulletinBoard.Database;
using BulletinBoard.Login;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "BulletinBoard_Cookie";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ServiceContext>();
builder.Services.AddScoped<DatabaseHelper>();

builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ServiceContext>();
    db.Database.EnsureCreated();
    if (!db.Users.Any())
    {
        db.Users.AddRange(new List<User>
            {
                new() {
                    UserName = "user1",
                    Role = LoginRoles.User,
                    PasswordHash = "pass",
                },
                new() {
                    UserName = "admin",
                    Role = LoginRoles.Admin,
                    PasswordHash = "adminpass",
                }
            });

        db.Notes.AddRange(new List<Note>
            {
                new() {
                    Owner = "user1",
                    CreationDate = DateTime.Now,
                    Price = 22,
                    Name = "Bike",
                    Description = "",
                },
                new() {
                    Owner = "other_user",
                    CreationDate = DateTime.Now,
                    Price = 425,
                    Name = "PC",
                    Description = "some description",
                },
                new() {
                    Owner = "user1",
                    CreationDate = DateTime.Now,
                    Name = "Old bike",
                    Description = "for free",
                },
            });

        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
        | ForwardedHeaders.XForwardedFor
        | ForwardedHeaders.XForwardedHost
});

app.UseRouting();

app.UseStaticFiles();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
