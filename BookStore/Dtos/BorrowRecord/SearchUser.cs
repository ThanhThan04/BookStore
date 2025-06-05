using BookStore.Dtos.Common;

namespace BookStore.Dtos.BorrowRecord
{
    public class SearchUser : GetPagingRequest
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? DurationDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
   
}
