using AutoMapper;
using BookStore.Data;
using BookStore.Repository;
using BookStore.Services.Author;
using BookStore.Services.Book;
using BookStore.Services.Borrow;
using BookStore.Services.Category;
using BookStore.Services.Setting;
using BookStore.Services.User;
using BookStore.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategorySevice>();
builder.Services.AddScoped<IUserService, UserSevice>();
builder.Services.AddScoped<IBorrowService, Borrowsevice>();
builder.Services.AddScoped<IPasswordHasher<BookStore.Entity.User>, PasswordHasher<BookStore.Entity.User>>();
builder.Services.AddHttpContextAccessor();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()   // Cho phép tất cả domain
                .AllowAnyMethod()   // Cho phép tất cả phương thức (GET, POST, PUT, DELETE...)
                .AllowAnyHeader();  // Cho phép tất cả headers
        });
});

// Add Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Automapper => Hệ thống có thể gọi và sử dụng
builder.Services.AddAutoMapper(typeof(Program));



//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//});


//smtp
builder.Services.Configure<SmtpSetting>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<ISmtp, Smtp>();

// JWT config
var jwtSetting = builder.Configuration.GetSection("JwtSettings"); // Gọi config của JWT
var key = Encoding.ASCII.GetBytes(jwtSetting["Secret"]);

// Xác thực
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(option =>
{
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSetting["Issuer"],
        ValidAudience = jwtSetting["Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(); // Phân quyền

// Cấu hình Swagger UI để nó nhận được JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "BookStore",
            Version = "v1"
        }
    );
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập 'Bearer {Token}'"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


//CORS config
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()   // Cho phép tất cả domain
                .AllowAnyMethod()   // Cho phép tất cả phương thức (GET, POST, PUT, DELETE...)
                .AllowAnyHeader();  // Cho phép tất cả headers
        });
});

builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Profile));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
