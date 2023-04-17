using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityLoader.Abstracts
{
   public interface IEntityLoader<T> where T : class
   {
    public Task<T> LoadAsync(int id, string[] paths = null);
  }
}
