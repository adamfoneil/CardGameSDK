using AppService;
using AppService.Entities;
using BlazorApp;
using BlazorApp.Components;
using BlazorApp.Components.Account;
using BlazorApp.Extensions;
using Games.FoxInTheForest;
using Games.Hearts;
using HashidsNet;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<CurrentUser>();
builder.Services.Configure<HashIdOptions>(builder.Configuration.GetSection("HashIds"));

builder.Services.AddSingleton<IHashids>(sp =>
{
	var options = sp.GetRequiredService<IOptions<HashIdOptions>>().Value;
	return new Hashids(options.Salt, minHashLength: options.MinLength);
});
builder.Services.AddSingleton<HeartsGameFactory>();
builder.Services.AddSingleton<FoxInTheForestGameFactory>();
builder.Services.AddHostedService<EventBackgroundService>();
builder.Services.AddSingleton<EventRelay>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddRoleManager<RoleManager<IdentityRole>>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.MigrateDatabase<ApplicationDbContext>();

var app = builder.Build();

app.UseMigrationsEndPoint();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
