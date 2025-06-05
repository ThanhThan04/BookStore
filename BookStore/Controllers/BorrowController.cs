using BookStore.Dtos.BorrowRecord;
using BookStore.Services.Borrow;
using BookStore.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        private readonly ISmtp _smtp;


        public BorrowController(
            IBorrowService borrowService,
            ISmtp smtp)
        {
            _borrowService = borrowService;
            _smtp = smtp;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterBorrow([FromBody] RegisterBorrow request)
        {
            var result = await _borrowService.RegisterBorrow(request);
            return Ok(result);
        }

        [HttpGet("Mail")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Smtp([FromQuery] string toEmail, string subject, string body, bool isHtml = false)
        {
             await _smtp.SendEmailAsync(toEmail, subject, body, isHtml);
            return Ok("Ok");
        }


        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var result = await _borrowService.GetAll();
            return Ok(result);
        }


        [HttpGet("GetById/{id}")]

        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _borrowService.GetById(id);
            return Ok(result);
        }


        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _borrowService.Delete(id);
            return Ok(result);
        }


        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] BorrowUpdate request)
        {
            var result = await _borrowService.Update(request);
            return Ok(result);
        }


        [HttpGet("User/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var result = await _borrowService.GetByUser(userId);
            return Ok(result);
        }


        [HttpPost("Search")]
        public async Task<IActionResult> SearchUser([FromBody] SearchUser request)
        {
            var result = await _borrowService.SearchUser(request);
            return Ok(result);
        }


        [HttpPost("Return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBook request)
        {
            var result = await _borrowService.ReturnBook(request);
            return Ok(result);
        }


        [HttpPost("TotalPrice")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TotalPrice([FromBody] TotalPrice request)
        {
            var result = await _borrowService.TotalPrice(request);
            return Ok(result);
        }
    }
}
