using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticApartment.Model;
using Nest;

namespace ElasticApartment.API.Services
{
    public interface IElasticService
    {
        Task<IEnumerable<dynamic>> SearchAsync(QueryModel model);
    }
}
