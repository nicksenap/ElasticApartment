using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticApartment.Model;
using Nest;

namespace ElasticApartment.API.Services
{
    public class ElasticService : IElasticService
    {
        private readonly IElasticClient _elasticClient;

        public ElasticService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<dynamic>> SearchAsync(QueryModel model)
        {
            var limit = model.Limit == 0 ? Constants.DefaultLimit : model.Limit;

            var multiMatchQuery = new MultiMatchQuery
            {
                Fuzziness = Fuzziness.EditDistance(3),
                Query = model.SearchPhase,
                Analyzer = "stop",
                AutoGenerateSynonymsPhraseQuery = true,
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

            return response.Hits.Select(r => r.Source);
        }
    }
}