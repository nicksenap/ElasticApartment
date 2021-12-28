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
        private readonly IElasticClient elasticClient;

        public SearchController(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        // GET api/<SearchController>/5
        [HttpGet("{id}")]
        public Property Get(string id)
        {
            throw new AbandonedMutexException();
        }

        // POST api/<SearchController>
        [HttpPost]
        public async Task<Property> Post([FromBody] QueryModel model)
        {
            var response = await elasticClient.SearchAsync<Property>();
            var request = new SearchRequest
            {
                //or specify index via settings.DefaultIndex("mytweetindex"),
                From = 0,
                Size = 10,
                Query = new MatchQuery { Field = "description", Query = "nest", Fuzziness = Fuzziness.Auto }
            };
            var resp = elasticClient.Search<dynamic>(s => s.AllIndices()
                .From(0)
                .Take(10)
                .Query(qry => qry
                    .Bool(b => b
                        .Must(m => m
                            .QueryString(qs => qs
                                .DefaultField("_all")
                                .Query(model.SearchPhase))))));
            throw new AbandonedMutexException();
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