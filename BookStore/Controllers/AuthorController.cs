using BookStore.Dtos.AuthorDto;
using BookStore.Dtos.UserDto;
using BookStore.Services.Author;
using BookStore.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([FromBody] AuthorCreate request)
        {
            try
            {
                var userId = await _authorService.Create(request);
                return Ok(userId);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAll()
        {
            var author = await _authorService.GetAll();
            return Ok(author);
        }
        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        // => /update
        public async Task<IActionResult> Update(AuthorUpdate request)
        {
            try
            {
                var result = await _authorService.Update(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Get/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _authorService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(Guid id)
        {
                await _authorService.Delete(id);
                return Ok(id);
        }
    }
}
