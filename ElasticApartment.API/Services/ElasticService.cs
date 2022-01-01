using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticApartment.API.Interfaces;
using ElasticApartment.Model;
using Microsoft.Extensions.Logging;
using Nest;

namespace ElasticApartment.API.Services
{
    public class ElasticService : IElasticService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger _logger;

        public ElasticService(IElasticClient elasticClient, ILogger<ElasticService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public async Task<IEnumerable<dynamic>> SearchAsync(QueryModel model)
        {
            var limit = model.Limit.Equals(0) ? Constants.DefaultLimit : model.Limit;
            var multiMatchQuery = new MultiMatchQuery
            {
                Fuzziness = Fuzziness.EditDistance(Constants.EditDistance),
                Query = model.SearchPhase,
                Analyzer = Constants.StopSearchAnalyzer,
                AutoGenerateSynonymsPhraseQuery = true
            };

            var termsQuery = new TermsQuery();
            if (model.Market.Count > 0)
            {
                termsQuery.Field = Constants.MarketKeyword;
                termsQuery.Terms = model.Market;
            }

            var searchDescriptor = new SearchDescriptor<dynamic>()
                .AllIndices()
                .Query(q => q.Bool(b => b
                    .Must(m => m.MultiMatch(_ => multiMatchQuery),
                        qt => qt.Terms(_ => termsQuery)
                    )));

            var response = await _elasticClient.SearchAsync<dynamic>(searchDescriptor.Size(limit));
            _logger.LogInformation(
                $"[ElasticService.SearchAsync] Elastic search returned {response.Hits.Count} result for searchPhase {model.SearchPhase}");

            return response.Hits.Select(r => r.Source);
        }
    }
}