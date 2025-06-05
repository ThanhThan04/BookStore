using BookStore.Dtos.Category;

namespace BookStore.Services.Category
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAll();
        Task<CategoryDto> GetById(Guid id);
        Task<Guid> Create(CategoryCreate request);
        Task<Guid> Update(CategoryUpdate id);
        Task<bool> Delete(Guid id);
    }
}
