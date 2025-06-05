using BookStore.Dtos.Book;
using BookStore.Services.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class BookController : ControllerBase
        {
            private readonly IBookService _bookService;

            public BookController(IBookService bookService)
            {
                _bookService = bookService;
            }

            [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] BookCreate request)
            {
                try
                {
                    var id = await _bookService.Create(request);
                    return Ok( id);
                }
                catch (Exception ex)
                {
                    return BadRequest( ex.Message);
                }
            }

            [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookUpdate request)
            {
                if (id != request.Id)
                    return BadRequest("Khong tim thay ID");

                var updatedId = await _bookService.Update(request);
                return Ok (updatedId);
            }

            [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
            {
                var result = await _bookService.Delete(id);
                return Ok(result);
            }

            [HttpGet("GetAll")]
       
        public async Task<IActionResult> GetAll()
            {
                var books = await _bookService.GetAll();
                return Ok(books);
            }

            [HttpGet("Get/{id}")]
       
        public async Task<IActionResult> GetById(Guid id)
            {
                var book = await _bookService.GetById(id);
                return Ok(book);
            }

            [HttpPost("Search")]
            public async Task<IActionResult> Search([FromBody] SearchBook request)
            {
                var result = await _bookService.SearchBook(request);
                return Ok(result);
            }
        [HttpPost("upload-image")]
        

        public async Task<IActionResult> UploadImage(IFormFile file, [FromQuery] Guid BookId)
        {
            try
            {
                var url = await _bookService.UploadBookImageAsync(file, BookId);
                return Ok(new { imageUrl = url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
    }


