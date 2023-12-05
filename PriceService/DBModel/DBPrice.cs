using Microsoft.EntityFrameworkCore;

namespace PriceService.DBModel;

[PrimaryKey(nameof(Symbol), nameof(CloseDate))]
public class DBPrice {
    public string Symbol { get; set; }
    public DateOnly CloseDate { get; set; }
    public double Price { get; set; }
}
