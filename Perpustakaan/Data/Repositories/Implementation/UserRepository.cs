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
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly FilterUtil<User> _filterUtil;

        public UserRepository(ApplicationDbContext db, IMapper mapper, FilterUtil<User> filterUtil)
        {
            _filterUtil = filterUtil;
            _db = db;
            _mapper = mapper;
        }

        public List<User> FetchAll()
        {
            var result =  _db.Users.Include(obj => obj.UserRole).ToList();
            var output = result;
            return result;
        }

        public Paginated<User> FetchAll(SearchRequest request)
        {
            List<string> searchKeywordsColumns = new List<string>();
            searchKeywordsColumns.Add("Name");
            searchKeywordsColumns.Add("Email");
            
            Paginated<User> query = _filterUtil.GetContent(request, searchKeywordsColumns);
            return query;
        }

        public User? FetchOne(int id)
        {
            return _db.Users.FirstOrDefault(x => x.Id == id);
        }

        public User? FetchByEmail(string email)
        {
            return _db.Users.FirstOrDefault(x => x.Email == email);
        }

        public User Register(UserRequest request)
        {
            User newUser = _mapper.Map<User>(request);
            newUser.CreatedDate = DateTime.Now;
            newUser.UpdatedDate = DateTime.Now;
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var createdUser = _db.Users.Add(newUser);
            _db.SaveChanges();

            return createdUser.Entity;
        }

        public User? Update(int id, UserRequest request)
        {
            User? user = _db.Users.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (user == null) return user;

            _mapper.Map<UserRequest, User>(request, user);
            user.UpdatedDate = DateTime.Now;

            _db.Users.Update(user);
            _db.SaveChanges();
            return user;
        }

        public List<string> Delete(DeleteRequest ids)
        {
            List<string> deletedIds = [];
            foreach (string userId in ids.Ids)
            {
                var user = _db.Users.FirstOrDefault(x => x.Id.ToString() == userId);
                if (user == null) continue;
                _db.Users.Remove(user);
                _db.SaveChanges();
                deletedIds.Add(userId);
            }
            return deletedIds;
        }

        public User Update(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
            return user;
        }

    }
}
