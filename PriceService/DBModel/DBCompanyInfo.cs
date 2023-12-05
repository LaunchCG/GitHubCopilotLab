using Microsoft.EntityFrameworkCore;

namespace PriceService.DBModel;

[PrimaryKey(nameof(Symbol))]
public class DBCompanyInfo {
    public string Symbol { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsValid { get; set; }
}
