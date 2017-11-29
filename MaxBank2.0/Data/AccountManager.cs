using System.Collections.Generic;
using System.Linq;
using MaxBank2._0.Entities;

namespace MaxBank2._0.Data
{
    public class AccountManager
    {
        private readonly List<Account> _accounts;

        //Laddar in listan till accountmanager genom konstruktorn
        public AccountManager(List<Account> list)
        {
            _accounts = list;
        }


        public void CreateAccount(Account account)
        {
            account.AccountNumber = GenerateUniqueAccountNumber();
            _accounts.Add(account);
        }


        public IEnumerable<Account> GetAccounts()
        {
            return _accounts;
        }

        public void DeleteAccount(Account account)
        {
            _accounts.Remove(account);
        }

        public Account GetAccount(int accountNumber)
        {
            return _accounts.SingleOrDefault(account => account.AccountNumber == accountNumber);
        }

        public void Deposit(Account account, decimal amount)
        {
            account.Balance += amount;
        }

        public void Withdraw(Account account, decimal amount)
        {
            account.Balance -= amount;
        }

        public void Transfer(Account fromAccount, Account toAccount, decimal amount)
        {
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;
        }

        private int GenerateUniqueAccountNumber()
        {
            var lastAccount = _accounts.OrderBy(account => account.AccountNumber).LastOrDefault();

            if (lastAccount != null)
            {
                return lastAccount.AccountNumber + 1;
            }
            return 1;
        }
    }
}
