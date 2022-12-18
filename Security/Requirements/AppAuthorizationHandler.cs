using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using App.Models;

namespace App.Security.Requirements
{
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
        private readonly ILogger<AppAuthorizationHandler> _logger;
        private readonly UserManager<AppUser> _userManager;
        public AppAuthorizationHandler(ILogger<AppAuthorizationHandler> logger, UserManager<AppUser> userManager)
        {
           _logger=logger;
           _userManager=userManager;
        }
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var requirements = context.PendingRequirements.ToList();
            _logger.LogInformation("context.Resource~ "+context.Resource?.GetType().Name);
            foreach (var requirement in requirements)
            {
                if(requirement is GenZRequirement)
                {
                    //code xử lý user đảm bảo
                   if (IsGenZ(context.User, (GenZRequirement)requirement))
                   {
                       context.Succeed(requirement);
                   }

                    //context.Succeed(requirement)
                }
                /* //if(requirement is OtherRequirement)
                {

                } */

                 if(requirement is ArticleUpdateRequirement)
                {
                    //code xử lý user đảm bảo
                    bool canUpdate=CanUpdateArticle(context.User, context.Resource, (ArticleUpdateRequirement)requirement);
                   if (canUpdate)
                   {
                       context.Succeed(requirement);
                   }

                    //context.Succeed(requirement)
                }
            }

            return Task.CompletedTask;
        }

        private bool CanUpdateArticle(ClaimsPrincipal user, object? resource, ArticleUpdateRequirement requirement)
        {
            if (user.IsInRole("Administrator")) 
            {
                _logger.LogInformation("Admin cập nhật...");
                return true;
            }

            var article= resource as Article;
            var datecreated= article.Created;
            var dateCanUpdate= new DateTime(requirement.Year,requirement.Month,requirement.Date);
            if(datecreated<dateCanUpdate) 
            {

                _logger.LogInformation("Quá ngày cập nhật");
                return false;

            }
            return true;
            

        }

        private bool IsGenZ(ClaimsPrincipal user, GenZRequirement requirement)
        {
            var appUserTask=_userManager.GetUserAsync(user);
            Task.WaitAll(appUserTask);
            var appUser=appUserTask.Result;
            if(appUser.BirthDate==null) 
            {
                _logger.LogInformation($"{appUser.UserName} không có ngày sinh, không thỏa mãn GenZRequirement");
                return false;
            }


            int year =appUser.BirthDate.Value.Year;
            var success= (year>=requirement.FromYear&&year<=requirement.ToYear);
            if(success)
            {
              _logger.LogInformation($"{appUser.UserName} thỏa mãn GenZRequirement với năm sinh là {year}");
            }
            else
            {
              _logger.LogInformation($"{appUser.UserName} không thỏa mãn GenZRequirement với năm sinh là {year}");
            }
         
            return success;
        }
    }
}