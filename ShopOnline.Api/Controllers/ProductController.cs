using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await this.productRepository.GetItems();
                //var categories = await this.productRepository.GetCategories();
                if (products == null )
                {
                    return NotFound();
                }
                var productDtos = products.ConvertToDto();
                return Ok(productDtos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from the database");
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await this.productRepository.GetProduct(id);
                
                if (product == null )
                {
                    return BadRequest();
                }
                //var productCategory = await this.productRepository.GetCategory(product.CategoryId);
                var productDto = product.ConvertToDto();
                return Ok(productDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from the database");
            }
        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await this.productRepository.GetCategories();

                if (productCategories == null)
                {
                    return BadRequest();
                }
                
                var productCategoryDtos = productCategories.ConvertToDto();

                return Ok(productCategoryDtos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving categories from the database");
            }
        }
        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await this.productRepository.GetProductsByCategory(categoryId);

                if (products == null)
                {
                    return BadRequest();
                }
                //var categories = await this.productRepository.GetCategories();
                var productDtos = products.ConvertToDto();

                return Ok(productDtos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving categories from the database");
            }
        }
    }
}
