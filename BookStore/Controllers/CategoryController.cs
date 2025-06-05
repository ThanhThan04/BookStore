using BookStore.Dtos.AuthorDto;
using BookStore.Dtos.Category;
using BookStore.Services.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryCreate request)
        {
                var userId = await _categoryService.Create(request);
                return Ok(userId);
        }
        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var Getall = await _categoryService.GetAll();
            return Ok(Getall);
        }
        [HttpGet("Get/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
                var result = await _categoryService.GetById(id);
                return Ok(result);
        }
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
                await _categoryService.Delete(id);
                return Ok(id); 
        }
        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(CategoryUpdate id)
        {       
                var result = await _categoryService.Update(id);
                return Ok(result);   
        }

    }
}
