using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Models
{
    public class LoggerDbContext : DbContext
    {
        public DbSet<ErrorLogger> ErrorLogger { get; set; }
        public LoggerDbContext(DbContextOptions<LoggerDbContext> dbContext) : base(dbContext)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
