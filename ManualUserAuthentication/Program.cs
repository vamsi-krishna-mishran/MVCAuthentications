

using ManualUserAuthentication.Context;
using ManualUserAuthentication.Extensions;
using ManualUserAuthentication.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();//options=> options.Filters.Add<BasicAuthFilter>());
//builder.Services.AddScoped<Books>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBookService,BookService>();
builder.Services.AddDbContext<BooksContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreDB")));
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<BooksContext>().AddDefaultTokenProviders();
//builder.Services.AddAuthentication("Basic").AddScheme<BasicAuthenticationOptions,BasicAuthenticationHandler>("Basic", null);
//builder.Services.AddDbContext<BooksContext>(
//    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreDB")));
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseStaticFiles();

//app.UseExceptionHandler("/Home/Error");
app.UseExceptionHandler(
                options =>
                {
                    options.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "text/html";
                            var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
                            if (null != exceptionObject)
                            {
                                var errorMessage = $"<b>Exception Error: {exceptionObject.Error.Message} </b> {exceptionObject.Error.StackTrace}";
                                await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
                            }
                        });
                }
            );

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=LogIn}/{id?}");
app.Run();
