using BookStore.Cost;

namespace BookStore.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }//
        public string Email { get; set; }//
        public string Phone { get; set; }//
        public string Password { get; set; }
        public Enums.UserRole Role { get; set; }
        public string Address { get; set; }

        // Navigation
        public virtual List<BorrowRecord> BorrowRecords { get; set; }
    }

}
