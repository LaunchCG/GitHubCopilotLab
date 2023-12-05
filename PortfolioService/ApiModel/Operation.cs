using System;
using System.ComponentModel.DataAnnotations;

namespace SALearning.ApiModel
{
    public enum OperationType : int
    {
        BuyStock = 0,
        SellStock = 1,
        Deposit = 2,
        Withdrawal = 3,
    }

    public class Operation
    {
        [Required]
        public int OperationId { get; set; }
        [Required]
        public OperationType OperationType { get; set; }
        public string Symbol { get; set; }
        public decimal Shares { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactDate { get; set; }
    }
}
