using SelfReferencingSample;

using Microsoft.Extensions.Hosting;

using Serilog;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleServiceCollectionExtensions
    {
        public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
        {
            services.AddScoped<Main>();

            // https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-5.0/whatsnew#dbcontextfactory
            services.AddDbContextFactory<AppContext>(b =>
                b.UseSqlite("Data Source=app.db"));
        }
    }
}
