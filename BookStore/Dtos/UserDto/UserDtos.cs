using BookStore.Cost;

namespace BookStore.Dtos.UserDto
{
    public class UserDtos
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        public Enums.UserRole Role { get; set; }
        public string Address { get; set; }
    }
}
