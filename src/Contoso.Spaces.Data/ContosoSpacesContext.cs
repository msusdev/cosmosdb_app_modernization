using Contoso.Spaces.Models;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Spaces.Data
{
    public class ContosoSpacesContext : DbContext
    {
        public ContosoSpacesContext(DbContextOptions<ContosoSpacesContext> options)
            : base(options)
        { }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Cart> Carts { get; set; }
    }
}