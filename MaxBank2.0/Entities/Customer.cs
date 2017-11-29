using System.Collections.Generic;

namespace MaxBank2._0.Entities
{
    public class Customer
    {
        
        // Kundnummer
        public int Id { get; set; }
        // Organisationsnummer
        public string OrgNumber { get; set; }
        // Företagsnamn
        public string OrgName { get; set; }
        // Adress
        public string Address { get; set; }
        // Stad
        public string City { get; set; }
        // Region
        public string Region { get; set; }
        // Postnummer
        public string Postnumber { get; set; }
        // Land
        public string Country { get; set; }
        // Telefonnummer
        public string Phonenumber { get; set; }

        public IEnumerable<Account> Accounts { get; set; } = new List<Account>();

        public override string ToString()
        {
            
            return $"Kundnummer: {Id}\nOrgnummer: {OrgNumber}\nNamn: {OrgName}\nAdress: {Address}\n\nKonton:\n{string.Join("\n", Accounts)} ";
        }
    }
}
