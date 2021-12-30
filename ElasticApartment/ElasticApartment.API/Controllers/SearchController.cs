using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticApartment.Model;
using Microsoft.AspNetCore.Mvc;
using Nest;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElasticApartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public SearchController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpPost]
        public async Task<IEnumerable<dynamic>> Post([FromBody] QueryModel model)
        {
            var limit = model.Limit == 0 ? 25 : model.Limit;

            var queryStringQuery = new QueryStringQuery
            {
                Query = model.SearchPhase,
                Fuzziness = Fuzziness.Auto,
                FuzzyMaxExpansions = 5,
                FuzzyPrefixLength = 5,
                FuzzyRewrite = MultiTermQueryRewrite.ConstantScore,
                Analyzer = "stop",
                Boost = 1.1
            };

            var termsQuery = new TermsQuery();

            if (model.Market.Count > 0)
            {
                termsQuery.Field = "market.keyword";
                termsQuery.Terms = model.Market;
            }

            var searchDescriptor = new SearchDescriptor<dynamic>()
                .AllIndices()
                .Query(q => q.Bool(b => b
                    .Must(m => m.QueryString(_ => queryStringQuery),
                        qt => qt.Terms(_ => termsQuery)
                    )));

            var response = await _elasticClient.SearchAsync<dynamic>(searchDescriptor.Size(limit));

            return response.Hits.Select(r => r.Source);
        }
    }
}