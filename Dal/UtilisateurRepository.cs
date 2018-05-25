using Bo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class UtilisateurRepository : GenericRepository<Utilisateur>
    {
        public UtilisateurRepository(Context context) : base(context)
        {

        }

        public Utilisateur getByUserId(string applicationUserId = "")
        {
            IQueryable<Utilisateur> utilisateurQuery = set.Where(u => u.ApplicationUserId == applicationUserId);

            if (utilisateurQuery.Count() > 0)
            {
                return utilisateurQuery.ToList().First();
            }
            else
            {
                return new Utilisateur();
            }
        }
    }
}
