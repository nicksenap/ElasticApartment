using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticApartment.API.Interfaces;
using ElasticApartment.API.Services;
using ElasticApartment.Model;
using Microsoft.AspNetCore.Mvc;

namespace ElasticApartment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IElasticService _elasticService;

        public SearchController(IElasticService elasticService)
        {
            _elasticService = elasticService;
        }

        [HttpPost]
        public async Task<IEnumerable<dynamic>> Post([FromBody] QueryModel model)
        {
            return await _elasticService.SearchAsync(model);
        }
    }
}