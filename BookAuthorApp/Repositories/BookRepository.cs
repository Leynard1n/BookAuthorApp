using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookAuthorApp.Models;

namespace BookAuthorApp.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private List<Book> books = new List<Book>();

        public Task<IEnumerable<Book>> GetAllAsync() => Task.FromResult<IEnumerable<Book>>(books);

        public Task<IEnumerable<Book>> GetAllByAuthorIdAsync(int authorId) =>
            Task.FromResult(books.Where(b => b.AuthorId == authorId));

        public Task<Book> GetByIdAsync(int id) => Task.FromResult(books.FirstOrDefault(b => b.Id == id));

        public Task AddAsync(Book book)
        {
            book.Id = IdGenerator.GenerateBookId(); // Генерируем уникальный ID для книги
            books.Add(book);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Book book)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.AuthorId = book.AuthorId;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
            }
            return Task.CompletedTask;
        }
    }
}

