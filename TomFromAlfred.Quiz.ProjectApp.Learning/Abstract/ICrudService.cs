using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Abstract
{
    public interface ICrudService<T> where T : class // Interfejs dla operacji Crud
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAllActive(); // Elastyczny interfejs, elementy są przetwarzane,
                                 // kiedy faktycznie są potrzebne tzw. leniwe przetwarzanie
    }
}
