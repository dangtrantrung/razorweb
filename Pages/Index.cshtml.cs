using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;
namespace razorweb.Pages;


public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly MyBlogContext _myBlogContext;
    //public List<Article> list;

    public IndexModel(ILogger<IndexModel> logger, MyBlogContext myBlogContext)
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
