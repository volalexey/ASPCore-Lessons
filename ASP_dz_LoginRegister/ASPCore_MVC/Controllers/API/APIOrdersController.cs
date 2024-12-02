using ASPCore_MVC.Models;
using ASPCore_MVC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPCore_MVC.Controllers.API
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class APIOrdersController : Controller
    {
        private readonly IOrdersService? _ordersService;

        public APIOrdersController(IOrdersService? ordersService)
        {
            _ordersService = ordersService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order p)
        {
            return Json(await _ordersService?.Create(p));
        }
        [HttpGet]
        public async Task<IActionResult> Read()
        {
            return Json(await _ordersService.Read());
        }
        [HttpGet("2")]
        public async Task<IActionResult> ReadByProductId(int productId)
        {
            return Json(await _ordersService.ReadByProductId(productId));
        }
        [HttpGet("3")]
        public async Task<IActionResult> ReadByUserId(string userId)
        {
            return Json(await _ordersService.ReadByUserId(userId));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Read(int id)
        {
            return Json(await _ordersService.Read(id));
        }
        [HttpPatch]
        public async Task<IActionResult> Update(Order p)
        {
            return Json(await _ordersService?.Update(p));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _ordersService.Delete(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
