using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityLoader.Abstracts
{
   public abstract class EntityLoaderAbstract<T> where T : class
   {
    public abstract Task<IEnumerable<T>> LoadAsync();
  }
}
