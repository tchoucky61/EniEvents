using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bo;

namespace Dal
{
    public class Context : DbContext, IDbContext
    {
        public Context(): base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Event> Events
        {
            get; set;
        }

        public DbSet<Picture> Pictures
        {
            get; set;
        }

        public DbSet<Thema> Themas
        {
            get; set;
        }

        public DbSet<Utilisateur> Utilisateurs
        {
            get; set;
        }
    }
}
