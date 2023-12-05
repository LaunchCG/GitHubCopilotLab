using Microsoft.EntityFrameworkCore;

namespace PriceService.DBModel;

[PrimaryKey(nameof(MsgId))]
public class DBBatchDetail {
    public int MsgId { get; set; }
    public int RunId { get; set; }
    public string Message { get; set; }
}
