using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using Perpustakaan.Data.Entities;
using Perpustakaan.Models.Requests;

namespace Perpustakaan.Data.Repositories.Abstract
{
    public interface IEndpoint
    {
        List<EndpointPath> FetchAll();
        Paginated<EndpointPath> FetchAll(SearchRequest request);
        EndpointPath? FetchOne(int id);
        EndpointPath Create(EndpointRequest request);
        EndpointPath? Update(int id, EndpointRequest request);
        List<string> Delete(DeleteRequest ids);
    }
}
