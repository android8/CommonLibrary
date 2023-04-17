using EntityLoader.Abstracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityLoader.Concretes
{
   public class EntityLazyLoader<T> : IEntityLoader<T> where T : class
   {
      private readonly IdentityDbContext _context;
      private readonly DbSet<T> _dbSet;

      /// <summary>
      /// Lazy load the Set<T> in the specified context
      /// </summary>
      /// <param name="context"></param>

      public EntityLazyLoader(IdentityDbContext context)
      {
         _context = context;
         _dbSet = _context.Set<T>();
      }
      public async Task<T> LoadAsync(int id, string[] paths = null)
      {
         var model = await _dbSet.FindAsync(id);
         foreach (var path in paths)
         {
            _context.Entry(model).Reference(path).Load();
         }
         return model;
      }
   }
}
