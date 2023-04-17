using EntityLoader.Abstracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityLoader.Concretes
{
   public class EntityEagerLoader<T> : IEntityLoader<T> where T : class
   {
      private readonly IdentityDbContext _context;
      private readonly DbSet<T> _dbSet;

      /// <summary>
      /// Eager load the Set<T> in the specified context
      /// </summary>
      /// <param name="context"></param>

      public EntityEagerLoader(IdentityDbContext context)
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

         //var dataset = await _context.Set<T>().ToListAsync();

         //var theseQuestions = (IIncludableQueryable<T, ICollection<T>>)dataset;

         //return theseQuestions;

         //https://entityframeworkcore.com/querying-data-loading-eager-lazy
         //In eager loading, the related data is loaded from the database as part of the initial query using Include &ThenInclude methods. The Include method specifies the related objects to include in the query results. It can be used to retrieve some information from the database and also want to include related entities. Now to retrieve all customers and their related invoices we have to use the Include Method.

         //using (_context)
         //{
         //   var customers = _context.Customers
         //       .Include(c => c.Invoices)
         //       .ToList();
         //}
      }
  }
}
