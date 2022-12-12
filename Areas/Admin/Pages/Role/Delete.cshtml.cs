using Microsoft.AspNetCore.Mvc;
using razorweb.models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace App.Admin.Role
{
     [Authorize(Roles="Administrator")]
    public class DeleteModel : RolePageModel
    {
         public DeleteModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {

        }
        public IdentityRole role{get;set;}
        public string RoleName{get;set;}
        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if(roleid==null) return NotFound("Không tìm thấy role");
            var role=await _roleManager.FindByIdAsync(roleid);
            
            if(role==null)
            {
               return NotFound("Không tìm thấy role");
                
            }
           RoleName=role.Name;
           return Page();
        }
       

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
             if(roleid==null) return NotFound("Không tìm thấy role");
            role=await _roleManager.FindByIdAsync(roleid);
            if(role==null) 
            {
                return NotFound("Không tìm thấy role");
            }
            
            RoleName=role.Name;
            var result =await _roleManager.DeleteAsync(role);
            
             if(result.Succeeded)
             {
                 StatusMessage=$"Bạn vừa xóa thành công role {RoleName}";
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
