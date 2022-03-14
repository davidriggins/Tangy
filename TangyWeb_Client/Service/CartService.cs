using Blazored.LocalStorage;
using Tangy_Common;
using TangyWeb_Client.Service.IService;
using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Service
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;

        public CartService(ILocalStorageService localStorageService)
        {
            _localStorage = localStorageService;
        }

        public ILocalStorageService LocalStorageService { get; }



        public async Task IncrementCart(ShoppingCart cartToIncrement)
        {
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            bool itemInCart = false;

            if (cart == null)
            {
                cart = new List<ShoppingCart>();
            }

            foreach (var obj in cart)
            {
                if (obj.ProductId == cartToIncrement.ProductId && obj.ProductPriceId == cartToIncrement.ProductPriceId)
                {
                    itemInCart = true;
                    obj.Count+= cartToIncrement.Count;
                }
            }

            if (!itemInCart)
            {
                cart.Add(new ShoppingCart()
                {
                    ProductId = cartToIncrement.ProductId,
                    ProductPriceId = cartToIncrement.ProductPriceId,
                    Count = cartToIncrement.Count
                });
            }

            await _localStorage.SetItemAsync(SD.ShoppingCart, cart);

        }


        public async Task DecrementCart(ShoppingCart cartToDecrement)
        {
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);

            // If count is 0 or 1, then we remove the item
            for(int i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductId == cartToDecrement.ProductId && cart[i].ProductPriceId == cartToDecrement.ProductPriceId)
                {
                    if (cart[i].Count == 1 || cart[i].Count == 0)
                    {
                        cart.Remove(cart[i]);
                    }
                    else
                    {
                        cart[i].Count -= cartToDecrement.Count;
                    }
                }
            }

            await _localStorage.SetItemAsync(SD.ShoppingCart, cart);
        }

    }
}
