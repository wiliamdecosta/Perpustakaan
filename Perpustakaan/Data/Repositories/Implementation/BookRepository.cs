using AutoMapper;
using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using Perpustakaan.Data.Entities;
using Perpustakaan.Data.Repositories.Abstract;
using Perpustakaan.Models.Requests;
using Perpustakaan.Utils.Filters;
using System.Data.Entity;

namespace Perpustakaan.Data.Repositories.Implementation
{
    public class BookRepository : IBookRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly FilterUtil<Book> _filterUtil;

        public BookRepository(ApplicationDbContext db, IMapper mapper, FilterUtil<Book> filterUtil)
        {
            _filterUtil = filterUtil;
            _db = db;
            _mapper = mapper;
        }

        public List<Book> FetchAll()
        {
            return _db.Books.ToList();
        }

        public Paginated<Book> FetchAll(SearchRequest request)
        {
            List<string> searchKeywordsColumns = new List<string>();
            searchKeywordsColumns.Add("Title");
            searchKeywordsColumns.Add("Author");
            searchKeywordsColumns.Add("Description");


            Paginated<Book> query = _filterUtil.GetContent(request, searchKeywordsColumns);
            return query;
        }

        public Book? FetchOne(int id)
        {
            return _db.Books.FirstOrDefault(x => x.Id == id);
        }


        public Book Create(BookRequest request)
        {
            Book newBook = _mapper.Map<Book>(request);
            newBook.CreatedDate = DateTime.Now;
            newBook.UpdatedDate = DateTime.Now;

            var createdBook = _db.Books.Add(newBook);
            _db.SaveChanges();

            return createdBook.Entity;
        }

        public Book? Update(int id, BookRequest request, string updatedBy)
        {
            Book? book = _db.Books.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (book == null) return book;

            _mapper.Map<BookRequest, Book>(request, book);
            book.UpdatedDate = DateTime.Now;
            book.UpdatedBy = updatedBy;

            _db.Books.Update(book);
            _db.SaveChanges();
            return book;
        }

        public List<string> Delete(DeleteRequest ids)
        {
            List<string> deletedIds = [];
            foreach (string bookId in ids.Ids)
            {
                var book = _db.Books.FirstOrDefault(x => x.Id.ToString() == bookId);
                if(book == null) continue;
                _db.Books.Remove(book);
                _db.SaveChanges();
                deletedIds.Add(bookId);
            }
            return deletedIds;
        }

    }
}
