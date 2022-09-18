using Projeto_Final.Models;

namespace Projeto_Final.Data
{
    public class PlayerContext : DbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
    }
}
