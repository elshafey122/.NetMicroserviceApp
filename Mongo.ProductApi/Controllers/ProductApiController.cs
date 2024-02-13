using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mongo.ProductApi.Model;
using Mongo.ProductApi.Services.Interfaces;

namespace Mongo.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductApiController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Product> products = await _productRepository.GetAllAsync();
                if (products == null)
                {
                    return NotFound(new Response("there is no data",true));
                }
                return Ok(new Response(products));
            }
            catch(Exception ex)
            {
                return BadRequest(new Response(ex.Message,false));
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Product product = await _productRepository.GetByIdAsync(x=>x.ProductId==id);
                if (product == null)
                {
                    return NotFound(new Response(""));
                }
                var productMapper = _mapper.Map<ProductDto>(product);
                return Ok(new Response(productMapper));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }

        [HttpPost("Create")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create([FromBody] ProductDto newproduct)
        {
            try
            {
                Product product = _mapper.Map<Product>(newproduct);
                await _productRepository.CreateAsync(product);
                return Ok(new Response(""));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ProductEdit newproduct)
        {
            try
            {
                Product oldProduct = await _productRepository.GetByIdAsync(x=>x.ProductId==newproduct.ProductId);
                if (oldProduct == null)
                    return NotFound(new Response($"there is no data with id {newproduct.ProductId}", false));

                Product productUpdated = _mapper.Map<Product>(newproduct);
                //productUpdated.ProductId = newproduct.ProductId;
                await _productRepository.Update(productUpdated);
                return Ok(new Response(""));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
            
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Product product = await _productRepository.GetByIdAsync(x=>x.ProductId==id);
                if (product == null)
                {
                    return NotFound(new Response("not found",false));
                }
                _productRepository.Delete(product);
                return Ok(new Response(""));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }
    }
}
