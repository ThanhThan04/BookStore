using BookStore.Dtos.Common;

namespace BookStore.Dtos.Book
{
    public class SearchBook:GetPagingRequest
    {
        public int? PublishYear { get; set; }
        public string? CategoryName { get; set; }
    }
}
