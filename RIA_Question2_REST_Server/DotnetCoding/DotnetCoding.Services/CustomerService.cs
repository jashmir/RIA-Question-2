using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;

namespace DotnetCoding.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<Customers> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }

        public Customers GetCustomerById(int id)
        {
            return _customerRepository.GetCustomerById(id);
        }

        public void TruncateCustomersTable()
        {
            _customerRepository.TruncateTable();
        }

        public IActionResult AddCustomers(List<Customers> customers)
        {
            var validationErrors = new List<string>();
            var validCustomers = new List<Customers>();

            foreach (var customer in customers)
            {
                var customerValidationErrors = new List<string>();

                // Validation checks...

                if (string.IsNullOrWhiteSpace(customer.FirstName))
                {
                    customerValidationErrors.Add("First name is required.");
                }

                if (string.IsNullOrWhiteSpace(customer.LastName))
                {
                    customerValidationErrors.Add("Last name is required.");
                }

                if (customer.Age < 18)
                {
                    customerValidationErrors.Add("Customer must be 18 years or older.");
                }

                if (_customerRepository.IsCustomerIdUnique(customer.Id))
                {
                    customerValidationErrors.Add("Customer ID already exists.");
                }

                if (customerValidationErrors.Count > 0)
                {
                    // Add the validation errors for this customer to the list of errors.
                    validationErrors.AddRange(customerValidationErrors);
                }
                else
                {
                    // If no validation errors, add the customer to the list of valid customers.
                    validCustomers.Add(customer);
                }
            }

            if (validCustomers.Count > 0)
            {
                // If there are valid customers, add them to the database.
                foreach (var customer in validCustomers)
                {
                    _customerRepository.AddCustomer(customer);
                }

                if (validCustomers.Count == customers.Count)
                {
                    // If all customers are valid and added, return Ok.
                    return new OkResult();
                }
                else
                {
                    // If only some customers are valid, return Partial Content with the validation errors.
                    var errorMessage = string.Join("\n", validationErrors);
                    return new ObjectResult(errorMessage)
                    {
                        StatusCode = 206 // HTTP 206 Partial Content
                    };
                }
            }

            // Return a BadRequest if no valid customers are found (should not normally happen).
            return new BadRequestResult();
        }



        //public IActionResult AddCustomers(List<Customers> customers)
        //{
        //    var validationErrors = new List<string>();

        //    foreach (var customer in customers)
        //    {
        //        // Validation checks...

        //        if (string.IsNullOrWhiteSpace(customer.FirstName))
        //        {
        //            validationErrors.Add("First name is required.");
        //        }

        //        if (string.IsNullOrWhiteSpace(customer.LastName))
        //        {
        //            validationErrors.Add("Last name is required.");
        //        }

        //        if (customer.Age < 18)
        //        {
        //            validationErrors.Add("Customer must be 18 years or older.");
        //        }

        //        if (_customerRepository.IsCustomerIdUnique(customer.Id))
        //        {
        //            validationErrors.Add("Customer ID already exists.");
        //        }
        //    }

        //    if (validationErrors.Count > 0)
        //    {
        //        var errorMessage = string.Join("\n", validationErrors);
        //        return new BadRequestObjectResult(errorMessage);
        //    }


        //    foreach (var customer in customers)
        //    {
        //        _customerRepository.AddCustomer(customer);
        //    }

        //    return new OkResult();
        //}

    }
}
