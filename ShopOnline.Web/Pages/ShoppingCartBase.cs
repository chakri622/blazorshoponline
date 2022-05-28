using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase:ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }
       
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get;set; }
        public string ErrorMessage { get;set;}
        protected string TotalPrice { get; set; }
        protected int TotalQty { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await this.ShoppingCartService.GetItems(HardCoded.UserId);
                //CalculateCartSummaryTotals();
                CartChanged();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        public async Task UpdateQty_Input(int id)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, true);
        }
        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await this.ShoppingCartService.DeleteItem(id);
            RemoveCartItem(id);
            CartChanged();


        }
        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }

        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQty();

        }

        private void SetTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
        }
        private void SetTotalQty()
        {
            TotalQty = this.ShoppingCartItems.Sum(p=>p.Qty);
        }
        private CartItemDto GetCartItem(int id)
        {
            return this.ShoppingCartItems.FirstOrDefault(i => i.Id == id);
        }

        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);
            this.ShoppingCartItems.Remove(cartItemDto);
            CalculateCartSummaryTotals();

        }

        protected async Task UpdateQtyCartItem_Click(int id,int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };
                    var returnedUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);
                    UpdateItemTotalPrice(returnedUpdateItemDto);
                    //CalculateCartSummaryTotals();
                    CartChanged();
                    await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, false);

                }
                else
                {
                    var cartItemDto = GetCartItem(id);
                    if (cartItemDto != null)
                    {
                        cartItemDto.Qty = 1;
                        cartItemDto.Price = cartItemDto.Price;
                    }
                }
               
            }catch (Exception ex)
            {

            }
        }

        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQty);
        }


    }
}
