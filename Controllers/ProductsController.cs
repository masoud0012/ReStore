using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(StoreContext context,ILogger<ProductsController> logger) : ControllerBase{
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll(){
        var list=await context.Products.ToListAsync();
        return Ok(list);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(int id){
       var product=await context.Products.FirstOrDefaultAsync(p=>p.ID==id);
       if (product==null)
       {
        return BadRequest();
       }
       return product;
    }

    [HttpPost("{product}")]
    public async Task<bool> Creat(Product product){
       await context.AddAsync(product);
        await context.SaveChangesAsync();
        return true;
    }
    [HttpDelete("{id}")]
    public bool Delete(int id){
        context.Products.Remove(context.Products.FirstOrDefault(p=>p.ID==id));
        return true;
    }
    [HttpPut("{prodct}")]
    public Product UPdate(Product prodct){
        var result=context.Products.FirstOrDefault(p=>p.ID==prodct.ID);
        result.Name=prodct.Name;
        result.Description=prodct.Description;
        result.PictureUrl=prodct.PictureUrl;
        result.Price=prodct.Price;
        context.SaveChanges();
        return result;
    }
}