using Microsoft.EntityFrameworkCore;

namespace PriceService.DBModel;

[PrimaryKey(nameof(RunId))]
public class DBBatchRun {
    public int RunId { get; set; }
    public string RunType { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public int ErrorCount { get; set; }
    public int SuccessCount { get; set; }
}
