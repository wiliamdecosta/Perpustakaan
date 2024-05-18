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
        Book Create(BookRequest request);
        Book? Update(int id, BookRequest request, string updatedBy);
        List<string> Delete(DeleteRequest ids);
    }
}
