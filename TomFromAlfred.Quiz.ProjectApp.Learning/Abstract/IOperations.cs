using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Abstract
{
    public interface IOperations<T> //interfejs generyczny, zawiera elementarne metody
    {
        List<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
