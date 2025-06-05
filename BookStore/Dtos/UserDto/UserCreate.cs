using BookStore.Cost;

namespace BookStore.Dtos.UserDto
{
    public class UserCreate  : UserRegister
    {
     
        public Enums.UserRole Role { get; set; }
       
    }
}
