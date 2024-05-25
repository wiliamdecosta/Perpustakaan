using Azure.Core;
using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using JustclickCoreModules.Validators;
using Perpustakaan.Data.Entities;
using Perpustakaan.Data.Repositories.Implementation;
using Perpustakaan.Models.Requests;
using Perpustakaan.Models.Responses;
using Perpustakaan.Utils;

namespace Perpustakaan.Services
{
    public class UserService
    {
        private readonly RequestValidator _validator;
        private readonly UserRepository _repository;

        public UserService(RequestValidator validator, UserRepository repository)
        {
            _validator = validator;
            _repository = repository;
        }

        public List<User> FetchAll()
        {
            return _repository.FetchAll();
        }

        public Paginated<User> FetchAll(SearchRequest searchRequest)
        {
            return _repository.FetchAll(searchRequest);
        }

        public User FetchOne(int id)
        {
            User? user = _repository.FetchOne(id);
            if (user == null)
            {
                throw new InvalidRequestValueException(null, "ID_NOT_FOUND");
            }
            return user;
        }

        public User? FetchByEmail(string email)
        {
            User? user = _repository.FetchByEmail(email);
            if (user == null)
            {
                throw new InvalidRequestValueException(null, "EMAIL_NOT_FOUND");
            }
            return user;
        }

        public User Register(UserRequest request)
        {

            if (!_validator.Validate(request))
            {
                throw new InvalidRequestValueException(_validator.Errors);
            }


            User user = _repository.Register(request);
            return user;
        }

        public User Update(int id, UserRequest request)
        {
            if (!_validator.Validate(request))
            {
                throw new InvalidRequestValueException(_validator.Errors);
            }

            User? user = _repository.Update(id, request);
            if (user == null)
            {
                throw new InvalidRequestValueException(null, "INVALID_ID");
            }

            return user;
        }

        public List<string> Delete(DeleteRequest request)
        {
            List<string> deletedIds = _repository.Delete(request);
            return deletedIds;
        }

        public User UpdateRefreshToken(User user)
        {
            User updatedUser = _repository.Update(user);

            return updatedUser;
        }

        public RefreshTokenResponse? GetNewToken(RefreshTokenRequest request, IConfiguration configuration)
        {
            if (!_validator.Validate(request))
            {
                throw new InvalidRequestValueException(_validator.Errors);
            }

            var principal = JwtUtil.GetPrincipalFromExpiredToken(request.Token, configuration);
            if (principal == null) return null;

            var userIdClaim = principal.FindFirst("UserId");
            if (userIdClaim == null) return null;

            var userId = userIdClaim.Value;
            User user = this.FetchOne(int.Parse(userId));

            if(user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) 
            {
                return null;
            }

            var newToken = JwtUtil.GenerateJwtToken(user, configuration);
            var newRefreshToken = JwtUtil.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;
            User updateRefreshToken = this.UpdateRefreshToken(user);

            return new RefreshTokenResponse()
            {
                Token = newToken,
                RefreshToken = updateRefreshToken.RefreshToken!,
            };

        }
    }
}
