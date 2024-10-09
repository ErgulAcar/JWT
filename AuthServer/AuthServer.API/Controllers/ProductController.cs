using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product, ProductDto> _genericService;

        public ProductController(IGenericService<Product, ProductDto> genericService)
        {
            _genericService = genericService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            return ActionResultInstance(await _genericService.GetAllAsync());
        }
        
        
        [HttpGet("id")]
        public async Task<IActionResult> GetByIdProduct(int id)
        {
            return ActionResultInstance(await _genericService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _genericService.AddAsync(productDto));
        }

        [HttpPut]
        public async Task<IActionResult> PutProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _genericService.UpdateAsync(productDto, productDto.id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultInstance(await _genericService.RemoveAsync(id));
        }
    }
}
