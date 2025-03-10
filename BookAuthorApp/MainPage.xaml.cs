using System.Collections.ObjectModel;
using BookAuthorApp.Models;
using BookAuthorApp.Repositories;

namespace BookAuthorApp
{
    public partial class MainPage : ContentPage
    {
        private AuthorRepository _authorRepository = new AuthorRepository();
        private BookRepository _bookRepository = new BookRepository();

        public ObservableCollection<Author> Authors { get; set; }
        public ObservableCollection<Book> Books { get; set; }

        private Author _currentAuthor;
        private Book _currentBook;

        public MainPage()
        {
            InitializeComponent();
            Authors = new ObservableCollection<Author>();
            Books = new ObservableCollection<Book>();
            AuthorsListView.ItemsSource = Authors;
            BooksListView.ItemsSource = Books;
            LoadAuthors();
        }

        private async void LoadAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();
            Authors.Clear();
            foreach (var author in authors)
            {
                author.Books = (await _bookRepository.GetAllByAuthorIdAsync(author.Id)).ToList();
                Authors.Add(author);
            }
        }

        private async void LoadBooks(int authorId)
        {
            // Получаем книги как IEnumerable<Book> и преобразуем в List<Book>
            var books = (await _bookRepository.GetAllByAuthorIdAsync(authorId)).ToList();
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        }

        private async void OnAddAuthorClicked(object sender, EventArgs e)
        {
            var newAuthor = new Author { Name = AuthorNameEntry.Text };
            await _authorRepository.AddAsync(newAuthor);
            LoadAuthors();
            AuthorNameEntry.Text = string.Empty; // Очищаем поле ввода
        }

        private async void OnUpdateAuthorClicked(object sender, EventArgs e)
        {
            if (_currentAuthor != null)
            {
                _currentAuthor.Name = AuthorNameEntry.Text;
                await _authorRepository.UpdateAsync(_currentAuthor);
                LoadAuthors();
                _currentAuthor = null; // Сбрасываем текущего автора
                AuthorNameEntry.Text = string.Empty; // Очищаем поле ввода
            }
        }

        private async void OnDeleteAuthorClicked(object sender, EventArgs e)
        {
            if (_currentAuthor != null)
            {
                await _authorRepository.DeleteAsync(_currentAuthor.Id);
                LoadAuthors();
                _currentAuthor = null; // Сбрасываем текущего автора
                AuthorNameEntry.Text = string.Empty; // Очищаем поле ввода
            }
        }

        private async void OnAuthorTapped(object sender, EventArgs e)
        {
            var selectedCell = sender as TextCell;
            var selectedAuthor = selectedCell?.BindingContext as Author;

            if (selectedAuthor != null)
            {
                _currentAuthor = selectedAuthor;
                AuthorNameEntry.Text = selectedAuthor.Name; // Заполняем поле для редактирования
                LoadBooks(selectedAuthor.Id); // Загружаем книги автора
            }
        }

        private async void OnAddBookClicked(object sender, EventArgs e)
        {
            if (_currentAuthor != null)
            {
                var newBook = new Book { Title = BookTitleEntry.Text, AuthorId = _currentAuthor.Id };
                await _bookRepository.AddAsync(newBook);
                LoadBooks(_currentAuthor.Id);
                BookTitleEntry.Text = string.Empty; // Очищаем поле ввода
            }
        }

        private async void OnUpdateBookClicked(object sender, EventArgs e)
        {
            if (_currentBook != null)
            {
                _currentBook.Title = BookTitleEntry.Text;
                await _bookRepository.UpdateAsync(_currentBook);
                LoadBooks(_currentAuthor.Id);
                _currentBook = null; // Сбрасываем текущую книгу
                BookTitleEntry.Text = string.Empty; // Очищаем поле ввода
            }
        }

        private async void OnDeleteBookClicked(object sender, EventArgs e)
        {
            if (_currentBook != null)
            {
                await _bookRepository.DeleteAsync(_currentBook.Id);
                LoadBooks(_currentAuthor.Id);
                _currentBook = null; // Сбрасываем текущую книгу
                BookTitleEntry.Text = string.Empty; // Очищаем поле ввода
            }
        }

        private async void OnBookTapped(object sender, EventArgs e)
        {
            var selectedCell = sender as TextCell;
            var selectedBook = selectedCell?.BindingContext as Book;

            if (selectedBook != null)
            {
                _currentBook = selectedBook;
                BookTitleEntry.Text = selectedBook.Title; // Заполняем поле для редактирования
            }
        }

        private void OnCancelEditClicked(object sender, EventArgs e)
        {
            // Логика для обработки нажатия кнопки "Отмена"
            AuthorNameEntry.Text = string.Empty; // Очищаем поле ввода имени автора
            BookTitleEntry.Text = string.Empty; // Очищаем поле ввода названия книги
            _currentAuthor = null; // Сбрасываем текущего автора
            _currentBook = null; // Сбрасываем текущую книгу
        }
    }
}

