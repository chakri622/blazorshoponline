using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }
        private async Task<bool> CartItemExists(int cartId,int productId)
        {
            return await this.shopOnlineDbContext.CartItems.AnyAsync(c=> c.CartId==cartId && c.ProductId == productId);  

        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if(await CartItemExists(cartItemToAddDto.CartId,cartItemToAddDto.ProductId))
            {
                cartItemToAddDto.Qty += 1;
            }
            var item = await this.shopOnlineDbContext.Products.Where(p => p.Id == cartItemToAddDto.ProductId).Select(c => new CartItem {
                CartId = cartItemToAddDto.CartId,
                ProductId=c.Id,
                Qty = cartItemToAddDto.Qty
                

            }).SingleOrDefaultAsync();
            if (item != null)
            {
                var result = await this.shopOnlineDbContext.CartItems.AddAsync(item);
                await this.shopOnlineDbContext.SaveChangesAsync();
                return result.Entity;
            }
            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            try
            {
                var item = await this.shopOnlineDbContext.CartItems.FindAsync(id);
                if(item != null)
                {
                     this.shopOnlineDbContext.CartItems.Remove(item);
                    await this.shopOnlineDbContext.SaveChangesAsync();
                }
                return item;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await this.shopOnlineDbContext.Carts.Join(shopOnlineDbContext.CartItems,
                c => c.Id,
                ci => ci.CartId,
                (ct, ci) => new { Cart = ct, CartItem = ci })
                .Where(c => c.CartItem.Id== id).Select(c => new CartItem
                {
                    Id = c.CartItem.Id,
                    CartId = c.CartItem.CartId,
                    ProductId = c.CartItem.ProductId,
                    Qty = c.CartItem.Qty

                }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await this.shopOnlineDbContext.Carts.Join(shopOnlineDbContext.CartItems,
                c=>c.Id,
                ci=>ci.CartId,
                (ct,ci)=>new {Cart=ct,CartItem=ci})
                .Where(c=>c.Cart.UserId==userId).Select(c=>new CartItem
                {
                    Id=c.CartItem.Id,
                  CartId=c.CartItem.CartId,
                  ProductId =c.CartItem.ProductId,
                  Qty=c.CartItem.Qty

                }).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
               var cartItemDto = await this.shopOnlineDbContext.CartItems.FindAsync(id);
                if (cartItemDto != null)
                {
                    cartItemDto.Qty = cartItemQtyUpdateDto.Qty;
                    await this.shopOnlineDbContext.SaveChangesAsync();
                    return cartItemDto;
                }
                return null;


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
