using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using Perpustakaan.Data.Entities;
using Perpustakaan.Models.Requests;

namespace Perpustakaan.Data.Repositories.Abstract
{
    public interface IUserRepository
    {
        List<User> FetchAll();
        Paginated<User> FetchAll(SearchRequest request);
        User? FetchOne(int id);
        User? FetchByEmail(string email);
        User Register(UserRequest request);
        User? Update(int id, UserRequest request);
        List<string> Delete(DeleteRequest ids);
        User? Update(User user);
    }
}
