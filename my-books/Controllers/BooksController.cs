using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Data.ViewModels.Authentication;

namespace my_books.Controllers
{
    [Authorize(Roles =UserRoles.Author + "," + UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public BooksService _booksService;

        public BooksController(BooksService booksService)
        { 
            _booksService = booksService;
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("get-all-books")]
        public IActionResult GetAllBooks()
        {
            var allBooks = _booksService.GetAllBooks();
            return Ok(allBooks);
        }
        [HttpGet("get-book-by-id/{id}")]
        public IActionResult GetBookByID(int id)
        {
            var book = _booksService.GetBookById(id);
            return Ok(book);
        }
        [HttpGet("get-book-by-id/{tital}/{publisher}")]
        public IActionResult GetBookByTital_Publisher(string tital,string publisher)
        {
            var book = _booksService.GetBookByTital_Publisher(tital,publisher);
            return Ok(book);
        }
        [HttpPost("add-book-with-authors")]
        public IActionResult AddBook([FromBody] BookVM book)
        { 
            _booksService.AddBookWithAuthors(book);
            return Ok();
        }
        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id,[FromBody] BookVM book)
        {
            var updatedBook = _booksService.UpadateBookById(id,book);
            return Ok(updatedBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            _booksService.DeleteBookById(id);
            return Ok();
        }
    }
}
