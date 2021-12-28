using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticApartment.Services
{
    public interface IElasticService<T>
    {
        Task BulkInsert(IEnumerable<T> property);
    }
}
