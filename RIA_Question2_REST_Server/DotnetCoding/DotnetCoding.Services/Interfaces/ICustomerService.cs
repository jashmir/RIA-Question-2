using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetCoding.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoding.Services.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<Customers> GetAllCustomers();
        Customers GetCustomerById(int id);
        IActionResult AddCustomers(List<Customers> customers);
        public void TruncateCustomersTable();
    }
}
