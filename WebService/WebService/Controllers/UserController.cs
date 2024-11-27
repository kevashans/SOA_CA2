using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebService.Data;

namespace WebService.Controllers;
/// <summary>
/// User related controllers
/// </summary>
[ApiController]
[Route("users")]
[Authorize]
public class UsersController : ControllerBase
{
	private readonly ApplicationDbContext _context;

	public UsersController(ApplicationDbContext context)
	{
		_context = context;
	}

	/// <summary>
	/// Get current user data
	/// </summary>
	/// <returns></returns>
	[HttpGet("me")]
	public async Task<IActionResult> GetCurrentUser()
	{
		string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (userId == null)
			return Unauthorized();

		var user = await _context.Users.FindAsync(userId);

		if (user == null)
			return NotFound(); 

		return Ok(user); 
	}
}
