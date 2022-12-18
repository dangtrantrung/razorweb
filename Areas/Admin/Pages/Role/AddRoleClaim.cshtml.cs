using Microsoft.AspNetCore.Mvc;
using App.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace App.Admin.Role
{
    [Authorize(Roles="Administrator")] 
    public class AddRoleClaimModel : RolePageModel
    {
         public AddRoleClaimModel(RoleManager<IdentityRole> roleManager, AppDbContext myBlogContext) : base(roleManager, myBlogContext)
        {

        }
        public IdentityRole role{get;set;}
        public async Task<IActionResult> OnGetAsync(string roleid)
        {
               role=await _roleManager.FindByIdAsync(roleid);
               if(role==null) return NotFound("Không tìm thấy role");
               return Page();
        }
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



        public async Task<IActionResult> OnPostAsync( string roleid)
        {
            role=await _roleManager.FindByIdAsync(roleid);
            if(role==null) return NotFound("Không tìm thấy role");
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if ((await _roleManager.GetClaimsAsync(role)).Any(c=>c.Type==Input.ClaimType&&c.Value==Input.ClaimValue))
            {
                     ModelState.AddModelError(string.Empty,"Claim này đã có trong role");
                     return Page();
            }
            var newclaim= new Claim(Input.ClaimType,Input.ClaimValue);
            var result=await _roleManager.AddClaimAsync(role,newclaim);
            if(!result.Succeeded)
            {
                result.Errors.ToList().ForEach(e=>
                {
                      ModelState.AddModelError(string.Empty,e.Description);
                });
                return Page();
            }
            StatusMessage ="Bạn vừa thêm claim (đặc tính) mới";
            return RedirectToPage("./Edit", new {roleid=role.Id});
        }
    }
}
