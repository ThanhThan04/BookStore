using BookStore.Dtos.Book;
using BookStore.Dtos.BorrowRecord;
using BookStore.Dtos.Common;

namespace BookStore.Services.Borrow
{
    public interface IBorrowService
    {
        Task<List<BorrowRecordDto>> GetAll();
        Task<BorrowRecordDto> GetById(Guid id);
        Task<Guid> Update(BorrowUpdate id);
        Task<bool> Delete(Guid id);
        Task<Guid> Create(BorrowCreate id);
        Task<List<BorrowRecordDto>> GetByUser(Guid UserId);
        Task<Guid> RegisterBorrow(RegisterBorrow request);
        Task<BorrowRecordDto> ReturnBook(ReturnBook request);
        Task<decimal> TotalPrice(TotalPrice request);



        // Tìm kiếm theo tên kh , thể loại
        Task<PageView<BorrowRecordDto>>SearchUser(SearchUser request);



        //getpaging 
        //searchtext timtheo tenvkh , sach
        //tim theo ngay muon, ngay tra, ngay tra du kien
        // 
        //returnbook
        //ngay thuc te =ReturnDate-BorrowDate
        // ngay du kien   DurationDate - BorrowDate
        // nggay phat = ngay thuc te - ngay d kien 
        // if ngày phạt <0 thì ngày phạt =0 
        // total money = (ngay du kien * gia ngay )+ (ngay phat * gia phat)

    }
    
}
