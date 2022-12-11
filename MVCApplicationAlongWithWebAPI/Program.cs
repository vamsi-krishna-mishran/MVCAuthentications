using MVCApplicationAlongWithWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using MVCApplicationAlongWithWebAPI.Repository;
using MVCApplicationAlongWithWebAPI.Repos;
using Microsoft.AspNetCore.Identity;
using MVCApplicationAlongWithWebAPI.Models;
using Twilio.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MVCApplicationAlongWithWebAPI;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient<ITwilioRestClient, TwilioClient>();
builder.Services.AddDistributedMemoryCache();

// Default Policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("*", "*", "*");                        
        });
});
builder.Services.AddHttpClient();//to insert httpclient middleware.which enables use to use Ihttpclientfactory dependencies.
//builder.Services.AddTransient<CustomMiddleware1>();
//adding dependencies into the application
//builder.Services.AddDbContext<BooksContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreDB")));
builder.Services.AddDbContext<BooksContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreDB")));
builder.Services.AddDbContext<UserContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserDB")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<BooksContext>().AddDefaultTokenProviders();//it is adding identity 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
};


app.UseHttpsRedirection();
app.UseStaticFiles();//to use static files inside wwwroot folder..
app.UseAuthentication();
app.UseCors();

app.UseRouting();
app.UseAuthorization();
app.Use((context, next) =>
{
    Thread.CurrentPrincipal = context.User;
    return next(context);
});
app.UseMiddleware<Middleware>();//custom middlewares
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

