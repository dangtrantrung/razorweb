using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using razorweb.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using App.Servicces;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


// config Mailsetting

builder.Services.AddOptions();
var mailSetting=builder.Configuration.GetSection("MailSettings");

builder.Services.Configure<MailSettings>(mailSetting); // dI Mailsetting vao Service collection
//add Mail service
builder.Services.AddSingleton<IEmailSender,SendMailService>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<MyBlogContext>(options=>{

string connectstring=builder.Configuration.GetConnectionString("MyBlogContext");
options.UseSqlServer(connectstring);
});

//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    //.AddEntityFrameworkStores<MyBlogContext>();

//add identity 

 /* builder.Services.AddIdentity<AppUser,IdentityRole> ()
                .AddEntityFrameworkStores<MyBlogContext>()
                .AddDefaultTokenProviders();  */

            
            var identityservice = builder.Services.AddIdentity<AppUser, IdentityRole>();

// Thêm triển khai EF lưu trữ thông tin về Idetity (theo AppDbContext -> MS SQL Server).
identityservice.AddEntityFrameworkStores<MyBlogContext>();

// Thêm Token Provider - nó sử dụng để phát sinh token (reset password, confirm email ...)
// đổi email, số điện thoại ...
identityservice.AddDefaultTokenProviders(); 


// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (1); // Khóa 1 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lần thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount=true;

});

//cấu hình service=> authorize, login, logout, accessdenied

builder.Services.ConfigureApplicationCookie(options=>{
  options.LoginPath="/login/";
  options.LogoutPath="/logout/";
  options.AccessDeniedPath="/khongduoctruycap.html";
});

//Dang ky DI dich vụ Appservice AppIdentityErrorDescriber
builder.Services.AddSingleton<IdentityErrorDescriber,AppIdentityErrorDescriber>();

// Trên 30 giây truy cập lại sẽ nạp lại thông tin User (Role)
// SecurityStamp trong bảng User đổi -> nạp lại thông tin Security
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromSeconds(30);
});

// Policy authorization
builder.Services.AddAuthorization(options=>
{
    options.AddPolicy("AllowEditRole",policyBuilder=>
    {
            // Điều kiện của policy P1
            policyBuilder.RequireAuthenticatedUser();
            policyBuilder.RequireRole("Administrator");
            policyBuilder.RequireRole("Editor");
    });
      options.AddPolicy("Policy P2",policyBuilder=>
    {
            // Điều kiện của policy
            // Claims based authorization
       /* policyBuilder.RequireClaim("TenClaim", new string[]
       {
          "gia tri 1",
          "gia tri 2"
       }); */
    });

     /* IdentityRoleClaim<string> claims1;
     IdentityUserClaim<string> claims2;
     Claim claim; */

});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Identity
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


// them package Identity
//identity de xac dnh danh tinh -xac thuc authenticate user -login log out
// xac dinh role , authorization ,create, update, delete

// sau khi thiet lap, dang ky cac dich vu Identity co the su dung
//Identity/Account/Login
//Identity/Account/Manage...

// phat sinh cac trang Identity de customize

// tu netcore 6.0 thi netcore da tich hop san identity version --6.0.0

//dotnet aspnet-codegenerator identity -dc razorweb.models.MyBlogContext

// Role based authorization (vai trò)
//Role:admin,editor,manager,user members
//Tạo các trang quản lý roles: index,create,eidt,delete,..role
// dotnet new page -n Index -o Areas/Admin/Pages/Role -na App.Admin.Role
//dotnet tool install -g Microsoft.Web.LibraryManager.Cli

//Policy based authorization
//Claims based authorization

  // claims là đặc tính của đối  tượng vd bằng lái B2 có uyền lái xe, trên bằng lái có các đặc tính : ngày sinh, nơi sinh, hộ khẩu,...

  // policy {role, claims,...}

