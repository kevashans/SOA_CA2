using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebService.Models.Auth;
using WebService.Models.Entities;

namespace WebService.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{

	public DbSet<ChatRoomEntity> ChatRooms { get; set; }
	public DbSet<MessageEntity> Messages { get; set; }
	public DbSet<SessionEntity> Sessions { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{ 
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}

}
