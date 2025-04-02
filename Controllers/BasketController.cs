using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

public class BasketController(StoreContext context) : BaseApiController
{
    [HttpGet(Name =nameof(GetBaskets))]
    public async Task<ActionResult<BasketDto>> GetBaskets()
    {
        var basket = await RetrieveBasket();
        if (basket == null) return NotFound();
        var basketDto=basket.ToBasketDto();
        return Ok(basketDto);
    }
    [HttpPost]
    public async Task<ActionResult<BasketDto>> AddItemToBasket(int productID,int quantity){
        
       var basket= await RetrieveBasket();
        if(basket==null) basket=CreateBasket();

        var product=await context.Products.FindAsync(productID);
        if(product==null) return NotFound();

        basket!.AddItem(product,quantity);
        var result= await context.SaveChangesAsync()>0;
        if(result) return CreatedAtRoute(nameof(GetBaskets),basket.ToBasketDto());

        return BadRequest(new ProblemDetails{Title="Problem saving items to basket!"});
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveBasketItem(int productId,int quantity){
        var basket=await RetrieveBasket();
        if(basket==null) return NotFound();
        basket.RemoveItem(productId,quantity);
       var result= await context.SaveChangesAsync()>0;
       if(result) return Ok();
        return BadRequest(new ProblemDetails{Title="Problem removing item from the basket!"});
    }
        private async Task<Basket?> RetrieveBasket()
    {
        return await context.baskets
               .Include(i => i.Items)
               .ThenInclude(p => p.Product)
               .FirstOrDefaultAsync(x => x.BuyerID == Request.Cookies["buyerId"]);
    }
        private Basket? CreateBasket()
    {
        var buyerId=Guid.NewGuid().ToString();
        var cookieOptions=new CookieOptions{
        Expires = DateTime.Now.AddDays(30),
        IsEssential = true
        };
        Response.Cookies.Append("buyerId",buyerId,cookieOptions);       
        var basket= new Basket{
            BuyerID=buyerId
        };
        context.baskets.Add(basket);
        return basket;
    }
}