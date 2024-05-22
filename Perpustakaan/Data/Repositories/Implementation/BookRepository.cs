using AutoMapper;
using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using Perpustakaan.Data.Entities;
using Perpustakaan.Data.Repositories.Abstract;
using Perpustakaan.Models.Requests;
using Perpustakaan.Utils;
using Perpustakaan.Utils.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Perpustakaan.Configurations.Db;
using Azure.Core;

namespace Perpustakaan.Data.Repositories.Implementation
{
    public class BookRepository : IBookRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly FilterUtil<Book> _filterUtil;
        private readonly JwtUtil _jwtUtil;
        private readonly UploadFileUtil _uploadFileUtil;

        public BookRepository(ApplicationDbContext db, IMapper mapper, FilterUtil<Book> filterUtil, JwtUtil jwtUtil, UploadFileUtil uploadFileUtil)
        {
            _filterUtil = filterUtil;
            _db = db;
            _mapper = mapper;
            _jwtUtil = jwtUtil;
            _uploadFileUtil = uploadFileUtil;
        }

        public List<Book> FetchAll()
        {
            var result = _db.Books.Include(obj => obj.ImageCovers).ToList();
            return result;
        }

        public  Paginated<Book> FetchAll(SearchRequest request)
        {
            List<string> searchKeywordsColumns = new List<string>();
            searchKeywordsColumns.Add("Title");
            searchKeywordsColumns.Add("Author");
            searchKeywordsColumns.Add("Description");


            Paginated<Book> query = _filterUtil.GetContent(request, searchKeywordsColumns);
            query.Data = query.Data.Include(obj => obj.ImageCovers);
            return query;
        }

        public Book? FetchOne(int id)
        {
            return _db.Books.FirstOrDefault(x => x.Id == id);
        }


        public async Task<Book> CreateAsync(BookRequest request)
        {
            string? userName = _jwtUtil.GetClaimValue(ClaimTypes.Name) ?? "";

            Book newBook = _mapper.Map<Book>(request);
            newBook.CreatedDate = DateTime.Now;
            newBook.UpdatedDate = DateTime.Now;
            newBook.CreatedBy = userName;
            newBook.UpdatedBy = userName;
            newBook.ImageCovers = new List<ImageCover>();

            foreach (var file in request.ImageCovers)
            {
                if (file.Length > 0)
                {
                    /*var uploadsFolderPath = Path.Combine(_uploadFileUtil.ContentRootPath, "uploads", "book");
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var path = $"uploads/book/{uniqueFileName}";*/

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName.Replace(" ", "_");
                    var uploadPath = _uploadFileUtil._defaultPath + "/book";

                    var newUpload = await _uploadFileUtil.Upload(file, uniqueFileName, uploadPath);

                    newBook.ImageCovers.Add(new ImageCover
                    {
                        OriginalFileName = newUpload.OriginalFileName,
                        FileName = newUpload.FileName,
                        FilePath = newUpload.FilePath,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                        CreatedBy = userName,
                        UpdatedBy = userName,
                    });
                }
            }

            var createdBook = _db.Books.Add(newBook);
            _db.SaveChanges();

            return createdBook.Entity;
        }

        public Book? Update(int id, BookRequest request)
        {
            string? userName = _jwtUtil.GetClaimValue(ClaimTypes.Name) ?? "";

            Book? book = _db.Books.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (book == null) return book;

            _mapper.Map<BookRequest, Book>(request, book);
            book.UpdatedDate = DateTime.Now;
            book.UpdatedBy = userName;

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
