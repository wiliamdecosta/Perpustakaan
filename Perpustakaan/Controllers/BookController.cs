using JustclickCoreModules.Filters;
using JustclickCoreModules.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Perpustakaan.Data.Entities;
using Perpustakaan.Services;

namespace Perpustakaan.Controllers
{
    [Route("api/v1/book")]
    public class BookController : ControllerBase
    {
        private readonly BookService _service;
        public BookController(BookService service) {
            _service = service;
        }


        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public ActionResult<BaseResponse<List<Book>>> GetAllBooks([FromBody] SearchRequest request)
        {
            Paginated<Book> paginatedItem = _service.FetchAll(request);

            var responseData = BaseResponse<List<Book>>.Builder()
                .Code(StatusCodes.Status200OK)
                .Message("FETCH_ALL_USER_LIST")
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
    }
}
