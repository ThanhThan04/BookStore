using BookStore.Cost;

namespace BookStore.Entity
{
    public class BorrowRecord
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? DurationDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public Enums.BorrowStatus Status { get; set; }

        // Navigation
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}
