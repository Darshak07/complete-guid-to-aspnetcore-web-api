using Microsoft.Identity.Client;
using my_books.Data.Models;
using my_books.Data.Paging;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System.Text.RegularExpressions;

namespace my_books.Data.Services
{
    public class PublisherService
    {
        private AppDbContext _context;
        public PublisherService(AppDbContext context)
        {
            _context = context;
        }
        public List<Publisher> GetAll() 
        {
            var allPublishers = _context.Publishers.OrderBy(n => n.Name).ToList();
            return allPublishers;
        }
        public List<Publisher> GetAllPublishers(string? sortBy,string? serchString,int? pageNumber)
        {
            //_context.Publishers.ToList();
            var allPublishers = _context.Publishers.OrderBy(n=>n.Name).ToList();

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
            if (!string.IsNullOrEmpty(serchString))
            {            
               allPublishers = allPublishers.Where(n=>n.Name.Contains(serchString,StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            //paging 
            int pageSize = 5;
            allPublishers = PaginatedList<Publisher>.Create(allPublishers.AsQueryable(), pageNumber ?? 1, pageSize);
            return allPublishers;
        }
        public Publisher AddPublisher(PublisherVM publisher)
        {
            if (stringStartsWithNumber(publisher.Name))
            {
                throw new PublisherNameException("Name Starts with number", publisher.Name);
            }
            var _publisher = new Publisher()
            {
                Name = publisher.Name
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();
            return _publisher;
        }

        public Publisher GetPublisherById(int id) => _context.Publishers.FirstOrDefault(n => n.Id == id);
        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
        { 
            var _publisherData = _context.Publishers.Where(n=>n.Id==publisherId)
                .Select(n => new PublisherWithBooksAndAuthorsVM() 
                { 
                    Name = n.Name,
                    BookAuthors = n.Books.Select(n=> new BookAuthorVM()
                    { 
                        BookName =  n.Title,
                        BookAuthors = n.Book_Authors.Select(n=>n.Author.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();
            return _publisherData;
        }

        public void DeletePublisherById(int id)
        {
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id==id);
            if (_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
            else 
            {
                throw new Exception($"The Publisher with id: {id} does not exist");
            }
        }

        private bool stringStartsWithNumber(string name)
        {
           return Regex.IsMatch(name, @"^\d");
        }
    }
}
