using API.DTOs;

namespace API.Entities;

public class Basket
{
    public int BasketId { get; set; }
    public string BuyerID { get; set; }
    public List<BasketItem> Items { get; set; }=new ();

    public void AddItem(Product product, int quantity){
        if(Items.All(item=>item.ProductId!=product.ID)){
            Items.Add(new BasketItem{
                Quantity=quantity,
                Product=product
            }); 
        }
        var existingItem=Items.FirstOrDefault(item=>item.ProductId==product.ID);
        if(existingItem!=null){
            existingItem.Quantity+=quantity;
        }
    }
    public void RemoveItem(int productID,int quantity){
        var item=Items.FirstOrDefault(item=>item.ProductId==productID);
        if(item==null) return;
        if(item.Quantity<quantity) return;
        item.Quantity-=quantity;
        if(item.Quantity==0) Items.Remove(item);
    }
    public BasketDto ToBasketDto(){
        return new BasketDto{
            ID=this.BasketId,
            BuyerId=this.BuyerID,
            Items=this.Items.Select(item=>
            new BasketItemDto{
            ProductId=item.ProductId,
            Name=item.Product.Name,
            Price=item.Product.Price,
            PictureUrl=item.Product.PictureUrl,
            Brand=item.Product.Brand,
            Type=item.Product.Type,
            Quantity=item.Quantity
            }).ToList()
        };
    }
}
