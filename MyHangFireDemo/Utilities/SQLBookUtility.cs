using Microsoft.EntityFrameworkCore;
using MyHangFireDemo.Helpers;
using MyHangFireDemo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHangFireDemo.Utilitities
{
    public interface ISQLBookUtility
    {
        public Task<List<SQLBooksModel>> GetNewBookAsync();
        public Task UpdateNewBookAsync(int bookId, SQLBooksModel bookModel);
    }
    public class SQLBookUtility : ISQLBookUtility
    {
        private readonly SQLDataContext _context;

        public SQLBookUtility(SQLDataContext context)
        {
            _context = context;
        }
        // SQL Server Services Implementation - Started
        public async Task<List<SQLBooksModel>> GetNewBookAsync()
        {
            var newBook = await _context.Books.Where(x => x.NewStatus == 1).Select(x => new SQLBooksModel()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Category = x.Category,
                Author = x.Author,
                NewStatus = x.NewStatus,
                BookOrigin = x.BookOrigin
            }).ToListAsync();

            return newBook;
        }
        public async Task UpdateNewBookAsync(int bookId, SQLBooksModel bookModel)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.NewStatus = 0;
                _context.Books.Update(book);
            }

            await _context.SaveChangesAsync();
        }
        // SQL Server Services Implementation - End
    }
}
