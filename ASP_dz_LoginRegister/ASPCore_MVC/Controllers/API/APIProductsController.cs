using ASPCore_MVC.DbContext;
using ASPCore_MVC.Models;
using ASPCore_MVC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ASPCore_MVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class APIProductsController : Controller
    {
        private readonly IProductsService? _productService;

        public APIProductsController(IProductsService? productService)
        {
            _productService = productService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product p)
        {
            return Json(await _productService?.Create(p));
        }
        [HttpGet]
        public async Task<IActionResult> Read()
        {
            return Json(await _productService.Read());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Read(int id)
        {
            return Json(await _productService.Read(id));
        }
        [HttpPatch]
        public async Task<IActionResult> Update(Product p)
        {
            return Json(await _productService?.Update(p));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _productService.Delete(id))
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
