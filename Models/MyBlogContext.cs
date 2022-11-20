using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace razorweb.models
{
    public class MyBlogContext:IdentityDbContext<AppUser>
    {
       
      //private const string connectstring ="Data Source=DTTRUNG-PC\\SQLEXPRESS;Initial Catalog=RAZORWEBDB;User ID=sa;Password=tr";
      public MyBlogContext (DbContextOptions<MyBlogContext> options):base(options)
      {
          // option se dc DI inject
        
      }

     
      protected override void OnConfiguring (DbContextOptionsBuilder builder)
      {
          base.OnConfiguring(builder);
         // builder.UseSqlServer(connectstring);

      }

      protected override void OnModelCreating (ModelBuilder modelbuilder)
      {
        base.OnModelCreating(modelbuilder);
        foreach (var entitytype in modelbuilder.Model.GetEntityTypes())
        {
           var tablename=entitytype.GetTableName();
           if(tablename.StartsWith("AspNet"))
           {
             entitytype.SetTableName(tablename.Substring(6));
           }
        }
      }
       public DbSet<Article> articles {get;set;}


    }
}