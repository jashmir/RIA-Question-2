using DotnetCoding.Core.Models;

namespace DotnetCoding.Core.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customers> GetAllCustomers();
        Customers GetCustomerById(int id);
        void AddCustomer(Customers customer);
        public bool IsCustomerIdUnique(int id);
        public void TruncateTable();
        public int GetNextCustomerId();
    }
}
