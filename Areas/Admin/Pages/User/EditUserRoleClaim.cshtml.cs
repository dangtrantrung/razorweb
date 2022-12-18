using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.Models;

namespace App.Admin.User
{
    public class EditUserRoleClaimModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        

        public EditUserRoleClaimModel( AppDbContext myBlogContext, UserManager<AppUser> userManager)
        {
           _context=myBlogContext;
           _userManager=userManager;
        }
            [TempData]
        public string StatusMessage {get;set;}
        public NotFoundObjectResult OnGet()=>NotFound("Không được truy cập");

         //class con tạo role
        public class InputModel
        {
           [Display(Name="Kiểu (Tên) của Role Claim")]
           [Required(ErrorMessage="Phải nhập {0}")]
           [StringLength(256,MinimumLength=3,ErrorMessage="{0} phải dài từ {2} đến {1} ký tự")]
           public string ClaimType{get;set;}
           [Display(Name="Giá trị của Claim Value")]
           [Required(ErrorMessage="Phải nhập {0}")]
           [StringLength(256,MinimumLength=3,ErrorMessage="{0} phải dài từ {2} đến {1} ký tự")]
           public string ClaimValue{get;set;}
        }
        [BindProperty] // khi ng dùng submit nó sẽ bind form value vào property input này
        public InputModel Input{get;set;}

        public AppUser user {get;set;}
        public IdentityUserClaim<string> userclaim{get;set;}

        public async Task<IActionResult> OnGetAddClaimAsync(string userid)
        {
            user=await _userManager.FindByIdAsync(userid);
            if(user==null) return NotFound("Không tìm thấy user này");
            return Page();

        }
        public async Task<IActionResult> OnGetEditClaimAsync(int? claimid)
        {
            if(claimid==null) return NotFound("Không tìm thấy user claim này");
            userclaim=_context.UserClaims.Where(c=>c.Id==claimid).FirstOrDefault();
            user=await _userManager.FindByIdAsync(userclaim.UserId);
            if(user==null) return NotFound("Không tìm thấy user này");

            Input = new InputModel()
            {
               ClaimType=userclaim.ClaimType,
               ClaimValue=userclaim.ClaimValue
            };
            return Page();

        }
         public async Task<IActionResult> OnPostEditClaimAsync(int? claimid)
        {
            if(claimid==null) return NotFound("Không tìm thấy user claim này");
            userclaim=_context.UserClaims.Where(c=>c.Id==claimid).FirstOrDefault();
            user=await _userManager.FindByIdAsync(userclaim.UserId);
            if(user==null) return NotFound("Không tìm thấy user này");

           if(!ModelState.IsValid)return Page();
           // kiểm tra trùng
           if(_context.UserClaims.Any
           (c=>c.UserId==user.Id
           &&c.ClaimType==Input.ClaimType
           &&c.ClaimValue==Input.ClaimValue
           &&c.Id!=userclaim.Id))
           {
              ModelState.AddModelError(string.Empty,"Claim này đã có");
           }
           userclaim.ClaimType=Input.ClaimType;
           userclaim.ClaimValue=Input.ClaimValue;
           await _context.SaveChangesAsync();

           StatusMessage="Bạn vừa cập nhật claim";
            return RedirectToPage("./AddRole", new {Id=user.Id});

        }
         public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            if(claimid==null) return NotFound("Không tìm thấy user claim này");
            userclaim=_context.UserClaims.Where(c=>c.Id==claimid).FirstOrDefault();
            user=await _userManager.FindByIdAsync(userclaim.UserId);
            if(user==null) return NotFound("Không tìm thấy user này");

           
           await _userManager.RemoveClaimAsync(user, new Claim(userclaim.ClaimType,userclaim.ClaimValue));

           StatusMessage="Bạn vừa xóa claim";
            return RedirectToPage("./AddRole", new {Id=user.Id});

        }
        public async Task<IActionResult> OnPostAddClaimAsync(string userid)
        {
            user=await _userManager.FindByIdAsync(userid);
            if(user==null) return NotFound("Không tìm thấy user này");

            if(!ModelState.IsValid) return Page();

             var claims =_context.UserClaims.Where(c=>c.UserId==user.Id);
             if(claims.Any(c=>c.ClaimType==Input.ClaimType&&c.ClaimValue==Input.ClaimValue))
             {
                     ModelState.AddModelError(string.Empty,"Đặc tính (claim)này đã có");

                     return Page();
             }
            await _userManager.AddClaimAsync(user, new Claim(
                Input.ClaimType, Input.ClaimValue));

                StatusMessage="Bạn vừa tạo claim riêng";
                return RedirectToPage("./AddRole", new {Id=user.Id});





        }
        
    }
}
