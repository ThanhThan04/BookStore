namespace BookStore.Dtos.Book
{
    public class BookCreate
    {
        public string Title { get; set; }
        public int PublishYear { get; set; }
        public int Quantity { get; set; }
        
        // Foreign keys
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
