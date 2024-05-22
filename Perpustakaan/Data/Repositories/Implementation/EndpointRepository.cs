using AutoMapper;
using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using Perpustakaan.Data.Entities;
using Perpustakaan.Data.Repositories.Abstract;
using Perpustakaan.Models.Requests;
using Perpustakaan.Utils.Filters;
using Microsoft.EntityFrameworkCore;
using Perpustakaan.Configurations.Db;

namespace Perpustakaan.Data.Repositories.Implementation
{
    public class EndpointRepository : IEndpoint
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly FilterUtil<EndpointPath> _filterUtil;

        public EndpointRepository(ApplicationDbContext db, IMapper mapper, FilterUtil<EndpointPath> filterUtil)
        {
            _filterUtil = filterUtil;
            _db = db;
            _mapper = mapper;
        }

        public List<EndpointPath> FetchAll()
        {
            return _db.Endpoints.ToList();
        }

        public Paginated<EndpointPath> FetchAll(SearchRequest request)
        {
            List<string> searchKeywordsColumns = new List<string>();
            searchKeywordsColumns.Add("PathRoute");
            searchKeywordsColumns.Add("Method");
            searchKeywordsColumns.Add("Description");

            Paginated<EndpointPath> query = _filterUtil.GetContent(request, searchKeywordsColumns);
            return query;
        }

        public EndpointPath? FetchOne(int id)
        {
            return _db.Endpoints.FirstOrDefault(x => x.Id == id);
        }

        public EndpointPath Create(EndpointRequest request)
        {
            EndpointPath newPath = _mapper.Map<EndpointPath>(request);
            newPath.CreatedDate = DateTime.Now;
            newPath.UpdatedDate = DateTime.Now;

            var createdPath = _db.Endpoints.Add(newPath);
            _db.SaveChanges();

            return createdPath.Entity;
        }

        public EndpointPath? Update(int id, EndpointRequest request)
        {
            EndpointPath? endpoint = _db.Endpoints.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (endpoint == null) return endpoint;

            _mapper.Map<EndpointRequest, EndpointPath>(request, endpoint);
            endpoint.UpdatedDate = DateTime.Now;

            _db.Endpoints.Update(endpoint);
            _db.SaveChanges();
            return endpoint;
        }

        public List<string> Delete(DeleteRequest ids)
        {
            List<string> deletedIds = [];
            foreach (string endpointId in ids.Ids)
            {
                var endpoint = _db.Endpoints.FirstOrDefault(x => x.Id.ToString() == endpointId);
                if (endpoint == null) continue;
                _db.Endpoints.Remove(endpoint);
                _db.SaveChanges();
                deletedIds.Add(endpointId);
            }
            return deletedIds;
        }

    }
}
