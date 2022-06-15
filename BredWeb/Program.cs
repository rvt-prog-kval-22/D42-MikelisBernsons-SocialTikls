using BredWeb.Data;
using BredWeb.Interfaces;
using BredWeb.Models;
using BredWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddIdentity<Person, IdentityRole>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("OldConnection")
    ));
}
else
{
    var secretClient = new SecretClient(new Uri(Environment.GetEnvironmentVariable("VaultUri")), new DefaultAzureCredential());
    KeyVaultSecret defaultConnection = await secretClient.GetSecretAsync("DefaultConnection");

    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    defaultConnection.Value.ToString()
    ));
}

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
