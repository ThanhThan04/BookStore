using AutoMapper;
using BookStore.Dtos.AuthorDto;
using BookStore.Dtos.Setting;
using BookStore.Entity;
using BookStore.Repository;

namespace BookStore.Services.Setting
{
    public class SettingService : ISettingService
    {

        private readonly IMapper _mapper;
        public readonly IRepository<Entity.Setting> _settingrepository;
        public SettingService(
            IMapper mapper,
            IRepository<Entity.Setting>  repository)
        {
            _mapper = mapper;
            _settingrepository = repository;

        }
        public async Task<Guid> Create(SettingCreate request)
        {
            //var create = await _settingrepository.FirstOrDefault(a => a.Key == request.Key);
            //if (create != null)
            //{
            //    throw new Exception("Đã có trong danhh sách!!");
            //}
            var create2 = _mapper.Map<Entity.Setting>(request);
            await _settingrepository.CreateAsync(create2);
            return create2.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var author = _settingrepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<SettingDto>> GetAll()
        {
            var  Getall = await _settingrepository.GetAllAsync();
            return _mapper.Map<List<SettingDto>>(Getall);
        }

        public async Task<SettingDto> GetById(Guid id)
        {
            var getbyid = await _settingrepository.GetAsync(id);
            return _mapper.Map<SettingDto>(getbyid);
        }

        public async Task<Guid> Update(SettingUpdate id)
        {
            var update = await _settingrepository.GetAsync(id.Id);
            _mapper.Map(id, update);
            await _settingrepository.UpdateAsync(update);
            return update.Id;
        }
    }
}
