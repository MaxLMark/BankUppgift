using System;

namespace MaxBank2._0.Entities
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public int AccountNumber { get; set; }
        public int? TranserToAccount { get; set; }
        public DateTime TimeStamp { get; set; }
        public TransactionType TransactionType { get; set; }

        public override string ToString()
        {
            return $"Datum: {TimeStamp} - Konto: {AccountNumber} - Summa: {Amount:#.00} - Till konto: {TranserToAccount} - Typ: {TransactionType}";
        }
    }

    public enum TransactionType
    {
        Withdraw = 1,
        Deposit = 2,
        Transfer = 3
    }
}
