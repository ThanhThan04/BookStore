namespace BookStore.Entity
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        // Navigation
        public virtual List<Book> Books { get; set; }
    }
}
