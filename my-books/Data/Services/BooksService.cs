using my_books.Data.Models;
using my_books.Data.ViewModels;
using System.Threading;

namespace my_books.Data.Services
{
    public class BooksService
    {
        private AppDbContext _context;
        public BooksService(AppDbContext context) 
        {
            _context = context;
        }

        public void AddBook(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = (bool)book.IsRead ? book.DateRead:null,
                Rate = (bool)book.IsRead ? book.Rate : null,
                Genre = book.Genre,
                Author = book.Author,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now
            };
            _context.Books.Add(_book);
            _context.SaveChanges();
        }

        //public List<Book> GetAllBooks()
        //{
        //    var allBooks = _context.Books.ToList();
        //    return allBooks;
        //}
        public List<Book> GetAllBooks() => _context.Books.ToList();

        public Book GetBookById(int bookID) => _context.Books.FirstOrDefault(n=>n.Id==bookID);

        public Book UpadateBookById(int bookId, BookVM book)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = (bool)book.IsRead ? book.DateRead : null;
                _book.Rate = (bool)book.IsRead ? book.Rate : null;
                _book.Genre = book.Genre;
                _book.Author = book.Author;
                _book.CoverUrl = book.CoverUrl;

                _context.SaveChanges();
            }
            return _book;
        }

        public void DeleteBookById(int bookId)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if ( _book != null ) 
            {
                _context.Books.Remove( _book );
                _context.SaveChanges();
            }
        }
    }
}
