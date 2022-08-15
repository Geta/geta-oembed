using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Geta.OEmbed.Optimizely.Tests.Common.Infrastructure
{
    public class MemoryEndpointRouteBuilder : IEndpointRouteBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICollection<EndpointDataSource> _dataSources;

        public MemoryEndpointRouteBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dataSources = new HashSet<EndpointDataSource>();
        }

        public IServiceProvider ServiceProvider => _serviceProvider;
        public ICollection<EndpointDataSource> DataSources => _dataSources;

        public IApplicationBuilder CreateApplicationBuilder()
        {
            return new ApplicationBuilder(_serviceProvider);
        }
    }
}
