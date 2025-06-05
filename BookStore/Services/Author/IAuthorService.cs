using BookStore.Dtos.AuthorDto;
using BookStore.Entity;

namespace BookStore.Services.Author
{
    public interface  IAuthorService
    {
        // Lấy tất cả tác giả
        Task<List<AuthorDtos>> GetAll();

        // Lấy chi tiết tác giả theo ID
        Task<AuthorDtos> GetById(Guid id);

        // Thêm tác giả mới
        Task<Guid>Create(AuthorCreate author);

        // Cập nhật tác giả
        Task<Guid> Update(AuthorUpdate id);

        // Xóa tác giả (nếu không liên kết với sách)
        Task<bool> Delete(Guid id);

    }
}
