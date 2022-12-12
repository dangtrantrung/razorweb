using Microsoft.AspNetCore.Mvc;
using razorweb.models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace App.Admin.Role
{
    [Authorize(Roles="Administrator")]  
    public class EditModel : RolePageModel
    {
         public EditModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {

        }
        public IdentityRole role{get;set;}
        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if(roleid==null) return NotFound("Không tìm thấy role");
            var role=await _roleManager.FindByIdAsync(roleid);
            if(role!=null)
            {
                Input= new InputModel()
                {
                    Name=role.Name
                };
                return Page();
            }
           return NotFound("Không tìm thấy role");
        }
        //class con tạo role
        public class InputModel
        {
           [Display(Name="Tên của role")]
           [Required(ErrorMessage="Phải nhập {0}")]
           [StringLength(256,MinimumLength=3,ErrorMessage="{0} phải dài từ {2} đến {1} ký tự")]
           public string Name{get;set;}

        }
        [BindProperty] // khi ng dùng submit nó sẽ bind form value vào property input này
        public InputModel Input{get;set;}

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
             if(roleid==null) return NotFound("Không tìm thấy role");
            var role=await _roleManager.FindByIdAsync(roleid);
            if(role==null) return NotFound("Không tìm thấy role");
            if(!ModelState.IsValid)
            {
                return Page();
            }
            role.Name=Input.Name;
            var result =await _roleManager.UpdateAsync(role);
            
             if(result.Succeeded)
             {
                 StatusMessage=$"Bạn vừa cập nhật thành công role {Input.Name}";
                 return RedirectToPage("./Index");
             }
             else{
                 result.Errors.ToList().ForEach(error=>{
                     ModelState.AddModelError(string.Empty,error.Description);
                 });
             }
              return Page();

            
        }
    }
}
