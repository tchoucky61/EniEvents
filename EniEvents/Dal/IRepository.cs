using Bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public interface IRepository<T> where T : IIdentifiable
    {
        List<T> GetAll();
        void Insert(T element);
        T GetById(int id);
        void Update(T element);
        void Delete(int id);
    }
}
