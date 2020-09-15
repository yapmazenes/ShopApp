using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {

        [HttpPost("{stockId}")]
        public async Task<IActionResult> AddOne(
            int stockId,
            [FromServices] AddToCart addToCart)
        {
            var request = new AddToCart.Request
            {
                StockId = stockId,
                Qty = 1
            };

            var success = await addToCart.Do(request);
            if (success)
            {
                return Ok("Item Added to cart");
            }
            return BadRequest("Failed to add to cart");
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> RemoveOne(
            int stockId,
            [FromServices] RemoveFromCart removeFromCart)

        {
            var request = new RemoveFromCart.Request
            {
                StockId = stockId,
                Qty = 1
            };


            var success = await removeFromCart.Do(request);
            if (success)
            {
                return Ok("Item removed from cart");
            }
            return BadRequest("Failed to remove item from cart");
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> RemoveAll(
            int stockId,
            [FromServices] RemoveFromCart removeFromCart)

        {
            var request = new RemoveFromCart.Request
            {
                StockId = stockId,
                All = true
            };


            var success = await removeFromCart.Do(request);

            if (success)
            {
                return Ok("Item removed all items from cart");
            }
            return BadRequest("Failed to remove all items from cart");
        }

        public IActionResult GetCartComponent([FromServices] GetCart _getCart)
        {
            var totalValue = _getCart.Do().Sum(x => x.RealValue * x.Qty);

            return PartialView("Components/Cart/Small", $"{totalValue} ₺");
        }
    }
}
