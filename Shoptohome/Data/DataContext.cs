using Microsoft.EntityFrameworkCore;
using Shoptohome.Domain;

namespace Shoptohome.Data
{
   public class DataContext:DbContext
   {
      public DataContext(DbContextOptions<DataContext> options) : base(options) { }
      public DbSet<User> Users { get; set; }


   }
}
