using Bo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class GenericRepository<T> : IRepository<T> where T : class, IIdentifiable
    {
        protected Context Context;
        protected DbSet<T> set;

        public Context GetContext()
        {
            return this.Context;
        }

        public DbSet<T> GetSet()
        {
            return set;
        }

        public GenericRepository(Context context)
        {
            Context = context;
            set = Context.Set<T>();
        }

        public void Delete(int id)
        {
            set.Remove(GetById(id));
            Context.SaveChanges();
        }

        public List<T> GetAll()
        {
            return set.ToList();
        }

        public List<T> GetAll(Func<T, bool> predicate)
        {
            return set.Where(predicate).ToList();
        }

        public T GetById(int id)
        {
            return set.Find(id);
        }

        public void Insert(T element)
        {
            set.Add(element);
            Context.SaveChanges();
        }

        public virtual void Update(T element)
        {
            Context.Entry(element).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void InsertTransact(T element)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                set.Add(element);
                Context.SaveChanges();

                //aurtes ajouts en bases
                transaction.Commit();
            }
        }
    }
}
