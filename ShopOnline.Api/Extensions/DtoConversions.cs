using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            var productsDto = products?.Select(p => new ProductDto { 
                CategoryId = p.ProductCategory.Id,
                CategoryName = p.ProductCategory.Name,
                Description = p.Description,
                ImageURL = p.ImageURL,
                Name = p.Name,
                Price = p.Price,
                Qty = p.Qty
            });
            return productsDto;
        }
        public static ProductDto ConvertToDto(this Product product)
        {

            return new ProductDto
            {
                Id = product.Id,
                CategoryId = product.ProductCategory.Id,
                Name = product.Name,
                CategoryName = product.ProductCategory.Name,
                Price = product.Price,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Qty = product.Qty
            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products){
            return cartItems.Join(products, ci => ci.ProductId, p => p.Id, (c, p) => new CartItemDto { Id = c.Id,
                ProductId = c.ProductId,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductImageUrl = p.ImageURL,
                Price = p.Price,
                CartId = c.CartId,
                Qty = c.Qty,
                TotalPrice = p.Price * c.Qty
            });
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
        {
            return  new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageUrl = product.ImageURL,
                Price = product.Price,
                CartId = cartItem.CartId,
                Qty = cartItem.Qty,
                TotalPrice = product.Price * cartItem.Qty
            };
        }
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return productCategories.Select(p => new ProductCategoryDto
            {
                IconCSS = p.IconCSS,
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }
    }
}
