using AutoMapper;
using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using Perpustakaan.Data.Entities;
using Perpustakaan.Data.Repositories.Abstract;
using Perpustakaan.Models.Requests;
using Perpustakaan.Utils.Filters;
using Microsoft.EntityFrameworkCore;

namespace Perpustakaan.Data.Repositories.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly FilterUtil<Role> _filterUtil;

        public RoleRepository(ApplicationDbContext db, IMapper mapper, FilterUtil<Role> filterUtil)
        {
            _filterUtil = filterUtil;
            _db = db;
            _mapper = mapper;
        }

        public List<Role> FetchAll()
        {
            return _db.Roles.ToList();
        }

        public Paginated<Role> FetchAll(SearchRequest request)
        {
            List<string> searchKeywordsColumns = new List<string>();
            searchKeywordsColumns.Add("Name");

            Paginated<Role> query = _filterUtil.GetContent(request, searchKeywordsColumns);
            return query;
        }

        public Role? FetchOne(int id)
        {
            return _db.Roles.FirstOrDefault(x => x.Id == id);
        }

        public Role Create(RoleRequest request)
        {
            Role newRole = _mapper.Map<Role>(request);
            newRole.CreatedDate = DateTime.Now;
            newRole.UpdatedDate = DateTime.Now;

            var createdRole = _db.Roles.Add(newRole);
            _db.SaveChanges();

            return createdRole.Entity;
        }

        public Role? Update(int id, RoleRequest request)
        {
            Role? role = _db.Roles.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (role == null) return role;

            _mapper.Map<RoleRequest, Role>(request, role);
            role.UpdatedDate = DateTime.Now;

            _db.Roles.Update(role);
            _db.SaveChanges();
            return role;
        }

        public List<string> Delete(DeleteRequest ids)
        {
            List<string> deletedIds = [];
            foreach (string roleId in ids.Ids)
            {
                var role = _db.Roles.FirstOrDefault(x => x.Id.ToString() == roleId);
                if (role == null) continue;
                _db.Roles.Remove(role);
                _db.SaveChanges();
                deletedIds.Add(roleId);
            }
            return deletedIds;
        }
    }
}
