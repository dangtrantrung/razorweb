using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Models;
//using Microsoft.AspNetCore.Authorization;


namespace App.Pages{

public class IndexModel : PageModel
{
    
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _myBlogContext;
    //public List<Article> list;

    public IndexModel(ILogger<IndexModel> logger, AppDbContext myBlogContext)
    {
        _logger = logger;
        _myBlogContext=myBlogContext; //DI dich vu Myblogcontrext database, SQL
    }

    /* public async Task OnGet()
    {
     // CreateDatabase();
     var list= await GetArticlesAsync();
    //list=GetArticlesAsync().Result;
    //Console.WriteLine("Complete"+posts.IsCompletedSuccessfully);
     Console.WriteLine("Complete..."+list.Count);
    ViewData["POSTS"] =list;

    }
   public async Task<List<Article>> GetArticlesAsync()
   {
       
       var list= (from a in _myBlogContext.articles
                 orderby a.Created descending
                 select a).ToList();
      await Task.CompletedTask;

      return list;
   } */

public void OnGet()
{
    var list= (from a in _myBlogContext.articles
                 orderby a.Created descending
                 select a).ToList();
     ViewData["POSTS"] =list;
}

  /*  static void CreateDatabase()
{
    using var dbcontext= new MyBlogContext();
    string dbname=dbcontext.Database.GetDbConnection().Database;
    var kq= dbcontext.Database.EnsureCreated();
    if(kq==true)
    {
        Console.WriteLine($"TẠO db  {dbname} thành công");
    }
    else{
        Console.WriteLine($"TẠO db  {dbname} không thành công");
    }

} */
}

}
// Phat sinh cac trang CRUD
//dotnet aspnet-codegenerator razorpage -m razorweb.models.Article -dc razorweb.models.MyBlogContext -outDir Pages/Blog -udl --referenceScriptLibraries
