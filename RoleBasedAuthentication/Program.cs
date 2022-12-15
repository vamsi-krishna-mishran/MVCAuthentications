using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using RoleBasedAuthentication.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<MyCustomMiddleware1>();
var app = builder.Build();
//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Use Middleware1 Incoming Request \n");
//    await next();
//    await context.Response.WriteAsync("Use Middleware1 Outgoing Response \n");
//});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCustomMiddleware1();
app.UseMiddleware<MyCustomMiddleware1>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Privacy}/{id?}");

//app.Run();
app.Run(); 
