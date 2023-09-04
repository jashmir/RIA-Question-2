using Microsoft.AspNetCore.Mvc;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;
using DotnetCoding.Services;

namespace DotnetCoding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var customer = _customerService.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost("truncate")]
        public IActionResult TruncateCustomersTable()
        {
            _customerService.TruncateCustomersTable();
            return Ok("Customers table truncated.");
        }

        [HttpPost]
        public IActionResult AddCustomers(List<Customers> customers)
        {
            IActionResult result = _customerService.AddCustomers(customers);

            if (result is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }
            else if (result is OkResult)
            {
                return Ok("Customers added successfully."); // You can customize the success message.
            }
            else if (result is ObjectResult objectResult && objectResult.StatusCode == 206)
            {
                return new ObjectResult(objectResult.Value)
                {
                    StatusCode = 206 // HTTP 206 Partial Content
                };
            }

            // Handle other cases as needed.
            return StatusCode(500);
        }

    }

}
