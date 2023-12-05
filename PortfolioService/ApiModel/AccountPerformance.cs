using System.Collections.Generic;

namespace SALearning.ApiModel
{
    public class StockPerformance {
        public string Symbol { get; set; }
        public decimal CurrentShares { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Gain { get; set; }
    };

    public class AccountPerformance {
        public int AccountNumber { get; set; }
        public int ProfileId { get; set; }
        public string Description { get; set; }
        public decimal CashBalance { get; set; }
        public decimal StockBalance { get; set; }
        public decimal Gain { get; set; }
        public decimal StockGain { get; set; }
        public List<StockPerformance> Stocks { get; set; }
    };
}
