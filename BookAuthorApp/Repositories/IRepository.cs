
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookAuthorApp.Repositories { 
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(); // Получить всех
        Task<T> GetByIdAsync(int id); // Получить по ID
        Task AddAsync(T item); // Добавить
        Task UpdateAsync(T item); // Обновить
        Task DeleteAsync(int id); // Удалить
    }
}
