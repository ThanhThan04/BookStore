using AutoMapper;
using Azure.Core;
using BookStore.Cost;
using BookStore.Dtos.Book;
using BookStore.Dtos.BorrowRecord;
using BookStore.Dtos.Common;
using BookStore.Entity;
using BookStore.Repository;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services.Borrow
{
    public class Borrowsevice : IBorrowService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Entity.BorrowRecord> _borrowrepository;
        private readonly IRepository<Entity.Setting> _Setting;
        private readonly IRepository<Entity.User> _userRepository;
        private readonly IRepository<Entity.Book> _bookRepository;
        public Borrowsevice(
            IMapper mapper,
            IRepository<Entity.BorrowRecord> borrowrepository,
            IRepository<Entity.Setting> setting,
            IRepository<Entity.User>user,
            IRepository<Entity.Book> book )
            
        {
            _mapper = mapper;
            _borrowrepository = borrowrepository;
            _Setting = setting;
            _userRepository = user;
            _bookRepository = book;
        }
        public async Task<Guid> Create(BorrowCreate id)
        {
            var create = _borrowrepository.FirstOrDefault(a => a.UserId==id.UserId);
            if (create != null)
            {
                throw new Exception("Nguoi muon da co");
            }
            var create2 = _mapper.Map<Entity.BorrowRecord>(id);
            await _borrowrepository.CreateAsync(create2);
            return create2.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var delete = _borrowrepository.DeleteAsync(id);
            return true;
        }

        public async Task<BorrowRecordDto> GetById(Guid id)
        {
            var getbyid = await _borrowrepository.GetAsync(id);
            return _mapper.Map<BorrowRecordDto>(getbyid);
        }

        public async Task<List<BorrowRecordDto>> GetAll()
        {
            var getall = await _borrowrepository.AsQueryable().ToListAsync();
            return _mapper.Map<List<BorrowRecordDto>>(getall);
        }

        public async Task<List<BorrowRecordDto>> GetByUser(Guid UserId)
        {
            var getUser= await _borrowrepository.GetAsync(UserId);
            var borrowRecordDtos = _mapper.Map<List<BorrowRecordDto>>(getUser);
            return borrowRecordDtos;
        }

        public async Task<Guid> RegisterBorrow(RegisterBorrow request)
        {
           
            var user = await _userRepository.GetAsync(request.UserId);// kiem tra nguoi dung co ton tai khay khong
            if (user == null)
            {
                throw new Exception("chua co nguoi dung .");
            }
            //// tim tra sach
            //var book = await _bookRepository.GetAsync(request.BookId);
            //if (book == null)
            //{
            //    throw new Exception("khong co sach.");
            //}

            // tim sach
            var borrowedRecord = await _borrowrepository
             .FirstOrDefault(b => b.BookId == request.BookId && b.ReturnDate == null);

            if (borrowedRecord != null)
            {
                throw new Exception("sach dang muon.");
            }
            var borrowRecord = _mapper.Map<BorrowRecord>(request);
            borrowRecord.Id = Guid.NewGuid();
            borrowRecord.BorrowDate = DateTime.Now;
            borrowRecord.ReturnDate = null;
            await _borrowrepository.CreateAsync(borrowRecord);
            return borrowRecord.Id;
        }


        public async Task<Guid> Update(BorrowUpdate id)
        {
            var update = await _borrowrepository.GetAsync(id.Id);
            _mapper.Map(id, update);

            if (id.Status == Enums.BorrowStatus.Returned)
            {
                var price= await _Setting.FirstOrDefault(t=>t.Key=="PriceToDay");
                var tax = await _Setting.FirstOrDefault(t => t.Key == "Tax");

                var priceNumber = price != null ? int.Parse(price.Value) : 5000;
                var taxNumber = tax != null ? int.Parse(tax.Value) : 2000;

                
                var tinhtien = (update.DurationDate - update.BorrowDate)
                    .Value.TotalDays * priceNumber + (id.ReturnDate - update.DurationDate)
                    .Value.TotalDays * taxNumber;
                update.TotalPrice = Convert.ToDecimal(tinhtien); //// chuyen doi duoble thanh decimal
            }
            await _borrowrepository.UpdateAsync(update);
            return update.Id;
        }

        public async Task<BorrowRecordDto> ReturnBook(ReturnBook request)
        {
            var record = await _borrowrepository.GetAsync(request.Id);
            if (record == null)
            {
                throw new Exception("nguoi dung khong muon.");
            }

            record.ReturnDate = request.ReturnDate;
            record.Status = Enums.BorrowStatus.Returned;

            var priceSetting = await _Setting.FirstOrDefault(t => t.Key == "PriceToDay");
            var taxSetting = await _Setting.FirstOrDefault(t => t.Key == "Tax");

            var price = priceSetting != null ? int.Parse(priceSetting.Value) : 5000;
            var tax = taxSetting != null ? int.Parse(taxSetting.Value) : 2000;

            double borrowDays = (record.DurationDate - record.BorrowDate)?.TotalDays ?? 0;
            double lateDays = (record.ReturnDate - record.DurationDate)?.TotalDays ?? 0;
            lateDays = lateDays > 0 ? lateDays : 0;

            var total = (borrowDays * price) + (lateDays * tax);
           
            
            
            string subject = "Thông báo quá hạn mượn sách từ <b>Book Store</b>";
            string body = $@"
                    Xin chào {record.User.FullName},<br><br>

                    Bạn đã mượn sách tại<b>Book Store</b>, và hiện tại một hoặc nhiều cuốn sách đã <b>quá hạn trả</b> theo quy định.<br><br>

                    Vui lòng kiểm tra lại tài khoản của bạn và hoàn trả sách trong thời gian sớm nhất để tránh phát sinh phí phạt hoặc bị hạn chế quyền mượn sách.<br><br>

                    Nếu bạn cần hỗ trợ thêm, đừng ngần ngại liên hệ với chúng tôi qua email hoặc hotline:00000000000000.<br><br>

                    Trân trọng,<br>
                   <b>Book Store</b>";


            record.TotalPrice = Convert.ToDecimal(total);

            await _borrowrepository.UpdateAsync(record);

            return _mapper.Map<BorrowRecordDto>(record);
        }


        public async Task<decimal> TotalPrice(TotalPrice request)
        {
           
            var priceSetting = await _Setting.FirstOrDefault(t => t.Key == "PriceToDay");
            var taxSetting = await _Setting.FirstOrDefault(t => t.Key == "Tax");

            var price = priceSetting != null ? int.Parse(priceSetting.Value) : 5000;
            var tax = taxSetting != null ? int.Parse(taxSetting.Value) : 2000;

            var totalDays = (request.ReturnDate - request.BorrowDate)?.TotalDays;
            var lateDays = (request.ReturnDate - request.DurationDate)?.TotalDays;
            lateDays = lateDays > 0 ? lateDays : 0;

            var total = (totalDays * price) + (lateDays * tax);

            return Convert.ToDecimal(total);
        }



        public Task<PageView<BorrowRecordDto>> SearchUser(SearchUser request)
        {
            IQueryable<Entity.BorrowRecord> query = _borrowrepository.AsQueryable().Include(t => t.User).Include(t => t.Book);
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(t => t.User.FullName.ToLower().Contains(request.SearchText.ToLower())
                || t.User.FullName.ToLower().Contains(request.SearchText.ToLower())
                || t.Book.Title.ToLower().Contains(request.SearchText.ToLower()));
            }

            if (request.BorrowDate.HasValue)
            {
                query = query.Where(t => t.BorrowDate.Date == request.BorrowDate.Value.Date);
            }

            if (request.ReturnDate.HasValue)
            {
                query = query.Where(t => t.ReturnDate.HasValue && t.ReturnDate.Value.Date == request.ReturnDate.Value.Date);
                                         
            }

            if (request.DurationDate.HasValue)
            {
                query = query.Where(t => t.DurationDate.HasValue && t.DurationDate.Value.Date == request.DurationDate.Value.Date);
                                        
            }
            return Task.FromResult(new PageView<BorrowRecordDto>
            {
                Items = _mapper.Map<List<BorrowRecordDto>>(query.ToList())
            });
        }
    }
}
