using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace razorweb.Pages_Blog
{
    public class IndexModel : PageModel
    {
        private readonly razorweb.models.MyBlogContext _context; //DI Myblogxontext DB

        public IndexModel(razorweb.models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }

        public async Task OnGetAsync(string SearchString)
        {
            //Article = await _context.articles.ToListAsync();
            var qr= from a in _context.articles
                    orderby a.Created descending
                    select a;
           

            if(!string.IsNullOrEmpty(SearchString))
            {
                Article =qr.Where(a=>a.Title.Contains(SearchString)).ToList();
            }
            else
            {
                 Article =await qr.ToListAsync(); // select all articles orderby created datetime
            }

           
        }
    }
}
