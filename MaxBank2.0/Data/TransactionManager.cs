using System;
using System.IO;
using MaxBank2._0.Entities;

namespace MaxBank2._0.Data
{
    public class TransactionManager
    {
        public void CreateTransaction(TransactionType transactionType, decimal amount, int accountNumber, int? toAccountNumber = null)
        {
            var transaction = new Transaction
            {
                TransactionType = transactionType,
                AccountNumber = accountNumber,
                Amount = amount,
                TranserToAccount = toAccountNumber,
                TimeStamp = DateTime.Now
            };
            File.AppendAllLines($"{Directory.GetCurrentDirectory()}/../../TransactionLog/transaktionslogg.txt", new[] { transaction.ToString() });
        }
    }
}
