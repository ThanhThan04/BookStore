namespace BookStore.Entity
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int PublishYear { get; set; }
        public int Quantity { get; set; }
        public string? ImgUrl {  get; set; }

        // Foreign keys
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }

        // Navigation
        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<BorrowRecord> BorrowRecords { get; set; }
    }
}
