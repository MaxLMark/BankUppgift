using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MaxBank2._0.Entities;

namespace MaxBank2._0.Data
{
    public class BankDataFileManager : BankDataFileLoader
    {
        public string GenerateBankDataFile(IEnumerable<Account> accounts, IEnumerable<Customer> customers)
        {
            var lines = new List<string>
            {
                customers.Count().ToString()
            };

            foreach (var customer in customers)
            {
                ///Formatera såhär:
                //1005;559268-7528;Berglunds snabbköp;Berguvsvägen  8;Luleå;;S-958 22;Sweden;0921-12 34 65
                lines.Add($"{customer.Id};{customer.OrgNumber};{customer.OrgName};{customer.Address};{customer.City};{customer.Region};{customer.Postnumber};{customer.Country};{customer.Phonenumber}");
            }

            lines.Add(accounts.Count().ToString());
            foreach (var account in accounts)
            {
                //Formatera såhär:
                //13130;1032;4807.00
                var balance = account.Balance.ToString("F", CultureInfo.InvariantCulture);
                lines.Add($"{account.AccountNumber};{account.CustomerId};{balance}");
            }

            var fileName = $"{DateTime.Now:yyyyMMdd-HHmm}.txt";
            var filePath = $"{Directory.GetCurrentDirectory()}/../../BankDataOutput/{fileName}";
            File.WriteAllLines(filePath, lines);

            return fileName;
        }
    }
}
