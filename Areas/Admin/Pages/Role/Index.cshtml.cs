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
        public class RoleModel:IdentityRole
        {
            // rolemodel kethua tu identityrole và có thêm property claims[]
            public string[] Claims {get;set;}
        }
        public List<RoleModel> roles {get;set;}
        public async Task OnGet()
        {
            //_roleManager.GetClaimsAsync();
           var r=await _roleManager.Roles.OrderBy(r=>r.Name).ToListAsync();
           roles=new List<RoleModel>();

           foreach (var _r in r)
           {
               var claims =await _roleManager.GetClaimsAsync(_r);
               var claimsString=claims.Select(c=>c.Type + "=" +c.Value);
               var rm= new RoleModel()
               {
                  Name=_r.Name,
                  Id=_r.Id,
                  Claims=claimsString.ToArray()
               };
               roles.Add(rm);
           }

        }

        public void OnPost()
        {
            RedirectToPage();
        }
    }
}
