using BookStore.Cost;
using BookStore.Entity;

namespace BookStore.Dtos.BorrowRecord
{
    public class BorrowRecordDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string FullName { get; set; } 
      
        public Guid BookId { get; set; }
        public string BookName { get; set; } 
        public DateTime BorrowDate { get; set; }
        public DateTime DurationDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Enums.BorrowStatus Status { get; set; }

      
    }
}
