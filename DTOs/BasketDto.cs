using API.Entities;

namespace API.DTOs;

public class BasketDto
{
    public int ID { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItemDto> Items { get; set; }
}
