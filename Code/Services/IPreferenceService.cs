using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Services
{
    public interface IPreferenceService<TEntity, in TPk> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetHeadersAsync();
        Task<TEntity> Update(TPk id, string entity);
    }
}
