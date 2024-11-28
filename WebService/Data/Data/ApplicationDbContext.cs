using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Data.Auth;
using Data.Entities;
using Domain.Entities;

namespace Data;

public class ApplicationDbContext : IdentityDbContext<User>
{

	public DbSet<ChatRoom> ChatRooms { get; set; }
	public DbSet<Message> Messages { get; set; }
	public DbSet<Session> Sessions { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{ 
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}

}
