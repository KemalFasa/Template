using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Basket.Api.GrpcServices;

namespace Basket.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketController : ControllerBase
{
      private readonly IBasketRepository _repository;
      private readonly DiscountGrpcService _discountGrpcService;

    //   public BasketController(IBasketRepository repository)
    //     {
    //         _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    //     }
         public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            
        }
        


        [HttpGet("{userName}", Name = "GetBasket")]
         [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
         public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
           foreach (var item in basket.Items)
           {
            // item.ProductName
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
           }

            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]        
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

       /*  [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price            
            // Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket

            // get existing basket with total price
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish<BasketCheckoutEvent>(eventMessage);

            // remove the basket
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        } */

}
