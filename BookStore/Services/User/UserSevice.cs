using AutoMapper;
using BookStore.Dtos.BorrowRecord;
using BookStore.Dtos.UserDto;
using BookStore.Entity;
using BookStore.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Services.User
{
    public class UserSevice : IUserService
    {
        private readonly IRepository<Entity.User> _rpUserRepository; // Gọi thẳng hành động trên bảng Users
        private readonly IMapper _mapper; // mapper từ DTO sang Entity, ....
        private readonly IPasswordHasher<Entity.User> _passwordHasher; // Băm mật khẩu của người dùng
        private readonly IHttpContextAccessor _httpContextAccessor; // Lấy thông tin người dùng từ token
        private readonly IConfiguration _configuaration; // Lấy thông tin trong appsettings.json
        private readonly IRepository<BorrowRecord> _rpBorrowRecord;


        public UserSevice(
            IRepository<Entity.User> rpUser,
            IMapper mapper,
            IPasswordHasher<Entity.User> passwordHasher,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
             IRepository<BorrowRecord> rpBorrowRecord


            )
        {
            _rpBorrowRecord = rpBorrowRecord;
            _rpUserRepository = rpUser;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _configuaration = configuration;

        }
        public async Task<Guid> Create(UserCreate request)
        {
            var userExist = await _rpUserRepository.FirstOrDefault(u => u.Email == request.Email);
            if (userExist != null)
            {
                throw new Exception("Email đã tồn tại");
            }

            var entity = _mapper.Map<Entity.User>(request);
            // thiếu mật khẩu, => tự băm mật khẩu và tự lưu vào entity
            entity.Password = _passwordHasher.HashPassword(entity, request.Password);

            var result = await _rpUserRepository.CreateAsync(entity);

            return result.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _rpUserRepository.GetAsync(id);

            await _rpUserRepository.DeleteAsync(id);

            return true;
        }

        public async Task<List<UserDtos>> GetAll()
        {
            var users = await _rpUserRepository.GetAllAsync();
            return _mapper.Map<List<UserDtos>>(users);
        }

        
        public async Task<List<BorrowRecordDto>> GetBorrowHistoryAsync(Guid userId)
        {
            var result = await _rpBorrowRecord.AsQueryable()
                .Include(b=>b.User)
                .Include(b=>b.Book)
                .Where(b=>b.UserId==userId)
                .ToListAsync();

            return _mapper.Map<List<BorrowRecordDto>>(result);
        }

        public async Task<string> Login(UserLogin request)
        {
            var user = await _rpUserRepository.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }

            // So sánh mật khẩu người dùng với mật khẩu trong CSDL
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            // Có trạng thái là Failed, nếu mật khẩu không đúng
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Mât khẩu không đúng");
            }

            return GenerateToken(user);

            }
        public async Task<UserDtos> Profile()
            {
            // Chỉ cần có token ở header
            // lấy thông tin từ token => decode JWT => cho dữ liệu
            // HTTContextAccessor: lấy thông tin từ token
            // String => Guid: Chuyển từ string sang Guid dùng Guid.Parse
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.First(u => u.Type == "Id").Value);

            //Tìm thông tin user theo Id đã lấy từ token
            var user = await _rpUserRepository.GetAsync(userId); // Tái sử dụng Repo: Generic Repository

            // UserEntity => Mapper

            return _mapper.Map<UserDtos>(user);
        }

            public async Task<string> Register(UserRegister request)
            {
            var userExist = await _rpUserRepository.FirstOrDefault(u => u.Email == request.Email);
            if (userExist != null)
            {
                throw new Exception("Email đã tồn tại");
            }

            var userExistphone = await _rpUserRepository.FirstOrDefault(u => u.Phone == request.Phone);
            if (userExistphone != null)
            {
                throw new Exception("So dien thoai đã tồn tại");
            }
            //Mapper tu UserRegisterRequest sang UserEntity (đã đinh nghĩa mapper chưa???)

            var entity = _mapper.Map<Entity.User>(request);
            // thiếu mật khẩu, => tự băm mật khẩu và tự lưu vào entity
            entity.Password = _passwordHasher.HashPassword(entity, request.Password);

            var result = await _rpUserRepository.CreateAsync(entity);

            return GenerateToken(result);
        }

        public async Task<Guid> Update(UserUpdate request)
        {
            var userupdateExist = await _rpUserRepository.GetAsync(request.Id);

            // Mapper sang UserEntity
            _mapper.Map(request, userupdateExist);

            await _rpUserRepository.UpdateAsync(userupdateExist);

            // Cập nhaatj thanhf coong
            return userupdateExist.Id;
        }

        private string GenerateToken(Entity.User user)
        {
            var jwtSettings = _configuaration.GetSection("JwtSettings");


            var claims = new[]
            {
                // Không hiện thị, hệ thống Authorize sẽ chỉ đọc có ClaimType.Role
                new Claim(ClaimTypes.Role, user.Role.ToString()),  // Không public
                new Claim("Role", user.Role.ToString()),
                new Claim("Name", user.FullName),
                new Claim("Email", user.Email),
                new Claim("phone",user.Phone),
                new Claim("address",user.Address),
                new Claim("Id", user.Id.ToString()), // Lấy Id Claim. Type == Id => String => Guid
            };

            var key = new Microsoft.IdentityModel.Tokens
                .SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"]));
            var creds = new Microsoft.IdentityModel.Tokens
                .SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
