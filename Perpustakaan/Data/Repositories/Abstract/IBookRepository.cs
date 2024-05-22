using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using Perpustakaan.Data.Entities;
using Perpustakaan.Models.Requests;

namespace Perpustakaan.Data.Repositories.Abstract
{
    public interface IBookRepository
    {
        List<Book> FetchAll();
        Paginated<Book> FetchAll(SearchRequest request);
        Book? FetchOne(int id);
        Task<Book> CreateAsync(BookRequest request);
        Book? Update(int id, BookRequest request);
        List<string> Delete(DeleteRequest ids);
    }
}
