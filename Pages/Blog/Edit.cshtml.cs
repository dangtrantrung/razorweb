using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Pages_Blog
{
    public class EditModel : PageModel
    {
        private readonly App.Models.AppDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(App.Models.AppDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService=authorizationService;
        }

        [BindProperty]
        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return Content($"Không thấy bài viết có ID này: {id}");
            }

            Article = await _context.articles.FirstOrDefaultAsync(m => m.Id == id);

            if (Article == null)
            {
               return Content($"Không thấy bài viết có ID này: {id}");
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                // kiểm tra quyền cập nhật - ngày tạo article trước khi cập nhật
                   var canUpdate=await _authorizationService.AuthorizeAsync(this.User,Article,"CanUpdateArticle");
                    if(canUpdate.Succeeded)
                    {
                      await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Content("Bạn không có quyền cập nhật article này");
                    }


                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.Id))
                {
                    return Content($"Không thấy bài viết có ID này {Article.Id}");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
            return _context.articles.Any(e => e.Id == id);
        }
    }
}
