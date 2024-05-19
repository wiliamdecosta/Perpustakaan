using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using Perpustakaan.Data.Entities;
using Perpustakaan.Models.Requests;

namespace Perpustakaan.Data.Repositories.Abstract
{
    public interface IRoleRepository
    {
        List<Role> FetchAll();
        Paginated<Role> FetchAll(SearchRequest request);
        Role? FetchOne(int id);
        Role Create(RoleRequest request);
        Role? Update(int id, RoleRequest request);
        List<string> Delete(DeleteRequest ids);
    }
}
