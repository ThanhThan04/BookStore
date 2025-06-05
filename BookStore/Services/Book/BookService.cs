using AutoMapper;
using BookStore.Dtos.Book;
using BookStore.Dtos.Category;
using BookStore.Dtos.Common;
using BookStore.Repository;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services.Book
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        public readonly IRepository<Entity.Book> _bookrepository;

        public BookService(
            IMapper mapper,
            IRepository<Entity.Book> bookrepository
            )
        {
            _mapper = mapper;
            _bookrepository = bookrepository;
        }
        public async Task<Guid> Create(BookCreate request)
        {
            var create = await _bookrepository.FirstOrDefault(a => a.Title== request.Title);
            if (create != null)
            {
                throw new Exception("Sách đã tồn tại");
            }
            var create2 = _mapper.Map<Entity.Book>(request);
            await _bookrepository.CreateAsync(create2);
            return create2.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var delete = _bookrepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<BookDto>> GetAll()
        {
            var getall = await _bookrepository.AsQueryable().ToListAsync();
            return _mapper.Map<List<BookDto>>(getall);
        }

        public async Task<BookDto> GetById(Guid id)
        {
            var getbyid = await _bookrepository.GetAsync(id);
            return _mapper.Map<BookDto>(getbyid);
        }

        public Task<PageView<BookDto>> SearchBook(SearchBook request)
        {

            //tim theo ten va danh muc
            IQueryable<Entity.Book> query= _bookrepository.AsQueryable().Include(t=>t.Author).Include(t=>t.Category);
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(t=>t.Title.ToLower().Contains(request.SearchText.ToLower()) 
                || t.Author.Name.ToLower().Contains(request.SearchText.ToLower())
                || t.Category.Name.ToLower().Contains(request.SearchText.ToLower()));
            }


            // bo loc theo nam va ten danh muc
            if(request.PublishYear.HasValue)
            {
                query = query.Where(n => n.PublishYear == request.PublishYear);
            }
            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                query = query.Where(t => t.Category.Name.ToLower().Contains(request.CategoryName.ToLower()));
            }
            var total = query.Count();


            // phan trang 
            if (request.PageIndex.HasValue && request.PageSize.HasValue)
            {
                query= query.Skip((request.PageIndex.Value -1)*request.PageSize.Value).Take(request.PageSize.Value);
            }
            return Task.FromResult(new PageView<BookDto>
            {
                TotalRecord = total,
                Items = _mapper.Map<List<BookDto>>(query.ToList())
            });
        }

        public async Task<Guid> Update(BookUpdate request)
        {
            var update = await _bookrepository.GetAsync(request.Id);
            _mapper.Map(request, update);
            await _bookrepository.UpdateAsync(update);
            return update.Id;
        }

        public async Task<string?> UploadBookImageAsync(IFormFile imageFile, Guid BookId)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new Exception("File không hợp lệ");

            // Tìm product theo Id
            var product = await _bookrepository.GetAsync(BookId);
            if (product == null)
                throw new Exception("Không tìm thấy sản phẩm");

            // Upload lên Cloudinary
            var account = new Account("dmmukvwvi", "595564171248616", "8U0ZRAJe75pAOwPsRpxIAsMG38g");
            var cloudinary = new Cloudinary(account);

            await using var stream = imageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = "productImage"
            };

            var result = cloudinary.Upload(uploadParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Upload lỗi: {result.Error?.Message}");

            // Cập nhật URL vào sản phẩm
            product.ImgUrl = result.SecureUri.ToString();
            await _bookrepository.UpdateAsync(product);

            return product.ImgUrl;
        }
    }
}
