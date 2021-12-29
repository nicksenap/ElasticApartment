using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElasticApartment.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElasticApartment.Controllers
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

        [HttpGet("{id}")]
        public Property Get(string id)
        {
            throw new AbandonedMutexException();
        }


        [HttpPost]
        public async Task<IEnumerable<Property>> Post([FromBody] QueryModel model)
        {
            var limit = model.Limit == 0 ? 10 : model.Limit;
            var termsQuery = new TermsQuery
            {
                Field = "market.keyword",
                Terms = model.Market
            };

            var queryStringQuery = new QueryStringQuery
            {
                Query = model.SearchPhase,
                Fuzziness = Fuzziness.AutoLength(1,3)
            };

            if (model.Market.Count > 0)
            {
            }

            var query1 = new SearchDescriptor<Property>().Query(q =>
                q.Bool(b => b
                    .Must(m => m.QueryString(_ => queryStringQuery),
                        qt => qt.Terms(_ => termsQuery)
                    )));

            var response = await _elasticClient.SearchAsync<Property>(query1.Size(limit));

            return response.Hits.Select(r => r.Source);
        }


        // PUT api/<SearchController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SearchController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}