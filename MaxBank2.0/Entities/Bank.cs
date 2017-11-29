using System.Collections.Generic;
using System.Linq;
using MaxBank2._0.Data;

namespace MaxBank2._0.Entities
{
    public class Bank
    {
        private readonly BankDataFileManager _bankDataFileManager;
        private readonly CustomerManager _customerManager;
        private readonly AccountManager _accountManager;
        private readonly TransactionManager _transactionManager;


        public Bank()
        {
            _bankDataFileManager = new BankDataFileManager();
            _customerManager = new CustomerManager(_bankDataFileManager.LoadCustomers());
            _accountManager = new AccountManager(_bankDataFileManager.LoadAccounts());
            _transactionManager = new TransactionManager();
        }

        public void CreateCustomer(Customer customer)
        {
            _customerManager.CreateCustomer(customer);
            _accountManager.CreateAccount(new Account { CustomerId = customer.Id });
        }

        public void CreateAccount(Account account)
        {
            _accountManager.CreateAccount(account);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _accountManager.GetAccounts();
        }

        public decimal GetTotalBankBalance()
        {
            return GetAccounts().Sum(account => account.Balance);
        }

        public Customer GetCustomer(int id)
        {
            var customer = GetCustomers().SingleOrDefault(cust => cust.Id == id);
            if (customer == null)
            {
                return null;
            }
            customer.Accounts = GetAccountsForCustomer(id);
            return customer;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _customerManager.GetCustomers();
        }

        public bool TryDeleteCustomer(int customerId)
        {
            var customerAccounts = GetAccountsForCustomer(customerId);
            if (customerAccounts.Sum(account => account.Balance) > 0.00m)
            {
                return false;
            }
            if (customerAccounts.ToList().Count == 0)
            {

            var customer = GetCustomer(customerId);
            _customerManager.DeleteCustomer(customer);

            return true;
            }

            return false;
        }

        public decimal GetTotalSaldoOfAccounts(int input)
        {
            decimal totalSum = GetAccountsForCustomer(input).Sum(x => x.Balance);

            return totalSum;
        }

        public bool TryDeleteAccount(int accountNumber)
        {
            var account = GetAccount(accountNumber);
            if (account == null)
            {
                return false;
            }
            if (account.Balance > 0.00m)
            {
                return false;
            }
            _accountManager.DeleteAccount(account);
            return true;
        }

        public Account GetAccount(int accountNumber)
        {
            return GetAccounts().SingleOrDefault(account => account.AccountNumber == accountNumber);
        }

        public IEnumerable<Account> GetAccountsForCustomer(int customerId)
        {
            return GetAccounts().Where(account => account.CustomerId == customerId);
        }
       

        public IEnumerable<Customer> FindCustomers(string query)
        {
            return GetCustomers().Where(customer => customer.OrgName.ToLower().Contains(query.ToLower()));
        }

        public bool TryDeposit(int accountNumber, decimal amount, out decimal newAmount)
        {
            var account = GetAccount(accountNumber);
            if (account == null)
            {
                newAmount = 0;
                return false;
            }

            _transactionManager.CreateTransaction(TransactionType.Deposit, amount, accountNumber);
            _accountManager.Deposit(account, amount);
            newAmount = account.Balance;
            return true;
        }

        public bool TryWithdraw(int accountNumber, decimal amount, out decimal newAmount)
        {
            var account = GetAccount(accountNumber);

            if (account == null)
            {
                newAmount = 0;
                return false;
            }
            if (account.Balance < amount)
            {
                newAmount = account.Balance;
                return false;
            }

            _transactionManager.CreateTransaction(TransactionType.Withdraw, amount, accountNumber);
            _accountManager.Withdraw(account, amount);
            newAmount = account.Balance;
            return true;
        }

        public bool TryTransfer(int fromAccountNumber, int toAccountNumber, decimal amount)
        {
            var fromAccount = GetAccount(fromAccountNumber);
            var toAccount = GetAccount(toAccountNumber);

            if (fromAccount == null || toAccount == null)
            {
                return false;
            }
            if (fromAccount.Balance < amount)
            {
                return false;
            }

            _transactionManager.CreateTransaction(TransactionType.Transfer, amount, fromAccountNumber, toAccountNumber);
            _accountManager.Transfer(fromAccount, toAccount, amount);
            return true;
        }

        public string CloseBank()
        {
            return _bankDataFileManager.GenerateBankDataFile(_accountManager.GetAccounts(), _customerManager.GetCustomers());
        }
    }
}