using book.Data.Models;
using book.Data.Paging;
using book.Data.ViewModels;
using book.Exceptions;
using System.Text.RegularExpressions;

namespace book.Data.Services
{
    public class PublishersService
    {
        private readonly AppDbContext _context;
        public PublishersService(AppDbContext context)
        {
            _context = context;
        }
        private bool stringStartWithNumber(string name) 
        {
            return Regex.IsMatch(name, @"^\d");    
        }
        public List<Publisher> GetAllPublishers(string? sortBy, string? searchString,int? pageNumber)
        {
            var allPublishers = _context.Publishers.OrderBy(n => n.Name).ToList();
            //sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        allPublishers = allPublishers.OrderByDescending(n => n.Name).ToList();
                        break;
                    default:
                        break;
                }
            }

            //searching
            if (!string.IsNullOrEmpty(searchString))
            {
                allPublishers = allPublishers.Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            // Paging
            int pageSize = 5;
            allPublishers = PaginatedList<Publisher>.Create(allPublishers.AsQueryable(), pageNumber ?? 1, pageSize);

            return allPublishers;
        }
        public Publisher AddPublisher(PublisherVM publisher)
        {
            if (stringStartWithNumber(publisher.Name))  throw new PublisherNameException("Name starts with  number",publisher.Name);

            var _publisher = new Publisher()
            {
                Name = publisher.Name
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();
            return _publisher;
        }
        public Publisher GetPublisherById(int id ) => _context.Publishers.FirstOrDefault( x => x.Id == id);
        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId) 
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId)
                .Select(n => new PublisherWithBooksAndAuthorsVM()
                {
                     Name = n.Name,
                     BookAuthors = n.Books.Select(n => new BookAuthorVM()
                     {
                         BookName = n.Title,
                         BookAuthors = n.Books_Authors
                         .Select(n => n.Author.FullName).ToList()
                     }).ToList()
                }).FirstOrDefault();
            return _publisherData;
        }

        public void DeletePublisherById(int id)
        {
            var _publisher = _context.Publishers.FirstOrDefault( n => n.Id == id);
            if (_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"The publisher with id: {id} does not exist");
            }
        }
    }
}
