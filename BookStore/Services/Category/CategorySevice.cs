using AutoMapper;
using BookStore.Dtos.Category;
using BookStore.Entity;
using BookStore.Repository;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services.Category
{
    public class CategorySevice : ICategoryService
    {
        private readonly IMapper _mapper;
        public readonly IRepository<Entity.Category> _caterepository;
        public CategorySevice(
            IMapper mapper,
            IRepository<Entity.Category> caterepository)
        {
            _mapper = mapper;
            _caterepository = caterepository;
        }

        public async Task<Guid> Create(CategoryCreate request)
        {
            var createcate = await _caterepository.FirstOrDefault(a => a.Name == request.Name);
            if (createcate != null)
            {
                throw new Exception("Đã có danh muc trong danhh sách!!");
            }
            var createcate2 = _mapper.Map<Entity.Category>(request);
            await _caterepository.CreateAsync(createcate2);
            return createcate2.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var delete = _caterepository.DeleteAsync(id);
            return true;

        }

        public async Task<CategoryDto> GetById(Guid id)
        {
            var getbyid = await _caterepository.GetAsync(id);
            return _mapper.Map<CategoryDto>(getbyid);
        }

        public async Task<Guid> Update(CategoryUpdate id)
        {
            var update = await _caterepository.GetAsync(id.Id);
            _mapper.Map(id, update);
            await _caterepository.UpdateAsync(update);
            return update.Id;
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            var getall = await _caterepository.AsQueryable().ToListAsync();
            return _mapper.Map<List<CategoryDto>>(getall);

        }
    }
}
