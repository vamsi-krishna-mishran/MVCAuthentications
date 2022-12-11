using Microsoft.EntityFrameworkCore;
using MySqlPolemro.context;

namespace MySqlPolemro
{
    public static class ServiceExtensions
    {
        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["mysqlconnection:connectionString"];
            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString,
                MySqlServerVersion.LatestSupportedServerVersion));
            Console.WriteLine(connectionString);
        }
    }
}
