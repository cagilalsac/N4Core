using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Utilities;
using N4Manager.Contexts;
using N4Manager.Repositories;
using N4Manager.Services;
using N4Web.Data;
using N4Web.Settings;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

#region Culture
var cultureManager = new CultureManager();
builder.Services.Configure(cultureManager.AddCulture());
#endregion

// Add services to the container.
#region Connection String
var connectionString = builder.Configuration.GetConnectionString("Db");
#endregion

#region Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
	options.SignIn.RequireConfirmedAccount = false)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();
#endregion

#region Session
builder.Services.AddSession(config =>
{
	config.IdleTimeout = TimeSpan.FromMinutes(20);
});
#endregion

#region IoC Container
builder.Services.AddDbContext<Db>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(RepoBase<>), typeof(Repo<>));
builder.Services.AddScoped<RecordFileServiceBase, RecordFileService>();
// Inversion of Control for services:
#endregion

#region AppSettings
var appSettingsUtil = new AppSettingsUtil(builder.Configuration);
appSettingsUtil.Bind<AppSettings>();
#endregion

// Reference: https://github.com/DavidSuescunPelegay/jQuery-datatable-server-side-net-core
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
	options.SerializerSettings.Converters.Add(new StringEnumConverter());
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; // to prevent self referencing loop exception
});

var app = builder.Build();

#region Culture
app.UseRequestLocalization(cultureManager.UseCulture());
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

#region Authentication
app.UseAuthentication();
#endregion

app.UseAuthorization();

#region Session
app.UseSession();
#endregion

#region Area
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
#endregion

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
