using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Penrose.Application.Common;

namespace Penrose.Application.Extensions
{
    public static class MapperExtensions
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(configure =>
            {
                configure.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}