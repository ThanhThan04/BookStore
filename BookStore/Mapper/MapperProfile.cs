using AutoMapper;
using BookStore.Cost;
using BookStore.Dtos.AuthorDto;
using BookStore.Dtos.Book;
using BookStore.Dtos.BorrowRecord;
using BookStore.Dtos.Category;
using BookStore.Dtos.Setting;
using BookStore.Dtos.UserDto;
using BookStore.Entity;

namespace BookStore.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            //Mapper tử UserRegisterRequet sang userEntity

            CreateMap<UserRegister, Entity.User>()
                .ForMember(dest => dest.Role, o => o.MapFrom(src => Enums.UserRole.User))
                // => Không mapper mật khẩu => Ignore
                .ForMember(dest => dest.Password, o => o.Ignore());

            // Chuyển dữ liệu từ đối tượng UserEnity => UserDto
            CreateMap<Entity.User, UserDtos>();

            CreateMap<UserCreate, Entity.User>();
            CreateMap<UserUpdate, Entity.User>();


            CreateMap<Entity.BorrowRecord, BorrowRecordDto>()
                .ForMember(dest => dest.FullName, o => o.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.BookName, o => o.MapFrom(src => src.Book.Title));



            // chuyển dữ liệu AuthorEntity sang => authorDto
            CreateMap<Entity.Author, AuthorDtos>();
            CreateMap<AuthorCreate, Entity.Author>();
            CreateMap<AuthorUpdate, Entity.Author>();


            // chuyển dữ liệu categoryEntity sang => categoryDto
            CreateMap<Entity.Category,CategoryDto>();
            CreateMap<CategoryCreate, Entity.Category>();
            CreateMap<CategoryUpdate, Entity.Category>();

            // chuyển dữ liệu BookEntity sang => bookDto
            CreateMap<Entity.Book, BookDto>()
                .ForMember(dest=>dest.AuthorName , o=>o.MapFrom(t=>t.Author != null ? t.Author.Name:""))
                .ForMember(dest => dest.CategoryName, o => o.MapFrom(t => t.Category != null ? t.Category.Name : ""))
                .ForMember(dest => dest.ImgUrl, o => o.MapFrom(t =>t.ImgUrl ));
            CreateMap<BookCreate, Entity.Book>();
            CreateMap<BookUpdate, Entity.Book>();



            // chuyển dữ liệu SettingEntity sang => settingDto
            CreateMap<Entity.Setting, SettingDto>();
            CreateMap<SettingCreate, Entity.Setting>();
            CreateMap<SettingUpdate, Entity.Setting>();

            // chuyển dữ liệu BorrowEntity sang => BorrowDto
            CreateMap<Entity.BorrowRecord, BorrowRecordDto>()
                .ForMember(dest => dest.FullName, o => o.MapFrom(t => t.User != null ? t.User.FullName : ""))
            .ForMember(dest => dest.BookName, o => o.MapFrom(t => t.Book != null ? t.Book.Title : ""));
            CreateMap<BorrowCreate, Entity.BorrowRecord>()
                .ForMember(dest=>dest.BorrowDate, o=>o.MapFrom(t=>DateTime.Now))
                .ForMember(dest=>dest.Status, o=>o.MapFrom(t=>Enums.BorrowStatus.Borrowed)); 
            CreateMap<BookUpdate, Entity.BorrowRecord>();
            
           

            // chuyển dữ liệu  sang => BorrowDto
            CreateMap<RegisterBorrow, Entity.BorrowRecord>();

        }
    }
}
