using Vinoteca.Abstractions;
using Vinoteca.Repositories;

namespace Vinoteca.Applications
{
    public interface IApplication<T> : IDbOperation<T>
    {
    }
    public class Application<T> : IApplication<T>
    {
        private IRepository<T> _repositorio;
        public Application(IRepository<T> repository)
        {
            _repositorio = repository;
        }

        public void Delete(int id)
        {
            _repositorio.Delete(id);
        }

        public IList<T> GetAll()
        {
            return _repositorio.GetAll();
        }

        public T GetById(int id)
        {
            return _repositorio.GetById(id);
        }

        public T Save(T entity)
        {
            return _repositorio.Save(entity);
        }
    }
}
