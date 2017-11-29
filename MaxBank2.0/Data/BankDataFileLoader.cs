using System.Collections.Generic;
using System.Globalization;
using System.IO;
using MaxBank2._0.Entities;

namespace MaxBank2._0.Data
{
    public class BankDataFileLoader
    {
        private readonly string[] _fileLines;

        //Läser in filen genom konstruktorn
        public BankDataFileLoader()
        {
            _fileLines = File.ReadAllLines($"{Directory.GetCurrentDirectory()}/../../bankdata.txt");
        }

        //Delar upp kunder och konton baserat på längden utav listan
        public List<Account> LoadAccounts()
        {
            var accounts = new List<Account>();
            foreach (var fileLine in _fileLines)
            {
                var info = fileLine.Split(';');
                if (info.Length == 3)
                {
                    accounts.Add(new Account
                    {
                        AccountNumber = int.Parse(info[0]),
                        CustomerId = int.Parse(info[1]),
                        Balance = decimal.Parse(info[2], NumberStyles.Any, CultureInfo.InvariantCulture)
                    });
                }

            }
            return accounts;
        }

        public List<Customer> LoadCustomers()
        {
            var customers = new List<Customer>();
            foreach (var fileLine in _fileLines)
            {
                var info = fileLine.Split(';');
                if (info.Length == 9)
                {
                    customers.Add(new Customer
                    {
                        Id = int.Parse(info[0]),
                        OrgNumber = info[1],
                        OrgName = info[2],
                        Address = info[3],
                        City = info[4],
                        Region = info[5],
                        Postnumber = info[6],
                        Country = info[7],
                        Phonenumber = info[8]
                    });
                }
            }


            return customers;
        }
    }
       
}
