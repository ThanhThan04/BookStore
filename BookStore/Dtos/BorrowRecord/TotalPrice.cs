namespace BookStore.Dtos.BorrowRecord
{
    public class TotalPrice
    {
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? DurationDate { get; set; }
    }
}
