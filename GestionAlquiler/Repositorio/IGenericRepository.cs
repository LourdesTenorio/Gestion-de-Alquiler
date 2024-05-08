namespace GestionAlquiler.Repositorio
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> Lista();
        Task<Boolean> Actualizar(T entity);
        Task<bool> Guardar(T modelo);
        Task<bool> Editar(T modelo);
        Task<bool> Eliminar(int id);

    }
}
