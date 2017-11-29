using System.Collections.Generic;
using System.Linq;
using MaxBank2._0.Entities;

namespace MaxBank2._0.Data
{
    public class CustomerManager
    {
        private readonly List<Customer> _customers;

        //Laddar in listan med kunder genom konstruktorn
        public CustomerManager(List<Customer> list)
        {
            _customers = list;
        }

        public void CreateCustomer(Customer customer)
        {
            customer.Id = GenerateUniqueCustomerId();
            _customers.Add(customer);
        }

        public Customer GetCustomer(int id)
        {
            return GetCustomers().SingleOrDefault(cust => cust.Id == id);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _customers;
        }

        public void DeleteCustomer(Customer customer)
        {
            _customers.Remove(customer);
        }

        private int GenerateUniqueCustomerId()
        {
            var lastCustomerId = _customers.OrderBy(customer => customer.Id).LastOrDefault();

            if (lastCustomerId != null)
            {
                return lastCustomerId.Id + 1;

            }
            return 1;
        }
    }
}
