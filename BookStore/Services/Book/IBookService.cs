using BookStore.Dtos.Book;
using BookStore.Dtos.Common;

namespace BookStore.Services.Book
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAll();
        Task<BookDto> GetById(Guid id);
        Task<Guid> Create(BookCreate request);
        Task<Guid> Update(BookUpdate request);
        Task<bool> Delete(Guid id);

        // Tìm kiếm theo tên sách, tên tác giả, thể loại
        Task<PageView<BookDto>> SearchBook(SearchBook request);

        Task<string?> UploadBookImageAsync(IFormFile imageFile, Guid BookId);
    }
}
