using BookStore.Dtos.AuthorDto;
using BookStore.Dtos.Setting;
using BookStore.Services.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
         private readonly ISettingService _settingService;

            public SettingController(ISettingService settingService)
            {
                _settingService = settingService;
            }

            [HttpGet("GetAll")]
            
            public async Task<IActionResult> GetAll()
            {
                var result = await _settingService.GetAll();
                return Ok(result);
            }

            [HttpGet("GetById/{id}")]
           
            public async Task<IActionResult> GetById(Guid id)
            {
                var result = await _settingService.GetById(id);
                return Ok(result);
            }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([FromBody] SettingCreate request)
        {
            try
            {
                var create = await _settingService.Create(request);
                return Ok(create);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(SettingUpdate id)
            {
               
                var existing = await _settingService.Update(id);
                return Ok (existing);
            }

            [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
            {
                var delete = await _settingService.Delete(id);
                await _settingService.Delete(id);
                return Ok(delete);
            }
        }
    }

