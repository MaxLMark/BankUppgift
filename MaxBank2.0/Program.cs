using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using MaxBank2._0.Entities;

namespace MaxBank2._0
{
    class Program
    {
        /// <summary>
        /// DIctionary till de olika valen i menyn.
        /// </summary>
        private static readonly Dictionary<string, Action<Bank>> _actions = new Dictionary<string, Action<Bank>>
        {
            { "1", FindCustomers },
            { "2", GetCustomer },
            { "3", CreateCustomer },
            { "4", DeleteCustomer },
            { "5", CreateAccount },
            { "6", DeleteAccount },
            { "7", Deposit },
            { "8", Withdraw },
            { "9", Transfer},
            { "0", CloseProgram }
        };

        static void Main(string[] args)
        {

            Console.WriteLine(@"
                               ****************************
                               *VÄLKOMMEN TILL BANKAPP 1.0*
                               ****************************");



            var bank = new Bank();

            WriteCurrentBankStatus(bank);

            while (true)
            {
                Console.WriteLine(@"
                              -----HUVUDMENY-----
                              0) Avsluta och spara 
                              1) Sök kund  
                              2) Visa kundbild 
                              3) Skapa kund 
                              4) Ta bort kund
                              5) Skapa konto 
                              6) Ta bort konto 
                              7) Insättning 
                              8) Uttag 
                              9) Överföring");

                var choice = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(choice) || !_actions.ContainsKey(choice))
                {
                    Console.WriteLine("Du valde någonting som inte existerade, försök igen.");
                }
                else
                {
                    _actions[choice](bank);
                }
            }
        }

        /// <summary>
        /// Val 1
        /// </summary>
        static void FindCustomers(Bank bank)
        {
            Console.WriteLine("Vad vill du söka efter?");
            var input = Console.ReadLine();
            var customers = bank.FindCustomers(input).ToList();
            Console.WriteLine($"Hittade {customers.Count} kund(er).");
            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.Id}: {customer.OrgName}");
            }
        }

        /// <summary>
        /// Val 2
        /// </summary>
        static void GetCustomer(Bank bank)
        {
            Console.WriteLine("Vilket kundnummer vill du söka efter?");
            
            if(!int.TryParse(Console.ReadLine(), out var customerId))
            {
                Console.WriteLine("Du måste ange korrekt format på kundnummer.");
                return;
            }
            var customer = bank.GetCustomer(customerId);
            var totalSum = bank.GetTotalSaldoOfAccounts(customerId);
            Console.WriteLine(customer.ToString() + "\n" + "Totala saldot: " +totalSum );
        }

        /// <summary>
        /// Val 3
        /// </summary>
        static void CreateCustomer(Bank bank)
        {
            Console.WriteLine("***** Skapa ny kund *****");

            var newCustomer = new Customer();

            Console.WriteLine("Skriv in organisationsnamn: ");
            newCustomer.OrgName = Console.ReadLine();

            Console.WriteLine("Skriv in organisationsnummer: ");
            newCustomer.OrgNumber = Console.ReadLine();

            Console.WriteLine("Skriv in address");
            newCustomer.Address = Console.ReadLine();

            Console.WriteLine("Skriv in stad");
            newCustomer.City = Console.ReadLine();

            Console.WriteLine("Skriv in region");
            newCustomer.Region = Console.ReadLine();

            Console.WriteLine("Skriv in postnummer");
            newCustomer.Postnumber = Console.ReadLine();

            Console.WriteLine("Skriv in land");
            newCustomer.Country = Console.ReadLine();

            Console.WriteLine("Skriv in telefonnummer");
            newCustomer.Phonenumber = Console.ReadLine();

            bank.CreateCustomer(newCustomer);
            Console.WriteLine($"Skapade ny kund med kundnummer {newCustomer.Id}");
        }

        /// <summary>
        /// Val 4
        /// </summary>
        static void DeleteCustomer(Bank bank)
        {
            Console.WriteLine("**** Ta bort kund ****");

            Console.WriteLine("Vilken kund vill du ta bort? Ange kundnummer:");
            if (!int.TryParse(Console.ReadLine(), out var customerId))
            {
                Console.WriteLine("Du måste ange korrekt format på kundnummer.");
                return;
            }

            if (bank.TryDeleteCustomer(customerId))
            {
                Console.WriteLine("Kunden är nu borttagen");
            }
            else
            {
                Console.WriteLine("Kunde inte ta bort kund. Finns konton kvar som måste tas bort först.");
            }
        }

        /// <summary>
        /// Val 5
        /// </summary>
        static void CreateAccount(Bank bank)
        {
            Console.WriteLine("**** Skapa konto ****");

            Console.WriteLine("Skriv in ditt kundnummmer:");


            if (!int.TryParse(Console.ReadLine(), out var customerId))
            {
                Console.WriteLine("Du måste ange korrekt format på kundnummer.");
                return;
            }


            if (bank.GetCustomer(customerId) == null)
            {
                Console.WriteLine("Kundnumret existerar inte.");
                return;
            }

            var newAccount = new Account
            {
                CustomerId = customerId
            };

            Console.WriteLine("Hur mycket pengar vill du sätta in?");
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
            {
                Console.WriteLine("Felaktigt format på pengar.");
                return;
            }

            newAccount.Balance = amount;
            bank.CreateAccount(newAccount);

            Console.WriteLine($"Nytt konto skapades med kontonummer {newAccount.AccountNumber}");
        }

        /// <summary>
        /// Val 6
        /// </summary>
        static void DeleteAccount(Bank bank)
        {
            Console.WriteLine("**** Ta bort konto ****");
            Console.WriteLine("Vilket konto vill du ta bort? Skriv in kontonummer");
            
            if (!int.TryParse(Console.ReadLine(), out var accountNumber))
            {
                Console.WriteLine("Du har anget fel format på kontonummer.");
                return;
            }

            var account = bank.GetAccount(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Kontot fanns inte.");
                return;
            }

            if (bank.TryDeleteAccount(accountNumber))
            {
                Console.WriteLine("Kontot har tagits bort.");
            }
            else
            {
                Console.WriteLine("Kunde inte ta bort kontot. Pengar finns kvar på kontot.");
            }

        }

        /// <summary>
        /// Val 7
        /// </summary>
        static void Deposit(Bank bank)
        {
            Console.WriteLine("**** Insättning ****");

            Console.WriteLine("På vilket konto? Ange kontonummer:");
            
            if (!int.TryParse(Console.ReadLine(), out var accountNumber))
            {
                Console.WriteLine("Felaktigt formaterad kontonummer.");
                return;
            }
            var account = bank.GetAccount(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Kontot existerar inte.");
                return;
            }

            Console.WriteLine("Hur mycket vill du sätta in?");

            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
            {
                Console.WriteLine("Du har angivit fel formaterad mängd");
                return;
            }

            if (amount <= 0.00m)
            {
                Console.WriteLine("Du måste sätta in minst 0.01 kr");
                return;
            }

            
            if (bank.TryDeposit(accountNumber, amount, out var newAmount))
            {
                Console.WriteLine($"Insättningen lyckades. Kontot har nu {newAmount:#.00} kr.");
            }
            else
            {
                Console.WriteLine("Insättningen misslyckades.");
            }
        }

        /// <summary>
        /// Val 8
        /// </summary>
        static void Withdraw(Bank bank)
        {
            Console.WriteLine("**** Uttag ****");

            Console.WriteLine("Från vilket konto? Ange kontonummer:");
            
            if (!int.TryParse(Console.ReadLine(), out var accountNumber))
            {
                Console.WriteLine("Du har angivit fel formaterad kontonummer.");
                return;
            }
            var account = bank.GetAccount(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Kontot existerar inte.");
                return;
            }

            Console.WriteLine("Hur mycket vill du ta ut?");
            
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
            {
                Console.WriteLine("Du har angivit fel formaterad mängd");
                return;
            }

            if (amount <= 0.00m)
            {
                Console.WriteLine("Du måste ta ut minst 0.01 kr");
                return;
            }

            if (bank.TryWithdraw(accountNumber, amount, out var newAmount))
            {
                Console.WriteLine($"Uttaget lyckades. Kontot har nu {newAmount:#0.00} kr.");
            }
            else
            {
                Console.WriteLine($"Uttaget misslyckades. Inte tillräckligt med saldo på kontot: {newAmount:#.00}.");
            }
        }

        /// <summary>
        /// Val 9
        /// </summary>
        static void Transfer(Bank bank)
        {
            Console.WriteLine("**** Överföring ****");

            Console.WriteLine("Överföra från vilket konto?");
            
            if(!int.TryParse(Console.ReadLine(),out var fromAccountNumber))
            {
                Console.WriteLine("Kontonumret var inte formaterat korrekt.");
            }
            if (bank.GetAccount(fromAccountNumber) == null)
            {
                Console.WriteLine("Kontot existerar inte.");
                return;
            }

            Console.WriteLine("Till konto?");
            
            if (!int.TryParse(Console.ReadLine(), out var toAccountNumber))
            {
                Console.WriteLine("Felaktigt kontonummer");
                return;
            }
            if (bank.GetAccount(toAccountNumber) == null)
            {
                Console.WriteLine("Kontot existerar inte.");
                return;
            }

            Console.WriteLine("Hur mycket?");
            
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
            {
                Console.WriteLine("Felaktigt formaterad mängd");
                return; 
            }

            if (amount <= 0.00m)
            {
                Console.WriteLine("Du måste överföra minst 0.01 kr");
                return;
            }

            if (bank.TryTransfer(fromAccountNumber, toAccountNumber, amount))
            {
                Console.WriteLine("Överföringen lyckades.");
            }
            else
            {
                Console.WriteLine("Överföringen misslyckades.");
            }
        }

        /// <summary>
        /// Val 0
        /// </summary>
        static void CloseProgram(Bank bank)
        {
            Console.WriteLine("**** Avslutar programmet och genererar ny textfil ****");

            var newBankdataFileName = bank.CloseBank();
            Console.WriteLine($"Sparar till {newBankdataFileName}...");
            WriteCurrentBankStatus(bank);

            Console.WriteLine("Tryck på valfri tangent för att avsluta ...");
            Console.Read();
            Environment.Exit(0);
        }

        static void WriteCurrentBankStatus(Bank bank)
        {
            Console.WriteLine($"Antal kunder: {bank.GetCustomers().Count()}");
            Console.WriteLine($"Antal konton: {bank.GetAccounts().Count()}");
            Console.WriteLine($"Totalt saldo: {bank.GetTotalBankBalance():#.00}");
        }
    }
}
