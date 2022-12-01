namespace MVCApplicationAlongWithWebAPI
{
    public class CustomMiddleware1 : IMiddleware
    {
        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("custom middleware code1");
            //await next(context);
            await context.Response.WriteAsync("custom middleware code2");
        }
    }
}
