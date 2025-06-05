namespace BookStore.Dtos.Book
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int PublishYear { get; set; }
        public int Quantity { get; set; }
        public string? ImgUrl { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
    }
}
