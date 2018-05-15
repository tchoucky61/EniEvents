using Bo;
using System.Data.Entity;

namespace Dal
{
    public interface IDbContext
    {
        DbSet<Event> Events { get; set; }
        DbSet<Utilisateur> Users { get; set; }
        DbSet<Thema> Themas { get; set; }
        DbSet<Picture> Pictures { get; set; }
    }
}