using BookStore.Dtos.UserDto;
using BookStore.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Đăng ký tài khoản
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister request)
        {
            try
            {
                var token = await _userService.Register(request);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Đăng nhập tài khoản
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin request)
        {
            try
            {
                var token = await _userService.Login(request);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("profile")]
        [Authorize] // Bắt buộc phải xác thực 
        public async Task<IActionResult> Profile()
        {
            try
            {
                var user = await _userService.Profile();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }


        [HttpDelete("Delete{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.Delete(id);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("borrowhistory/{userId}")]
        public async Task<IActionResult> GetBorrowHistory(Guid userId)
        {
            var history = await _userService.GetBorrowHistoryAsync(userId);
            return Ok(history);
        }
        [HttpPut("update")] 

        public async Task<IActionResult> Update(UserUpdate request)
        {
            try
            {
                var result = await _userService.Update(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Create")]
       
        public async Task<IActionResult> Create([FromBody] UserCreate request)
        {
            try
            {
                var userId = await _userService.Create(request);
                return Ok(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
