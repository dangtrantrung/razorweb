using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace App.Admin.Role
{
    //[Authorize(Roles="Vip")]
    [Authorize(Roles="Administrator")]
   // [Authorize(Roles="Editor")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {

        }
        public List<IdentityRole> roles {get;set;}
        public async Task OnGet()
        {
           roles=await _roleManager.Roles.OrderBy(r=>r.Name).ToListAsync();
        }

        public void OnPost()
        {
            RedirectToPage();
        }
    }
}
