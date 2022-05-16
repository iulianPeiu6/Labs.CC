using Microsoft.EntityFrameworkCore;
using UScheduler.WebApi.Boards.Data.Entities;

namespace UScheduler.WebApi.Boards.Data
{
    public class BoardsContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }

        public BoardsContext(DbContextOptions options) : base(options)
        {
        }
    }
}
