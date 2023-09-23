using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Data.ViewModels.Authentication;
using my_books.Exceptions;
using Serilog;
namespace my_books.Controllers
{
    [Authorize(Roles = UserRoles.Publisher+","+UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublisherService _publishersService;
        
        public PublishersController(PublisherService publishersService) 
        {
            _publishersService = publishersService;
         
           
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var allinfor = _publishersService.GetAll();
            Log.Information("Publisher Get All Information {@allinfor}", allinfor);
            return Ok(allinfor);
        }
        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers(string? sortBy,string? searchString,int? pageNumber)
        {
            try
            {
                var _result = _publishersService.GetAllPublishers(sortBy, searchString, pageNumber);
                return Ok(_result);
            }
            catch (Exception)
            {
                return BadRequest("Sorry, we could not load the publishers {}");
            }
        }
        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody]PublisherVM publisher)
        {
            try
            {
                var newpublisher = _publishersService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newpublisher);
            }
            catch (PublisherNameException ex)
            { 
                return BadRequest($"{ex.Message}, Publisher name: {ex.PublisherName}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }    
        }

        [HttpGet("get-publisher-books-with-authors/{id}")]
        public IActionResult GetPublisherData(int id)
        {
            var _response = _publishersService.GetPublisherData(id);
            return Ok(_response);
        }
        [HttpGet("get-publisher-by-id/{id}")]
        public ActionResult<Publisher> GetPublisherById(int id)
        {
          
            var _response = _publishersService.GetPublisherById(id);
            

            if (_response!= null)
            {
                return _response;
            }
            else
            { 
                return NotFound(); 
            }
        }
        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id) 
        {
           
            try {
                _publishersService.DeletePublisherById(id);
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
