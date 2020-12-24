using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SelfReferencingSample.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SelfReferencingSample
{
    public class Main
    {
        private readonly IDbContextFactory<AppContext> _contextFactory;
        private ILogger<Main> _logger;

        public IConfiguration Configuration { get; set; }

        public Main(
            IDbContextFactory<AppContext> contextFactory,
            IConfiguration configuration,
            ILogger<Main> logger)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> RunAsync()
        {
            _logger.LogInformation("Main executed");

            await EnsureCreationAsync();

            await CreateSampleDataAsync();

            using var ctx = _contextFactory.CreateDbContext();

            var all = ctx.Customers.Include(x => x.Parent).ToList();

            var virtualRootNode = all.ToTree((parent, child) => child.ParentId == parent.Id);
            var rootLevelCusomtersWithChidlren = virtualRootNode.Children.ToList();

            var flattenedListOfCustiners = virtualRootNode.Children.Flatten(node => node.Children).ToList();
            
            // Each Folder entity can be retrieved via node.Data property:
            var customer = flattenedListOfCustiners.First(node => node.Data.FullName == "Parent - 0");
            var folder = customer.Data;
            int level = customer.Level;
            bool isLeaf = customer.IsLeaf;
            bool isRoot = customer.IsRoot;
            var children = customer.Children;

            var parent = customer.Parent;
            var parents = customer.GetParents();

            return await Task.FromResult(0);
        }

        private async Task CreateSampleDataAsync()
        {
            var ctx = _contextFactory.CreateDbContext();

            // parent
            for (int p = 0; p < 100; p++)
            {
                var pe = new Customer
                {
                    FullName = $"Parent - {p}",
                    ParentId = 0,
                    Children = null
                };

                ctx.Customers.Add(pe);
                await ctx.SaveChangesAsync();

                // child
                for (int c = 0; c < 50; c++)
                {
                    var ce = new Customer
                    {
                        ParentId = pe.Id,
                        FullName = $"Child - {c}"
                    };
                    ctx.Customers.Add(ce);
                }

                await ctx.SaveChangesAsync();
            }
        }

        private async Task EnsureCreationAsync()
        {
            using var ctx = _contextFactory.CreateDbContext();
            {
                await ctx.Database.EnsureDeletedAsync();
                await ctx.Database.EnsureCreatedAsync();
            }
        }
    }
}
