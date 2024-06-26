using Microsoft.EntityFrameworkCore;
using VanHorn_WebServices_Final.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DomainContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

// added this in startup file initially but this seems to work
builder.Services.AddAuthentication("ThisCookieAuth").AddCookie("ThisCookieAuth", options =>
{
    options.Cookie.Name = "ThisCookieAuth";
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

//declaring authorization policies
builder.Services.AddAuthorization(Options =>
{
    Options.AddPolicy("ServiceOnly",
        policy => policy.RequireClaim("Service"));
    Options.AddPolicy("CustomerOnly",
        policy => policy.RequireClaim("Customer"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();