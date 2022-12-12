using Microsoft.AspNetCore.Mvc;
using razorweb.models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace App.Admin.Role
{
    [Authorize(Roles="Administrator")] 
    public class CreateModel : RolePageModel
    {
         public CreateModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {

        }
        public void OnGet()
        {
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

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var newrole= new IdentityRole(Input.Name);
            var result= await _roleManager.CreateAsync(newrole);
             if(result.Succeeded)
             {
                 StatusMessage=$"Bạn vừa tạo thành công role mới {Input.Name}";
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
