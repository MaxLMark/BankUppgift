namespace MaxBank2._0.Entities
{
    public class Account
    {
        // Kontonummer
        public int AccountNumber { get; set; }
        // Kundnummer på ägaren av kontot
        public int CustomerId { get; set; }
        // Saldo på bankkontot
        public decimal Balance { get; set; }

        public override string ToString()
        {
            return $"{AccountNumber}: {Balance:#0.00} kr";
        }
    }
}
