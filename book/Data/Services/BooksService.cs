using book.Data.Models;
using book.Data.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;

namespace book.Data.Services
{
    public class BooksService
    {
        private readonly AppDbContext _context;
        public BooksService(AppDbContext context)
        {
            _context = context;
        }
        public void AddBookWithAuthors(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId
            };
            _context.Books.Add(_book);  
            _context.SaveChanges();
            foreach (var id in book.AuthorIds)
            {
                var _book_Author = new Book_Author()
                {
                    BookId = _book.Id,
                    AuthorId = id
                };
                _context.Books_Authors.Add(_book_Author);
                _context.SaveChanges();
            }
        }
        public List<Book> GetAllBooks() => _context.Books.ToList();
        public BookWithAuthoresVM GetBookById(int bookId)
        {
            var _bookWithAuthores = _context.Books
                .Where(n => n.Id == bookId)
                .Select(book => new BookWithAuthoresVM()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Books_Authors
                .Select(n => n.Author.FullName).ToList()
            }).FirstOrDefault();
            return _bookWithAuthores;
        }
        public Book UpdateBookById(int BookId ,BookVM book)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == BookId);
            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Genre = book.Genre;
                _book.CoverUrl = book.CoverUrl;
                _context.SaveChanges();
            }
            return _book;
        }
        public void DeleteBookById(int BookId)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == BookId);
            if (_book != null)
            {
                _context.Books.Remove( _book );
                _context.SaveChanges();
            }
        }
    }
}
