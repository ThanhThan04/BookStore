using AutoMapper;
using Azure.Core;
using BookStore.Dtos.AuthorDto;
using BookStore.Entity;
using BookStore.Repository;

namespace BookStore.Services.Author
{
    public class AuthorService : IAuthorService

    {
        private readonly IMapper _mapper;
        public readonly IRepository<Entity.Author> _authorrepository;

        public AuthorService(
            IMapper mapper,
            IRepository<Entity.Author> repository)
        {
            _mapper = mapper;
            _authorrepository = repository;
        }


        public async Task<Guid> Create(AuthorCreate author)
        {
            //var createauthor = _authorrepository.FirstOrDefault(a => a.Bio == author.Bio);
            //if (createauthor != null)
            //{
            //    throw new Exception("Đã có tác giả trong danhh sách!!");
            //}
            var createauthor2 = _mapper.Map<Entity.Author>(author);

                // map tay 
            //{
            //    Id = Guid.NewGuid(),
            //    Name = author.Name,
            //    Bio = author.Bio,
            //};
            await _authorrepository.CreateAsync(createauthor2);
            return createauthor2.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var author = _authorrepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<AuthorDtos>> GetAll()
        {
            var authors = await _authorrepository.GetAllAsync();
            return _mapper.Map<List<AuthorDtos>>(authors);
        }

        public async Task<AuthorDtos> GetById(Guid id)
        {
           var getbyid = await _authorrepository.GetAsync(id);
           return _mapper.Map<AuthorDtos>(getbyid);
        }

        public async Task<Guid> Update(AuthorUpdate id)
        {
            var update = await _authorrepository.GetAsync(id.Id);
            _mapper.Map(id, update);
            await _authorrepository.UpdateAsync(update);
            return update.Id;

        }
    }
}
