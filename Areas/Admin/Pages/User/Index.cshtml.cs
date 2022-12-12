using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace App.Admin.User
{
    [Authorize]
    public class Index : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public Index(UserManager<AppUser> userManager)
        {
          _userManager=userManager;
        }
        public List<UserAndRole> users {get;set;}

        public class UserAndRole:AppUser
        {
            public string RoleNames {get;set;}
        }
        public const int ITEMS_PER_PAGE=10;
        [BindProperty(SupportsGet =true,Name ="p")]
        public int currentPage{get;set;}
        public int countPages{get;set;}

        [TempData]
        public string StatusMessage {get;set;}
        public int Totaluser {get;set;}

        public async Task OnGet()
        {
           //users=await _userManager.Users.OrderBy(u=>u.UserName).ToListAsync();
           var qr=_userManager.Users.OrderBy(u=>u.UserName);
            Totaluser=await qr.CountAsync();
            countPages=(int)Math.Ceiling((double)Totaluser/ITEMS_PER_PAGE);
            if(currentPage<1)
            {
                currentPage=1;
            }

            if(currentPage>countPages)
            {
                currentPage=countPages;
            }
             var qr1= qr.Skip((currentPage-1)*ITEMS_PER_PAGE)
                        .Take(ITEMS_PER_PAGE)
                        .Select(u=> new UserAndRole()
                        {
                            Id=u.Id,
                            UserName=u.UserName
                        });

            users= await qr1.ToListAsync();

            foreach (var user in users)
            {
                var roles= await _userManager.GetRolesAsync(user);

                user.RoleNames=string.Join(",",roles);
            }
        }

        public void OnPost()
        {
            RedirectToPage();
        }
    }
}
