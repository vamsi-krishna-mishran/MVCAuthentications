using MVCApplicationAlongWithWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using MVCApplicationAlongWithWebAPI.Repository;
using MVCApplicationAlongWithWebAPI.Repos;
using Microsoft.AspNetCore.Identity;
using MVCApplicationAlongWithWebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Twilio.Clients;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient<ITwilioRestClient, TwilioClient>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(1);//You can set Time   
});
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
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<BooksContext>().AddDefaultTokenProviders();//it is adding identity 
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => {
    option.SaveToken = true;
    option.RequireHttpsMetadata = false;
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateLifetime=true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
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
});//to set currentprinciple
app.UseSession();
//app.UseMiddleware<CustomMiddleware1>();
//app.Map("/hope", delegats);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LogIn}/{id?}");

app.Run();
//static void delegats(IApplicationBuilder app)
//{
//    app.Run(async context =>
//    {
//        await context.Response.WriteAsync("Map Test 1");
//    });
//}
