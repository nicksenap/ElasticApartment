using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticApartment.Models;
using Nest;

namespace ElasticApartment.Services
{
    public class ElasticPropertyService : IElasticService<Property>
    {
        private readonly IElasticClient _elasticClient;

        public ElasticPropertyService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task BulkInsert(IEnumerable<Property> properties)
        {
            var result = await _elasticClient
                .BulkAsync(b => b.Index("property").IndexMany(properties));
            if (result.Errors)
            {
                //foreach (var itemWithError in result.ItemsWithErrors)
                //{
                //    _logger.LogError("Failed to index document {0}: {1}",
                //        itemWithError.Id, itemWithError.Error);
                //}
            }
        }
    }
}