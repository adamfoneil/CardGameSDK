using AppService;
using AppService.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp;

public class CurrentUser(
	AuthenticationStateProvider authStateProvider,
	IDbContextFactory<ApplicationDbContext> dbFactory)
{
	private readonly AuthenticationStateProvider _authState = authStateProvider;
	private readonly IDbContextFactory<ApplicationDbContext> _dbFactory = dbFactory;

	private ApplicationUser? _currentUser;

	public async Task<ApplicationUser?> GetCurrentUserAsync()
	{
		if (_currentUser != null)
		{
			return _currentUser;
		}

		var authState = await _authState.GetAuthenticationStateAsync();
		var user = authState.User;

		if (user.Identity is not null && user.Identity.IsAuthenticated)
		{
			using var db = _dbFactory.CreateDbContext();
			_currentUser = await db.Users.FirstOrDefaultAsync(u => u.UserName == user.Identity.Name);
		}

		return _currentUser;
	}
}
