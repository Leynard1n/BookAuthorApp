using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookAuthorApp.Models;

namespace BookAuthorApp.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private List<Author> authors = new List<Author>();
        private List<Book> books = new List<Book>(); // Добавьте это, если у вас есть книги

        public Task<IEnumerable<Author>> GetAllAsync() => Task.FromResult<IEnumerable<Author>>(authors);

        public Task<Author> GetByIdAsync(int id) => Task.FromResult(authors.FirstOrDefault(a => a.Id == id));

        public Task AddAsync(Author author)
        {
            author.Id = IdGenerator.GenerateAuthorId(); // Генерируем уникальный ID для автора
            authors.Add(author);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Author author)
        {
            var existingAuthor = authors.FirstOrDefault(a => a.Id == author.Id);
            if (existingAuthor != null)
            {
                existingAuthor.Name = author.Name;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var author = authors.FirstOrDefault(a => a.Id == id);
            if (author != null)
            {
                // Проверка на наличие связанных книг
                if (books.Any(b => b.AuthorId == id))
                {
                    throw new InvalidOperationException("Нельзя удалить автора, у которого есть связанные книги.");
                }
                authors.Remove(author);
            }
            return Task.CompletedTask;
        }
    }
}

