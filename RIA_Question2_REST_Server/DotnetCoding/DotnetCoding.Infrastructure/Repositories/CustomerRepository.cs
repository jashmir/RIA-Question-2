using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoding.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customers> GetAllCustomers()
        {
            return _context.Customers.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
        }

        public Customers GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }

        public void AddCustomer(Customers customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public bool IsCustomerIdUnique(int id)
        {
            return _context.Customers.Any(c => c.Id == id);
        }

        public void TruncateTable()
        {
            // Delete all records from the Customers table
            _context.Database.ExecuteSqlRaw("DELETE FROM Customers");
        }

        public int GetNextCustomerId()
        {
            var maxId = _context.Customers.Max(c => (int?)c.Id) ?? 0;
            return maxId + 1;
        }
    }
}
