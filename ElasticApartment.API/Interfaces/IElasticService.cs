using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticApartment.Model;

namespace ElasticApartment.API.Interfaces
{
    public interface IElasticService
    {
        Task<IEnumerable<dynamic>> SearchAsync(QueryModel model);
    }
}
