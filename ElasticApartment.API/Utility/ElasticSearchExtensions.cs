using System;
using ElasticApartment.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ElasticApartment.API.Utility
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];
            var user = configuration["elasticsearch:user"];
            var pass = configuration["elasticsearch:password"];
            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);
            settings.BasicAuthentication(user, pass);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
        }
    }
}