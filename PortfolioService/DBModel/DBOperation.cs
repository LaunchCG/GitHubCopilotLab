
using System;

namespace SALearning.DBModel 
{
    public class DBOperation
    {
        public int OperationId { get; set; }
        public int OperationType { get; set; }
        public int AccountNumber { get; set; }
        public string Symbol { get; set; }
        public decimal Shares { get; set; }
        public decimal Amount { get; set; }
        public DateOnly TransactDate { get; set; }
    }
}
