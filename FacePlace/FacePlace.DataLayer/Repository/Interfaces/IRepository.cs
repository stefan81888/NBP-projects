using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePlace.DataLayer.Repository.Interfaces
{
    public interface IRepository<Type>
    {
        Type Get(string identifier);
        Type Create(Type typeInstance);
        void Delete(string identifier);
        Type Update(Type typeInstance);
        List<Type> GetAll();
    }
}
