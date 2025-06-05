using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BookStore.Entity
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Navigation
        public virtual List<Book> Books { get; set; }
    }
}
