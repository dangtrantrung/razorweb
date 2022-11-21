using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using razorweb.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;


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
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
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
