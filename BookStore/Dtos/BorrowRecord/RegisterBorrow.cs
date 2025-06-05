namespace BookStore.Dtos.BorrowRecord
{
    public class RegisterBorrow
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime DurationDate { get; set; }
    }
}
