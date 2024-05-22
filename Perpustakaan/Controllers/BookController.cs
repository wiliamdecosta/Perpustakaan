using JustclickCoreModules.Filters;
using JustclickCoreModules.Requests;
using JustclickCoreModules.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perpustakaan.Data.Entities;
using Perpustakaan.Models.Requests;
using Perpustakaan.Services;
using System.Security.Claims;

namespace Perpustakaan.Controllers
{
    [Route("api/v1/book")]
    public class BookController : ControllerBase
    {
        private readonly BookService _service;
        public BookController(BookService service) {
            _service = service;
        }


        [HttpPost("all", Name ="GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public ActionResult<BaseResponse<List<Book>>> GetAllBooks([FromBody] SearchRequest request)
        {
            Paginated<Book> paginatedItem = _service.FetchAll(request);

            var responseData = BaseResponse<List<Book>>.Builder()
                .Code(StatusCodes.Status200OK)
                .Message("FETCH_ALL_BOOK_LIST")
                .Data(paginatedItem.Data.ToList())
                .Page(new PageResponse()
                {
                    Total = paginatedItem.TotalCount,
                    Size = paginatedItem.PageSize,
                    TotalPage = paginatedItem.TotalPages,
                    Current = paginatedItem.PageNumber,
                })
                .Build();

            return Ok(responseData);
        }

        [HttpGet("{id:int}", Name = "GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public ActionResult<BaseResponse<Book>> GetBook(int id)
        {
            var book = _service.FetchOne(id);
            var responseData = BaseResponse<Book>.Builder()
                .Code(StatusCodes.Status200OK)
                .Message("FETCH_ONE_BOOK")
                .Data(book)
                .Build();

            return Ok(responseData);
        }

        [HttpPost("create", Name = "CreateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<BaseResponse<Book>>> CreateBook([FromForm] BookRequest request)
        {
            Book book = await _service.Create(request);

            var responseData = BaseResponse<Book>.Builder()
                .Code(StatusCodes.Status200OK)
                .Message("CREATE_BOOK_SUCCESS")
                .Data(book)
                .Build();

            return Ok(responseData);
        }

        [HttpPut("{id:int}", Name = "UpdateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public ActionResult<BaseResponse<Book>> UpdateBook(int id, [FromBody] BookRequest bookRequest)
        {
            var book = _service.Update(id, bookRequest);
            var responseData = BaseResponse<Book>.Builder()
                .Code(StatusCodes.Status200OK)
                .Message("UPDATE_BOOK_SUCCESS")
                .Data(book)
                .Build();

            return Ok(responseData);
        }


        [HttpPost("delete", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public ActionResult<BaseResponse<List<string>>> DeleteBook([FromBody] DeleteRequest ids)
        {
            var deletedIds = _service.Delete(ids);
            var responseData = BaseResponse<List<string>>.Builder()
                .Code(StatusCodes.Status200OK)
                .Message("VILLA_DELETED")
                .Data(deletedIds)
                .Build();

            return Ok(responseData);
        }

    }
}
