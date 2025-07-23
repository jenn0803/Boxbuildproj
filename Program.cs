using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BoxBuildproj.Data;
using BoxBuildproj.Areas.Identity.Data;
using BoxBuildproj.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Retrieve connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("BoxBuildprojContextConnection")
    ?? throw new InvalidOperationException("Connection string 'BoxBuildprojContextConnection' not found.");

// Register DbContext with SQL Server provider
builder.Services.AddDbContext<BoxBuildprojContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddScoped<CategoryService>();

// Register email service to resolve IEmailSender (Identity)
builder.Services.AddTransient<IEmailSender, EmailService>();

// Add Identity services and configure options
builder.Services.AddIdentity<BoxBuildprojUser, IdentityRole>(options =>
{
    // Set to false if you don't require email confirmation for login
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<BoxBuildprojContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // 👈 Path to your login page
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Optional
});



// Add HttpContextAccessor to access the logged-in user in services
builder.Services.AddHttpContextAccessor();

// Add Razor Pages and support Identity UI with Bootstrap
builder.Services.AddRazorPages();

// Add controllers for the application (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ensure roles and admin user are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<BoxBuildprojUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await CreateRolesAndAdminUser(roleManager, userManager);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication middleware is enabled
app.UseAuthorization();

// Map Razor Pages to support Identity pages
app.MapRazorPages();

// Set default route to HomeController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Home}/{id?}");

app.Run();

/// <summary>
/// Ensures that required roles exist and a default admin user is created.
/// </summary>
async Task CreateRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<BoxBuildprojUser> userManager)
{
    // Define required roles
    string[] roleNames = { "Admin", "User" };

    // Create roles if they do not exist
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Check if the admin user already exists
    string adminEmail = "admin@example.com";
    string adminPassword = "Admin@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        // Create admin user
        adminUser = new BoxBuildprojUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true // Set email confirmation to true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            // Assign the "Admin" role to the admin user
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
