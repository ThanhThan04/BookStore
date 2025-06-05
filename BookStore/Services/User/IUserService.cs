using BookStore.Dtos.BorrowRecord;
using BookStore.Dtos.UserDto;
using BookStore.Entity;

namespace BookStore.Services.User
{
    public interface IUserService
    {
        Task<string> Register(UserRegister request);

        Task<string> Login(UserLogin request);
       
        Task<UserDtos> Profile();

        // Tạo tài khoản
        Task<Guid> Create(UserCreate request);
        Task<List<UserDtos>> GetAll();

        // Lấy danh sách sách đã từng mượn bởi người dùng
        Task<List<BorrowRecordDto>>GetBorrowHistoryAsync(Guid userId);
        Task<Guid> Update(UserUpdate request);

        Task<bool> Delete (Guid id);
    }
}
