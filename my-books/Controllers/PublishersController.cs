using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModels;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublisherService _publishersService;

        public PublishersController(PublisherService publishersService) 
        {
            _publishersService = publishersService;
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody]PublisherVM publisher)
        {
            _publishersService.AddPublisher(publisher);
            return Ok();
        }
    }
}
