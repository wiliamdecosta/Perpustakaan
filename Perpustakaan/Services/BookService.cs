using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using JustclickCoreModules.Validators;
using Perpustakaan.Data.Entities;
using Perpustakaan.Data.Repositories.Implementation;
using Perpustakaan.Models.Requests;
using System.Security.Claims;

namespace Perpustakaan.Services
{
    public class BookService
    {
        private readonly RequestValidator _validator;
        private readonly BookRepository _repository;

        public BookService(RequestValidator validator, BookRepository repository) {
            _validator = validator;
            _repository = repository;   
        }

        public List<Book> FetchAll()
        {
            return _repository.FetchAll();
        }

        public Paginated<Book> FetchAll(SearchRequest searchRequest)
        {
            return _repository.FetchAll(searchRequest);
        }

        public Book FetchOne(int id)
        {
            Book? book = _repository.FetchOne(id);
            if (book == null)
            {
                throw new InvalidRequestValueException(null, "ID_NOT_FOUND");
            }
            return book;
        }

        public async Task<Book> Create(BookRequest request)
        {

            if (!_validator.Validate(request))
            {
                throw new InvalidRequestValueException(_validator.Errors);
            }


            Book book = await _repository.CreateAsync(request);
            return book;
        }

        public Book Update(int id, BookRequest request)
        {
            
            if (!_validator.Validate(request))
            {
                throw new InvalidRequestValueException(_validator.Errors);
            }
            
            Book? book = _repository.Update(id, request);
            if (book == null)
            {
                throw new InvalidRequestValueException(null, "INVALID_ID");
            }

            return book;
        }

        public List<string> Delete(DeleteRequest request)
        {
            List<string> deletedIds = _repository.Delete(request);
            return deletedIds;
        }
    }
}
