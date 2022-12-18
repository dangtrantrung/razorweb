using Microsoft.AspNetCore.Mvc;
using App.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace App.Admin.Role
{
    [Authorize(Roles="Administrator")] 
    public class EditRoleClaimModel : RolePageModel
    {
         public EditRoleClaimModel(RoleManager<IdentityRole> roleManager, AppDbContext myBlogContext) : base(roleManager, myBlogContext)
        {

        }
        public IdentityRoleClaim<string> claim {get;set;}
        public IdentityRole role{get;set;}
        public async Task<IActionResult> OnGetAsync(int? claimid)
        {
            if(claimid==null) return NotFound("Không tìm thấy roleclaim");
            claim =_myBlogContext.RoleClaims.Where(c=>c.Id==claimid).FirstOrDefault();
            if(claim==null) return NotFound("Không tìm thấy roleclaim");
               role=await _roleManager.FindByIdAsync(claim.RoleId);
               if(role==null) return NotFound("Không tìm thấy role");
               Input =new InputModel()
               {
                   ClaimType =claim.ClaimType,
                   ClaimValue=claim.ClaimValue
               };
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



        public async Task<IActionResult> OnPostAsync(int? claimid)
        {
           if(claimid==null) return NotFound("Không tìm thấy roleclaim");
            claim =_myBlogContext.RoleClaims.Where(c=>c.Id==claimid).FirstOrDefault();
            if(claim==null) return NotFound("Không tìm thấy roleclaim");
               role=await _roleManager.FindByIdAsync(claim.RoleId);
               if(role==null) return NotFound("Không tìm thấy role");
                                            
              if(!ModelState.IsValid)
            {
                return Page();
            }
            if (_myBlogContext.RoleClaims.Any(c=>c.RoleId==role.Id&&c.ClaimType==Input.ClaimType&&c.ClaimValue==Input.ClaimValue))
            {
                     ModelState.AddModelError(string.Empty,"Claim này đã có trong role");
                     return Page();
            }
            claim.ClaimType=Input.ClaimType;
            claim.ClaimValue=Input.ClaimValue;
            var result= await _myBlogContext.SaveChangesAsync();
            
            
            StatusMessage ="Bạn vừa CHỈNH SỬA claim (đặc tính)";
            return RedirectToPage("./Edit", new {roleid=role.Id});
        }
        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
           if(claimid==null) return NotFound("Không tìm thấy roleclaim");
            claim =_myBlogContext.RoleClaims.Where(c=>c.Id==claimid).FirstOrDefault();
            if(claim==null) return NotFound("Không tìm thấy roleclaim");
               role=await _roleManager.FindByIdAsync(claim.RoleId);
               if(role==null) return NotFound("Không tìm thấy role");
                                            
            /*   if(!ModelState.IsValid)
            {
                return Page();
            } */// không cần thiết valid dữ liệu khi submit
            await _roleManager.RemoveClaimAsync(role,new Claim(claim.ClaimType,claim.ClaimValue));
           
            
            
            StatusMessage ="Bạn vừa xóa claim (đặc tính)";
            return RedirectToPage("./Edit", new {roleid=role.Id});
        }
    }
}
