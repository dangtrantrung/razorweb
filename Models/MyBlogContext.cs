using Microsoft.EntityFrameworkCore;

namespace razorweb.models
{
    public class MyBlogContext:DbContext{
       
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
      }
       public DbSet<Article> articles {get;set;}


    }
}