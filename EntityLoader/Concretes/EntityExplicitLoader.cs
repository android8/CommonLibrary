using EntityLoader.Abstracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityLoader.Concretes
{
   public class EntityExplicitLoader<T> : IEntityLoader<T> where T : class
   {
      private readonly IdentityDbContext _context;
      private DbSet<T> _dbSet;

      /// <summary>
      /// Explicit load the Set<T> in the specified context
      /// </summary>
      /// <param name="context"></param>

      public EntityExplicitLoader(IdentityDbContext context)
      {
         _context = context;
         _dbSet = _context.Set<T>();
      }
      public async Task<T> LoadAsync(int id, string[] paths = null)
      {
         _dbSet = _context.Set<T>();

         //Reference() to load single navigation property, and Collection() to load collections
         //_context.Entry(theDataset).Reference(dataset => dataset.theRrelatedTable).Load();

         //navigat through collection
         //using (var thisTest = _context.Entry(dataset).Collection("tmpQuestionnaire").LoadAsync())
         //{
         //   return Task.Run<T>()=> thisTest;
         //}

         //https://entityframeworkcore.com/querying-data-loading-eager-lazy
         //In explicit loading, the related data is explicitly loaded from the database at a later time.You can explicitly load a navigation property via the DbContext.Entry() method.
         //using (var context = new MyContext())
         //{
         //    var customer = context.Customers
         //        .Single(c => c.CustomerId == 1);

         //   context.Entry(customer)
         //        .Collection(c => c.Invoices)
         //        .Load();
         //}

         //You can get a LINQ query that represents the contents of a navigation property and then filters which related entities are loaded into memory
         using (_context)
         {
            return await _dbSet.FindAsync(id);

            //var customer = _dbSet
            //      .Single(c => c.CustomerId == 1);
            //return customer;

            //_context.Entry(customer)
            //      .Collection(c => c.Invoices)
            //      .Query()
            //      .Where(i => i.Date >= new DateTime(2018, 1, 1))
            //      .ToList();
         }
      }
   }
}
