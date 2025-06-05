using BookStore.Dtos.Category;
using BookStore.Dtos.Setting;

namespace BookStore.Services.Setting
{
    public interface ISettingService
    {
        Task<List<SettingDto>> GetAll();
        Task<SettingDto> GetById(Guid id);
        Task<Guid> Create(SettingCreate request);
        Task<Guid> Update(SettingUpdate id);
        Task<bool> Delete(Guid id);
    }
}
