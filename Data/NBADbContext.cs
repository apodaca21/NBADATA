using Microsoft.EntityFrameworkCore;
using NBADATA.Models;

namespace NBADATA.Data;

public class NBADbContext : DbContext
{
	public NBADbContext(DbContextOptions<NBADbContext> options) : base(options)
	{
	}

	public DbSet<Player> Players { get; set; } = null!;
}
